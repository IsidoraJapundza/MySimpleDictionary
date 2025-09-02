using MySimpleDictionary_IsidoraJapundza;
using System;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        const int N = 10_000; // 100_000

        var myDict = new MySimpleDictionary<int, int>();
        var sysDict = new Dictionary<int, int>();

        var sw = new Stopwatch();

        #region Test ADD
        sw.Start();
        for (int i = 0; i < N; i++)
            myDict.Add(i, i);
        sw.Stop();
        Console.WriteLine($"MySimpleDictionary Add: {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        for (int i = 0; i < N; i++)
            sysDict.Add(i, i);
        sw.Stop();
        Console.WriteLine($"System.Dictionary Add: {sw.ElapsedMilliseconds} ms");
        #endregion

        #region Test ACCESS
        sw.Restart();
        for (int i = 0; i < N; i++)
        {
            var x = myDict[i];
        }
        sw.Stop();
        Console.WriteLine($"MySimpleDictionary Access: {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        for (int i = 0; i < N; i++)
        {
            var x = sysDict[i];
        }
        sw.Stop();
        Console.WriteLine($"System.Dictionary Access: {sw.ElapsedMilliseconds} ms");
        #endregion

    }
}
