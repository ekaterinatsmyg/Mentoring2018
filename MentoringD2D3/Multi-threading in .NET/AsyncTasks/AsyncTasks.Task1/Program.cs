using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTasks.Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter number for starting calculation. For exit tape E");

            var userInput = Console.ReadLine();
            CancellationTokenSource cts = new CancellationTokenSource();
            while (userInput != null && !userInput.ToUpper().StartsWith("E"))
            {
                GetSum(userInput, cts.Token);
                userInput = Console.ReadLine();
                cts.Cancel();
                cts.Dispose();
                cts = new CancellationTokenSource();
            }
        }

        /// <summary>
        /// Summarize sequence of integers from 0 to <c>userNumberText</c> asynchronously and printthe result.
        /// </summary>
        /// <param name="input">The top bound, wich a user input.</param>
        /// <param name="token">The object, which indicates whether cancellation is requested.</param>
        public static async void GetSum(string input, CancellationToken token)
        {
            var result = await StartCalculationAsync(input, token);
            Console.WriteLine(result);
        }

        /// <summary>
        /// Summarize sequence of integers from 0 to <c>userNumberText</c> asynchronously.
        /// </summary>
        /// <param name="userNumberText">The top bound, wich a user input.</param>
        /// <param name="token">The object, which indicates whether cancellation is requested.</param>
        /// <returns>The object that contains a result of the calculations.</returns>
        public static async Task<string> StartCalculationAsync(string userNumberText, CancellationToken token)
        {
            try
            {
                return await Task<string>.Factory.StartNew(() => GetCounter(userNumberText, token), token);
            }
            catch
            {
                return "Canceled";
            }
        }

        /// <summary>
        ///  Summarize sequence of integers from 0 to <c>maxNumber</c>.
        /// </summary>
        /// <param name="userNumberText">The top bound, wich a user input.</param>
        /// <param name="token">The object, which indicates whether cancellation is requested.</param>
        /// <returns>The message that shows a result of calculayion.</returns>
        public static string GetCounter(string userNumberText, CancellationToken token)
        {
            long.TryParse(userNumberText, out var userNumberlong);

            return string.Concat("userNumber=", userNumberlong, " sumUserNumber=", Count(userNumberlong, token));
        }


        /// <summary>
        /// Summarize sequence of integers from 0 to <c>maxNumber</c>.
        /// </summary>
        /// <param name="maxNumber">The top bound.</param>
        /// <param name="token">The object, which indicates whether cancellation is requested.</param>
        /// <returns>The result of summarizing</returns>
        public static long Count(long maxNumber, CancellationToken token)
        {
            long result = 0;

            for (int i = 0; i <= maxNumber; i++)
            {
                token.ThrowIfCancellationRequested();
                result += i;
            }
            return result;
        }
    }

}
