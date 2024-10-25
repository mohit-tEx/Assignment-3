using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Assignment_3
{
    [JsonConverter(typeof(MergeRequestInfoConverter))]    
    internal class MergeRequestInfo
    {
        public int IId { get; set; }
        public DateTime CreationTime { get; set; }
        public User? Author { get; set; }
        public string? Web_URL { get; set; }
        public string? Message { get; set; }
        public string? State { get; set; }
        public IEnumerable<User>? Reviewers { get; set; }

        public override string ToString()
        {
            return $"When: {CreationTime}\n\tWho: {Author?.Name}\n\tLink: {Web_URL}\n\tSummary: {Message}\n\tStatus: {State}";
        }

    }

    //CODE FOR CUSTOM CONVERTER SO THAT WHILE DESERIALIZATION JSON FROM DIFFERENT SOURCES
    //SUCH AS GITHUB, GITLAB, BITBUCKET CAN BE CONVERTED TO OBJECT
    public class MergeRequestInfoConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(MergeRequestInfo);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var mergeRequestInfo = new MergeRequestInfo()
            {
                IId = Convert.ToInt32(jsonObject["iid"]),
                CreationTime = jsonObject["created_at"]?.ToObject<DateTime>() ?? default,
                Author = jsonObject["author"]?.ToObject<User>(),
                Web_URL = jsonObject["web_url"]?.ToString(),
                Message = jsonObject["description"]?.ToString(),
                State = jsonObject["state"]?.ToString(),
                Reviewers = jsonObject["reviewers"]?.ToObject<List<User>>() ?? default,
            };

            return mergeRequestInfo;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            //Used for serialization
        }
    }
}
