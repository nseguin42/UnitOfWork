using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace UnitOfWork.SourceGenerators;

[Generator]
public sealed class DapperSourceGenerator : IIncrementalGenerator
{
    private static string? s_generatedCodeAttribute;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        s_generatedCodeAttribute ??= GetGeneratedCodeAttribute();
        var compilationProvider = context.CompilationProvider;
        context.RegisterSourceOutput(compilationProvider, RegisterSourceOutput);
    }

    private static void RegisterSourceOutput(
        SourceProductionContext context,
        Compilation compilation)
    {
        INamedTypeSymbol? dbSession = compilation.GetTypeByMetadataName("UnitOfWork.DbSession`1");
        if (dbSession is null)
        {
            return;
        }

        INamedTypeSymbol? sqlMapper = compilation.GetTypeByMetadataName("Dapper.SqlMapper");
        if (sqlMapper is null)
        {
            return;
        }

        var sqlMapperPublicMethodSymbols = sqlMapper.GetMembers()
            .OfType<IMethodSymbol>()
            .Where(m => m.DeclaredAccessibility == Accessibility.Public)
            .ToList();

        var dbSessionMethods = sqlMapperPublicMethodSymbols.Select(GetMethodString)
            .Where(s => s is not null)
            .ToList();

        var dbSessionMethodsSource = string.Join("\n", dbSessionMethods);
        var source = $$"""
        // Auto-generated code
        namespace {{dbSession.ContainingNamespace.ToDisplayString()}} {
            {{s_generatedCodeAttribute}}
            public partial class {{dbSession.Name}} {
                {{dbSessionMethodsSource}}
            }
        }
        """;

        context.AddSource("DbSession111299009.g.cs", source);
    }

    static string? GetMethodString(IMethodSymbol method)
    {
        var returnType = method.ReturnType.ToDisplayString();
        var parameters = method.Parameters.ToList();
        var indexOfDbConnectionParameter = parameters.FindIndex(
            p => p.Type.Name == "IDbConnection");

        if (indexOfDbConnectionParameter == -1)
        {
            return null;
        }

        var indexOfDbTransactionParameter = parameters.FindIndex(
            p => p.Type.Name == "IDbTransaction");

        if (indexOfDbTransactionParameter == -1)
        {
            return null;
        }

        var parameterStrings
            = parameters.Select(p => $"{p.Type.ToDisplayString()} {p.Name}").ToList();

        parameterStrings.RemoveAt(indexOfDbConnectionParameter);
        parameterStrings.RemoveAt(indexOfDbTransactionParameter);
        var argumentStrings = parameters.Select(p => p.Name).ToList();
        argumentStrings[indexOfDbConnectionParameter] = "Connection";
        argumentStrings[indexOfDbTransactionParameter] = "Transaction";
        var methodString = $@"
        public static {returnType} {method.Name}({string.Join(", ", parameterStrings)})
        {{
            return SqlMapper.{method.Name}({string.Join(", ", argumentStrings)});
        }}
";

        return methodString;
    }

    string GetGeneratedCodeAttribute()
    {
        var thisAssembly = GetType().Assembly.GetName();
        var version = thisAssembly.Version;
        return @$"[System.CodeDom.Compiler.GeneratedCode(""{thisAssembly.Name}"", ""{version}"")]";
    }
}
