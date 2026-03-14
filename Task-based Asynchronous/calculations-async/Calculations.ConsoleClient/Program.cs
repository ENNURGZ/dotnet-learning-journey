using System;
using System.Threading.Tasks;

namespace Calculations.ConsoleClient
{
    internal static class Program
    {
        /// <summary>
        /// Calculates the sum from 1 to n synchronously.
        /// The value of n is set by the user from the console.
        /// The user can change the boundary n during the calculation, which causes the calculation to be restarted,
        /// this should not crash the application.
        /// After receiving the result, be able to continue calculations without leaving the console.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task Main()
        {
            Console.WriteLine("Welcome to Calculations Async Application.");
            Console.WriteLine("Enter a number 'n' to calculate the sum from 1 to n asymptotically.");
            Console.WriteLine("You can enter another number at any time to cancel and restart the calculation.");
            Console.WriteLine("Type 'exit' or 'quit' to terminate the application.");

            CancellationTokenSource? cts = null;
            Task<long>? calculationTask = null;

            while (true)
            {
                Console.Write("\nEnter n: ");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                if (input.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase) || 
                    input.Trim().Equals("quit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                if (!int.TryParse(input, out int n) || n <= 0)
                {
                    Console.WriteLine("Please enter a valid positive integer greater than 0.");
                    continue;
                }

                if (cts != null && !cts.IsCancellationRequested)
                {
                    Console.WriteLine("\n[Canceling previous calculation...]");
                    await cts.CancelAsync();
                }
                
                cts?.Dispose();
                cts = new CancellationTokenSource();
                var token = cts.Token;

                try
                {
                    Console.WriteLine($"[Starting calculation for n={n}...] ");
                    
                    calculationTask = Calculations.Calculator.CalculateSumAsync(n, token);

                    long result = await calculationTask;
                    Console.WriteLine($"[Result for n={n}] Sum = {result}");
                }
                catch (OperationCanceledException)
                {
                    // Catch cancellation and don't rethrow. User simply started another task.
                }
            }
            
            cts?.Dispose();
        }
    }
}
