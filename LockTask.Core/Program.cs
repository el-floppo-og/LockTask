namespace LockTask.Core;

class Program
{
    static void Main()
    {
        Server.AddToCount(1);
        Server.AddToCount(2);
        
        var count = Server.GetCount();
        Console.WriteLine($"Значение count: {count}");
        
        Console.ReadLine();
    }
}