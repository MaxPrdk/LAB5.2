using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static Queue<int> sharedQueue = new Queue<int>();
    static object lockObject = new object();

    static void Main(string[] args)
    {
        Thread producerThread = new Thread(Producer);
        Thread consumerThread = new Thread(Consumer);

        producerThread.Start();
        consumerThread.Start();

        producerThread.Join();
        consumerThread.Join();
    }

    static void Producer()
    {
        Random rand = new Random();

        for (int i = 0; i < 10; i++)
        {
            int number = rand.Next(100);
            Console.WriteLine($"Producing {number}");

            lock (lockObject)
            {
                sharedQueue.Enqueue(number);
                Monitor.Pulse(lockObject);
            }

            Thread.Sleep(100);
        }

        lock (lockObject)
        {
            sharedQueue.Enqueue(-1);
            Monitor.Pulse(lockObject);
        }
    }

    static void Consumer()
    {
        while (true)
        {
            int number;

            lock (lockObject)
            {
                while (sharedQueue.Count == 0)
                {
                    Monitor.Wait(lockObject);
                }

                number = sharedQueue.Dequeue();

                if (number == -1)
                {
                    return;
                }
            }

            Console.WriteLine($"Consuming {number}");
        }
    }
}
