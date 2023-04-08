using System;
using System.Threading;

class Program2
{
    static TrafficLight[] trafficLights = new TrafficLight[4];
    static object lockObject = new object();

    static void Main(string[] args)
    {
        for (int i = 0; i < 4; i++)
        {
            trafficLights[i] = new TrafficLight();
        }

        Thread[] threads = new Thread[4];

        for (int i = 0; i < 4; i++)
        {
            int index = i;

            threads[i] = new Thread(() => TrafficFlow(index));
            threads[i].Start();
        }

        for (int i = 0; i < 4; i++)
        {
            threads[i].Join();
        }
    }

    static void TrafficFlow(int index)
    {
        while (true)
        {
            lock (lockObject)
            {
                if (trafficLights[index].CanEnter())
                {
                    trafficLights[index].Enter();
                }
            }

            Thread.Sleep(1000);

            lock (lockObject)
            {
                trafficLights[index].Exit();
            }

            Thread.Sleep(1000);
        }
    }
}

class TrafficLight
{
    const int MAX_CARS = 5;
    int cars;

    public bool CanEnter()
    {
        return cars < MAX_CARS;
    }

    public void Enter()
    {
        cars++;
        Console.WriteLine($"Car entered intersection, cars in intersection: {cars}");
    }

    public void Exit()
    {
        cars--;
        Console.WriteLine($"Car exited intersection, cars in intersection: {cars}");
    }
}
