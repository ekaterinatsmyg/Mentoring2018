using System;
using System.Linq;

namespace CorporateChat.Helpers
{
    /// <summary>
    /// Help to create a new message for clients. 
    /// </summary>
    public class ClientMessagesHelper
    {
        /// <summary>
        /// Minimum value of a message's id.
        /// </summary>
        private const int MIN_MSG_NUMBER = 1;

        /// <summary>
        /// Maximum value of a message's id.
        /// </summary>
        private const int MAX__MSG_NUMBER = 7;

        /// <summary>
        /// Random value generator.
        /// </summary>
        private static readonly Random randomGenerator = new Random();

        /// <summary>
        /// Select a random message from existing list of the messages.
        /// </summary>
        /// <returns>A random message.</returns>
        public static string GetRandomMessage()
        {
            return ClientMessages.Messages.Where(message =>
                message.Id == randomGenerator.Next(MIN_MSG_NUMBER, MAX__MSG_NUMBER)).Select(message => message.Text).FirstOrDefault();
        }
    }
}
