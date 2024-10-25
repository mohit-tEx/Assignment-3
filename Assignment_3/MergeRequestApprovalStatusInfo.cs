using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_3
{
    [JsonConverter(typeof(MergeRequestApprovalStatusInfoConverter))]
    internal class MergeRequestApprovalStatusInfo
    {
        public int Approvals_Required { get; set; }
        public int Approvals_Left { get; set; }
        public string? State { get; set; }

        public override string ToString()
        {
            return $"Approval Status: {State}\nNumber of approvals requires: {Approvals_Required}\nNumber of Approvals Left: {Approvals_Left}";
        }
    }

    //CODE FOR CUSTOM CONVERTER SO THAT WHILE DESERIALIZATION JSON FROM DIFFERENT SOURCES
    //SUCH AS GITHUB, GITLAB, BITBUCKET CAN BE CONVERTED TO OBJECT
    public class MergeRequestApprovalStatusInfoConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(MergeRequestApprovalStatusInfo);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var mergeRequestStatusInfo = new MergeRequestApprovalStatusInfo()
            {
                Approvals_Required = Convert.ToInt32(jsonObject["approvals_required"]),
                Approvals_Left = Convert.ToInt32(jsonObject["approvals_left"]),
                State = jsonObject["state"]?.ToString()
            };

            return mergeRequestStatusInfo;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            //Used for serialization
        }
    }
}
