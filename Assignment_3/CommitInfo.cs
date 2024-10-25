using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_3
{
    [JsonConverter(typeof(CommitInfoConverter))]
    internal class CommitInfo
    {
        public string? Id { get; set; }
        public DateTime Committed_Date { get; set; }
        public string? Committer_Name { get; set; }
        public string? Web_URL { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }

        public override string ToString()
        {
            return $"Who: {Committer_Name}\n\tWhen: {Committed_Date}\n\tLink: {Web_URL}\n\tSummary: {Message}\n\tStatus: {Status}";
        }
    }

    //CODE FOR CUSTOM CONVERTER SO THAT WHILE DESERIALIZATION JSON FROM DIFFERENT SOURCES
    //SUCH AS GITHUB, GITLAB, BITBUCKET CAN BE CONVERTED TO OBJECT
    public class CommitInfoConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(CommitInfo);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var commitInfo = new CommitInfo()
            {
                Id = jsonObject["id"]?.ToString(),
                Committed_Date = jsonObject["committed_date"]?.ToObject<DateTime>() ?? default,
                Committer_Name = jsonObject["author_name"]?.ToString(),
                Web_URL = jsonObject["web_url"]?.ToString(),
                Status = jsonObject["state"]?.ToString() ?? "Status Not Found",
                Message = jsonObject["message"]?.ToString()
            };

            return commitInfo;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            //Used for serialization
        }
    }
}
