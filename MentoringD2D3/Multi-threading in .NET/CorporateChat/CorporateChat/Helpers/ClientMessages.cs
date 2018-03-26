using System;
using System.Collections.Concurrent;
using System.IO;
using CorporateChat.ClientMessages;
using Newtonsoft.Json;

namespace CorporateChat.Helpers
{
    /// <summary>
    /// Storage of clients' messages.
    /// </summary>
    public class ClientMessages
    {
        private const string GreetingMessagesFilePath = @"..\..\..\CorporateChat\ClientMessages\GreetingMessages.json";

        private static readonly Lazy<ConcurrentBag<ClientMessage>> Lazy =
            new Lazy<ConcurrentBag<ClientMessage>>(() =>
            {
                var messages = JsonConvert.DeserializeObject<ConcurrentBag<ClientMessage>>(File.ReadAllText(GreetingMessagesFilePath));
                return messages;
            });

        public static ConcurrentBag<ClientMessage> Messages => Lazy.Value;
    }
}
