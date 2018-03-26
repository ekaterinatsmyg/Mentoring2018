using Newtonsoft.Json;

namespace CorporateChat.ClientMessages
{
    [JsonObject]
    public class ClientMessage
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Text { get; set; }
    }
}
