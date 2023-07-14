using System.Collections;
using System.Collections.Concurrent;

namespace UnitOfWork.Sample.DAL;

public sealed class WorkQueue<T> : WorkQueue, IReadOnlyList<T>
{
    public WorkQueue(
        IEnumerable<T> unitsOfWork,
        Action<T>? beforeCommit = null,
        Action<T>? afterCommit = null,
        Action<T>? beforeRollback = null,
        Action<T>? afterRollback = null) =>
        Fill(unitsOfWork, beforeCommit, afterCommit, beforeRollback, afterRollback);

    public T this[int index] => (T)base[index];

    public new IEnumerator<T> GetEnumerator() => this.Cast<T>().GetEnumerator();

    public static WorkQueue<T> Create(
        IEnumerable<T> unitsOfWork,
        Action<T>? beforeCommit = null,
        Action<T>? afterCommit = null,
        Action<T>? beforeRollback = null,
        Action<T>? afterRollback = null) =>
        new(unitsOfWork, beforeCommit, afterCommit, beforeRollback, afterRollback);
}

public class WorkQueue : IUnitOfWork, ICollection, IReadOnlyList<IUnitOfWork>
{
    private readonly ConcurrentQueue<UnitOfWorkAction> _queue = new();
    private bool _isDisposed;

    public WorkQueue(IEnumerable<IUnitOfWork> unitsOfWork) =>
        Fill(unitsOfWork.Select(u => UnitOfWorkAction.Create(u)));

    protected WorkQueue()
    {
    }

    public int Count => _queue.Count;
    public bool IsSynchronized => ((ICollection)_queue).IsSynchronized;

    public object SyncRoot => ((ICollection)_queue).SyncRoot;

    public IUnitOfWork this[int index] => _queue.ElementAt(index);

    public void CopyTo(Array array, int index) => ((ICollection)_queue).CopyTo(array, index);

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_queue).GetEnumerator();

    public IEnumerator<IUnitOfWork> GetEnumerator() => _queue.Cast<IUnitOfWork>().GetEnumerator();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Commit() => DoForEach(c => c.Commit());

    public void Rollback() => DoForEachAndDequeue(c => c.Rollback());

    public static WorkQueue Create<T>(
        IEnumerable<T> unitsOfWork,
        Action<T>? beforeCommit = null,
        Action<T>? afterCommit = null,
        Action<T>? beforeRollback = null,
        Action<T>? afterRollback = null)
    {
        var queue = new WorkQueue();
        queue.Fill(unitsOfWork, beforeCommit, afterCommit, beforeRollback, afterRollback);
        return queue;
    }

    protected void Fill<T>(
        IEnumerable<T> unitsOfWork,
        Action<T>? beforeCommit = null,
        Action<T>? afterCommit = null,
        Action<T>? beforeRollback = null,
        Action<T>? afterRollback = null)
    {
        foreach (var unitOfWork in unitsOfWork)
        {
            _queue.Enqueue(
                UnitOfWorkAction.Create(
                    unitOfWork,
                    beforeCommit,
                    afterCommit,
                    beforeRollback,
                    afterRollback));
        }
    }

    protected virtual void Dispose(bool isDisposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (isDisposing)
        {
            DoForEachAndDequeue(c => c.Dispose());
        }

        _isDisposed = true;
    }

    private void DoForEach(Action<IUnitOfWork> action)
    {
        foreach (var unit in _queue)
        {
            action(unit);
        }
    }

    private void DoForEachAndDequeue(Action<IUnitOfWork> action)
    {
        while (_queue.TryDequeue(out var connection))
        {
            action(connection);
        }

        if (_queue.IsEmpty)
        {
            return;
        }

        SpinWait spinWait = default;
        do
        {
            spinWait.SpinOnce();
            while (_queue.TryDequeue(out var connection))
            {
                action(connection);
            }
        }
        while (!_queue.IsEmpty);
    }

    private readonly record struct UnitOfWorkAction(
        object? UnitOfWork,
        Action<object?>? ActionBeforeCommit,
        Action<object?>? ActionAfterCommit,
        Action<object?>? ActionBeforeRollback,
        Action<object?>? ActionAfterRollback) : IUnitOfWork
    {
        public void Dispose()
        {
            if (UnitOfWork is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        public void Commit()
        {
            ActionBeforeCommit?.Invoke(UnitOfWork);
            if (UnitOfWork is IUnitOfWork unitOfWork)
            {
                unitOfWork.Commit();
            }

            ActionAfterCommit?.Invoke(UnitOfWork);
        }

        public void Rollback()
        {
            ActionBeforeRollback?.Invoke(UnitOfWork);
            if (UnitOfWork is IUnitOfWork unitOfWork)
            {
                unitOfWork.Rollback();
            }

            ActionAfterRollback?.Invoke(UnitOfWork);
        }

        public static UnitOfWorkAction Create<T>(
            T unitOfWork,
            Action<T>? beforeCommit = null,
            Action<T>? afterCommit = null,
            Action<T>? beforeRollback = null,
            Action<T>? afterRollback = null)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork);
            return new UnitOfWorkAction(
                unitOfWork,
                obj => beforeCommit?.Invoke((T)obj!),
                obj => afterCommit?.Invoke((T)obj!),
                obj => beforeRollback?.Invoke((T)obj!),
                obj => afterRollback?.Invoke((T)obj!));
        }
    }
}
