using System;
using System.Collections.Generic;
using MultiThreading.TaskMulth;

namespace MultiThreading
{
    class Program
    {
        static void Main(string[] args)
        {
            var multiThreadWorker = new MultiThreadWorker();
            //var leftMatrix = new IntegerMatrix(2, 3);
            //leftMatrix.InitializeMatrixWithRandomIntegers();
            //var rightMatrix = new IntegerMatrix(3, 4);
            //rightMatrix.InitializeMatrixWithRandomIntegers();

            //multiThreadWorker.MultiTaskExecute();

            //multiThreadWorker.SequenceTaskExecute();

            //var resultingMatrix = multiThreadWorker.MultiplyMatrixInParallel(leftMatrix, rightMatrix);
            //for (int i = 0; i < resultingMatrix.RowsNumber; i++)
            //{
            //    for(int j = 0; j < resultingMatrix.ColumnsNumber; j++)
            //        Console.Write($"{resultingMatrix[i,j]} ");

            //    Console.WriteLine();
            //}
            
            //multiThreadWorker.RecursiveThreadsExecute(23, 10);

            //multiThreadWorker.RecursiveThreadPoolsTasksExecute(23);

            //var list = new List<int>() {2, 4, 8};
            //multiThreadWorker.ChangeReadCollectionInParallel(list);

            multiThreadWorker.TaskWithChildExecute();

            Console.ReadLine();
        }
    }
}
