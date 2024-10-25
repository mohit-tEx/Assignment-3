using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_3
{
    [JsonConverter(typeof(BranchInfoConverter))]
    internal class BranchInfo
    {
        public string? Name { get; set; }
        public CommitInfo? Commit { get; set; }

        public override string ToString()
        {
            return $"Branch Name: {Name}\n\tCreator: {Commit?.Committer_Name}\n\tCreation Date: {Commit?.Committed_Date}\n\tTag: {Commit?.Status}";
        }
    }

    //CODE FOR CUSTOM CONVERTER SO THAT WHILE DESERIALIZATION JSON FROM DIFFERENT SOURCES
    //SUCH AS GITHUB, GITLAB, BITBUCKET CAN BE CONVERTED TO OBJECT
    public class BranchInfoConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BranchInfo);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var branchInfo = new BranchInfo()
            {
                Name = jsonObject["name"]?.ToString(),
                Commit = jsonObject["commit"]?.ToObject<CommitInfo>()

            };

            return branchInfo;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            //Used for serialization
        }
    }

}
