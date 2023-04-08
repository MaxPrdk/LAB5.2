using System;
using System.Threading;

class Program4
{
    static Semaphore semaphore;
    static int numThreadsRunning;

    static void Main(string[] args)
    {
        int[,] matrixA = { { 1, 2, 3 }, { 4, 5, 6 } };
        int[,] matrixB = { { 7, 8 }, { 9, 10 }, { 11, 12 } };
        int[,] result = new int[matrixA.GetLength(0), matrixB.GetLength(1)];

        
        semaphore = new Semaphore(1, 1);
        
        numThreadsRunning = 0;

        
        for (int i = 0; i < matrixA.GetLength(0); i++)
        {
            for (int j = 0; j < matrixB.GetLength(1); j++)
            {
                while (numThreadsRunning >= Environment.ProcessorCount)
                {
                    
                    Thread.Sleep(100);
                }

               
                Interlocked.Increment(ref numThreadsRunning);

                
                ThreadPool.QueueUserWorkItem(new WaitCallback(CalculateElement), new Tuple<int, int, int[,]>(i, j, result));
            }
        }

        
        while (numThreadsRunning > 0)
        {
            Thread.Sleep(100);
        }

       
        for (int i = 0; i < result.GetLength(0); i++)
        {
            for (int j = 0; j < result.GetLength(1); j++)
            {
                Console.Write(result[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    static void CalculateElement(object state)
    {
        Tuple<int, int, int[,]> tuple = (Tuple<int, int, int[,]>)state;
        int i = tuple.Item1;
        int j = tuple.Item2;
        int[,] result = tuple.Item3;

        int sum = 0;
        for (int k = 0; k < result.GetLength(1); k++)
        {
            sum += matrixA[i, k] * matrixB[k, j];
        }

        semaphore.WaitOne();

       
        result[i, j] = sum;

       
        semaphore.Release();

        
        Interlocked.Decrement(ref numThreadsRunning);
    }
}
