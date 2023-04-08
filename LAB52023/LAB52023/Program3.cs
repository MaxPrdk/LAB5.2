using System;
using System.Threading;

class Program3
{
    static object lockObject = new object();

    static void Main(string[] args)
    {
        int[] array = { 7, 2, 1, 6, 8, 5, 3, 4 };
        Console.WriteLine("Unsorted array: " + string.Join(", ", array));

        QuickSort(array, 0, array.Length - 1);

        Console.WriteLine("Sorted array: " + string.Join(", ", array));
    }

    static void QuickSort(int[] array, int left, int right)
    {
        if (left < right)
        {
            int pivot = Partition(array, left, right);

            Thread leftThread = null;
            Thread rightThread = null;

            lock (lockObject)
            {
                if (left < pivot - 1)
                {
                    leftThread = new Thread(() => QuickSort(array, left, pivot - 1));
                    leftThread.Start();
                }

                if (pivot + 1 < right)
                {
                    rightThread = new Thread(() => QuickSort(array, pivot + 1, right));
                    rightThread.Start();
                }
            }

            leftThread?.Join();
            rightThread?.Join();
        }
    }

    static int Partition(int[] array, int left, int right)
    {
        int pivot = array[left];
        int i = left;
        int j = right;

        while (true)
        {
            while (array[i] < pivot)
            {
                i++;
            }

            while (array[j] > pivot)
            {
                j--;
            }

            if (i >= j)
            {
                return j;
            }

            Swap(array, i, j);
        }
    }

    static void Swap(int[] array, int i, int j)
    {
        int temp = array[i];
        array[i] = array[j];
        array[j] = temp;
    }
}
