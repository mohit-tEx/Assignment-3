using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System;
using System.Configuration;
using Assignment_3;
using Microsoft.Extensions.Configuration;

namespace Assignment_3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            //string projectId = "62792581";
            //string mergeRequestId = "2";
            //string commitId = "1272ef5c7985130218189b5829faf637d75ba79f";

            string sourceControlPlatform = "gitlab";

            string GitLabBaseURL = config[$"SourceControl:{sourceControlPlatform}:BaseUrl"] ?? "";
            string GitLabAPIToken = config[$"SourceControl:{sourceControlPlatform}:API_Token"] ?? "";
            
            GitLab gitLab = new(GitLabBaseURL, GitLabAPIToken);
        }
    }

}