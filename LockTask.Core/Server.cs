namespace LockTask.Core;

/// <summary>
/// Сервер в виде статического класса
/// </summary>
public static class Server
{
    private static int _count = 0;
    private static readonly ReaderWriterLockSlim ReaderWriterLock = new();
    
    /// <summary>
    /// Получить значение count
    /// </summary>
    /// <returns>Значение count</returns>
    public static int GetCount()
    {
        ReaderWriterLock.EnterReadLock();
        
        try
        {
            return _count;
        }
        finally
        {
            ReaderWriterLock.ExitReadLock();
        }
    }
    
    /// <summary>
    /// Увеличить значение count
    /// </summary>
    /// <param name="value">Значение, на которое увеличивается count</param>
    public static void AddToCount(int value)
    {
        ReaderWriterLock.EnterWriteLock();
        
        try
        {
            _count += value;
        }
        finally
        {
            ReaderWriterLock.ExitWriteLock();
        }
    }
}