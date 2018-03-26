using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using MultiThreading.TaskMulth.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.TaskMulth
{
    public class MultiThreadWorker
    {

        /// <summary>
        /// Minimum value of array or matrix items
        /// </summary>
        private const int MIN_RANDOM_NUMBER = -100;

        /// <summary>
        /// Maximum value of array or matrix items
        /// </summary>
        private const int MAX_RANDOM_NUMBER = 100;

        private readonly Random randomGenerator = new Random();

        #region Subtask 1

        /// <summary>
        /// Run 100 tasks with iteration until 1000.
        /// </summary>
        public void MultiTaskExecute()
        {
            var tasks = new List<Task>();

            for (var i = 0; i < 100; i++)
            {
                tasks.Add(Task.Run(() => PrintTaskMessage(Task.CurrentId)));
            }

            //for (var i = 0; i < 100; i++)
            //{
            //    Task.Run(() => PrintTaskMessage(Task.CurrentId));
            //}

            Task.WaitAll(tasks.ToArray(100)); // wait all or do nothing if Task.WaitAll() ?
        }

        /// <summary>
        /// Print the current task number 1000 times.
        /// </summary>
        /// <param name="taskNumber">A number of the current task.</param>
        private void PrintTaskMessage(int? taskNumber)
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine($"Task {taskNumber.GetValueOrDefault()} - iteration: {i}");
            }
        }

        #endregion

        #region Subtask 2

        /// <summary>
        /// Create and run a sequence of 4 tasks, that provides simple operations with array.
        /// </summary>
        public void SequenceTaskExecute()
        {
            Task.Factory.StartNew(GenerateIntegerArray)
                .ContinueWith(task =>
                {
                    var multiplier = randomGenerator.Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER);
                    var taskResult = new ConcurrentBag<int>(task.Result.Select(element => element * multiplier));

                    Console.WriteLine("------------Task 2------------");
                    Console.WriteLine($"Multiplier {multiplier}");
                    Console.WriteLine(taskResult.ToString(Environment.NewLine));

                    return taskResult;
                })
                .ContinueWith(task =>
                {
                    var taskResult = new ConcurrentBag<int>(task.Result.OrderBy(element => element));
                    Console.WriteLine("------------Task 3------------");
                    Console.WriteLine(taskResult.ToString(Environment.NewLine));
                    return taskResult;
                })
                .ContinueWith(task =>
                {
                    Console.WriteLine("------------Task 4------------");
                    Console.WriteLine(task.Result.Average());
                });

        }

        /// <summary>
        /// Generates an array of 10 random integers.
        /// </summary>
        /// <returns>The array of 10 random integers.</returns>
        private int[] GenerateIntegerArray()
        {
            int[] result = Enumerable
                .Repeat(0, 10)
                .Select(i => randomGenerator.Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER))
                .ToArray();

            Console.WriteLine("------------Task 1------------");
            foreach (var element in result)
            {
                Console.WriteLine(element);
            }

            return result;
        }

        #endregion

        #region Subtask 3

        /// <summary>
        /// Multiplies two integer matrixes.
        /// </summary>
        /// <param name="left">The first matrix.</param>
        /// <param name="right">The second matrix.</param>
        /// <returns>The result of multiply.</returns>
        public IntegerMatrix MultiplyMatrixInParallel(IntegerMatrix left, IntegerMatrix right)
        {
            if (left.ColumnsNumber != right.RowsNumber)
                //TODO: Logger Warn or rewrite logic
                return null;

            var result = new IntegerMatrix(left.RowsNumber, right.ColumnsNumber);

            Parallel.For(0, left.RowsNumber, i =>
                {
                    for (int j = 0; j < right.ColumnsNumber; ++j)
                    for (int k = 0; k < left.ColumnsNumber; ++k)
                        result[i, j] += left[i, k] * right[k, j];
                }
            );

            return result;
        }

        #endregion

        #region Subtask 4 

        /// <summary>
        /// Recursive decrement <c>thredState</c> parametr by 10 threads.
        /// </summary>
        /// <param name="threadState">The initial state of the thread.</param>
        public void RecursiveThreadsExecute(int threadState)
        {
            RecursiveThreadsExecute(threadState, 10);
        }

        /// <summary>
        /// Recursive decrement <c>thredState</c> parametr.
        /// </summary>
        /// <param name="threadState">The initial state of the thread.</param>
        /// <param name="threadCount">The number of threads that should be perform work.</param>
        public void RecursiveThreadsExecute(int threadState, int threadCount)
        {
            if (threadCount == 0)
                return;

            var thread = new Thread(() => threadState = DecrementThreadState(threadState));
            thread.Start();
            thread.Join();

            RecursiveThreadsExecute(threadState, threadCount - 1);
        }

        /// <summary>
        /// Recursive execute <value>threadCounter</value> threads that decrement threadState.
        /// </summary>
        /// <param name="threadState">The state of the thread that is decremented.</param>
        private int DecrementThreadState(object threadState)
        {
            int currentState = (int) threadState;
            currentState--;
            Console.WriteLine(currentState);
            return currentState;
        }

        #endregion

        #region Subtask 5 

        /// <summary>
        ///  Recursive decrement <c>thredState</c> parametr by 10 threads.
        /// </summary>
        /// <param name="threadState">The initial state of the thread</param>
        public void RecursiveThreadPoolsTasksExecute(int threadState)
        {
            Semaphore semaphore = new Semaphore(1, 10);

            RecursiveThreadPoolsTasksExecute(ref threadState, 10, semaphore);
        }

        /// <summary>
        ///  Recursive decrement <c>thredState</c> parametr.
        /// </summary>
        /// <param name="threadState">The initial state of the thread.</param>
        /// <param name="threadCount">The number of threads that should be perform work.</param>
        /// <param name="semaphore">Syncronization object.</param>
        private void RecursiveThreadPoolsTasksExecute(ref int threadState, int threadCount, Semaphore semaphore)
        {
            if (threadCount < 0)
                throw new ArgumentOutOfRangeException(nameof(threadCount));
            if (threadCount == 0)
                return;

            semaphore.WaitOne();

            threadState--;
            Console.WriteLine(threadState);
            int currentThreadState = threadState;
            threadCount--;

            semaphore.Release();

            ThreadPool.QueueUserWorkItem(action =>
                RecursiveThreadPoolsTasksExecute(ref currentThreadState, threadCount, semaphore));

        }

        #endregion

        #region Subtask 6 

        /// <summary>
        /// Concurent reading/editing a shared between tasks collection.
        /// </summary>
        /// <param name="list">The shared collection.</param>
        public void ChangeReadCollectionInParallel(List<int> list)
        {
            ReaderWriterLockSlim listLockSlim = new ReaderWriterLockSlim();

            var readCollection = new Action(() =>
            {
                listLockSlim.EnterReadLock();
                try
                {
                    Console.WriteLine(
                        $"Task {Task.CurrentId} reading. ThreadId {Thread.CurrentThread.ManagedThreadId}");
                    list.ForEach(Console.WriteLine);
                }
                finally
                {
                    listLockSlim.ExitReadLock();
                }

            });

            Task.Run(() =>
            {

                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"Task {Task.CurrentId} adding. ThreadId {Thread.CurrentThread.ManagedThreadId}");
                    listLockSlim.EnterWriteLock();
                    try
                    {
                        list.Add(randomGenerator.Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER));
                    }
                    finally
                    {
                        listLockSlim.ExitWriteLock();
                    }
                    var readTask = Task.Run(readCollection);
                    readTask.Wait();
                }
            });
        }

        #endregion

        #region Subtask 7

        /// <summary>
        /// Executes tasks with continuations that are started by different criterias.
        /// </summary>
        public void TaskWithChildExecute()
        {
            //point a ? 
            Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                        Console.WriteLine($"Task {Task.CurrentId} iteration {i}");
                })
                .ContinueWith(task =>
                {
                    for (int i = 0; i < 20; i++)
                        Console.WriteLine($"Task {Task.CurrentId} iteration {i}");

                }, TaskContinuationOptions.None);


            //point b
            Task.Run(() => throw new Exception($"Task {Task.CurrentId} failed ..."))
                .ContinueWith(task =>
                {
                    if (task.Exception != null) Console.WriteLine($"{task.Exception.Message}");
                    Console.WriteLine("Parent task failed. Rolling back...");
                }, TaskContinuationOptions.OnlyOnFaulted);


            //point c
            Task.Run(() =>
                {
                    Console.WriteLine($"Thread ID {Thread.CurrentThread.ManagedThreadId} Task ID {Task.CurrentId}");
                    throw new Exception($"Task {Task.CurrentId} failed ...");
                })
                .ContinueWith(task =>
                {
                    if (task.Exception != null)
                    {
                        Console.WriteLine($" Exception Message: {task.Exception.Message} Thread ID: {Thread.CurrentThread.ManagedThreadId}  Current Task ID: {Task.CurrentId}");
                    }

                    Console.WriteLine("Parent task failed. Rolling back...");
                }, TaskContinuationOptions.ExecuteSynchronously);

            //point d
            var tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            var firstTask = Task.Run(() =>
            {
                Console.WriteLine($"Task {Task.CurrentId} {Thread.CurrentThread.IsThreadPoolThread}");
                if (token.CanBeCanceled)
                {
                    Console.WriteLine("Task was canceled...");
                }
            }, token);

            tokenSource.Cancel();

            firstTask.ContinueWith(task =>
                {
                    Console.WriteLine(
                        $"Current Task {Task.CurrentId} | Parent Task is canceled : {task.IsCanceled} | Is the current taskfrom Thread pool : {Thread.CurrentThread.IsThreadPoolThread}");
                },
                TaskContinuationOptions.LongRunning);
        }

        #endregion
    }
}
