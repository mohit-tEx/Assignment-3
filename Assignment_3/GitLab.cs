using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Assignment_3
{
    internal class GitLab : ISourceControlProvider
    {
        public string BaseUrl { get; }
        public string API_Token { get; }

        public GitLab(string baseUrl, string apiToken)
        {
            BaseUrl = baseUrl;
            API_Token = apiToken;
        }
        public async Task<string> MakeGitLabAPIRequest(string API_URL, FormUrlEncodedContent? requestBody)
        {
            using var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", API_Token);

            HttpResponseMessage response;

            if (requestBody == null)
            {
                response = await client.GetAsync(API_URL);
            }
            else
            {
                response = await client.PostAsync(API_URL, requestBody);
            }

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }

        public async Task<int> CreateProject(string projectName, string? projectDescription = null)
        {
            var body = new FormUrlEncodedContent
            ([
                new KeyValuePair<string, string>("name", projectName),
                new KeyValuePair<string, string>("description", projectDescription ?? "")
            ]);

            string url = $"{BaseUrl}/projects";

            var response = await MakeGitLabAPIRequest(url, body);

            var project = JsonConvert.DeserializeObject<Project>(response);

            return project.Id;
        }

        public async Task CreateBranch(string branchName, string projectId)
        {
            var url = $"{BaseUrl}/projects/{projectId}/repository/branches";

            var body = new FormUrlEncodedContent
            ([
                new KeyValuePair<string, string>("branch", branchName),
                new KeyValuePair<string, string>("ref", "main"),
            ]);

            await MakeGitLabAPIRequest(url, body);

            Console.WriteLine("Branch Created");
        }

        public async Task<List<BranchInfo>> GetBranches(string projectId, string? filterTag = null, string? filterName = null, string? filterType = null)
        {
            string url = $"{BaseUrl}/projects/{projectId}/repository/branches";

            string responseBody = await MakeGitLabAPIRequest(url, null);
            
            var branches = JsonConvert.DeserializeObject<List<BranchInfo>>(responseBody);

            // Filter logic

            return branches.Where(b => (string.IsNullOrEmpty(filterTag) || b.Commit.Message.Contains(filterTag))

                                    && (string.IsNullOrEmpty(filterName) || b.Name.Contains(filterName))

                                    && (string.IsNullOrEmpty(filterType) || b.Name.Contains(filterType)))

                           .ToList();
        }

        public async Task<List<CommitInfo>> GetCommits(string projectID, string? filterByTag = null, string? filterByMsg = null, string? filterByName = null)
        {
            string url = $"{BaseUrl}/projects/{projectID}/repository/commits";

            string response = await MakeGitLabAPIRequest(url, null);

            var commits = JsonConvert.DeserializeObject<List<CommitInfo>>(response);

            // Filter logic

            return commits.Where(c => (string.IsNullOrEmpty(filterByTag) || c.Message.Contains(filterByTag))

                                   && (string.IsNullOrEmpty(filterByMsg) || c.Message.Contains(filterByMsg))

                                   && (string.IsNullOrEmpty(filterByName) || c.Committer_Name == filterByName))

                          .ToList();
        }

        public async Task<string> SyncCommitStatus(string projectId, string commitId)
        {
            string url = $"{BaseUrl}/projects/{projectId}/repository/commits/{commitId}";

            string response = await MakeGitLabAPIRequest(url, null);

            var commit = JsonConvert.DeserializeObject<CommitInfo>(response);

            return commit?.Status;
        }

        public async Task<List<MergeRequestInfo>> GetMergeRequests(string projectId, string? filterTag = null, string? filterByName = null, string? filterByStatus = null)
        {
            string url = $"{BaseUrl}/projects/{projectId}/merge_requests";

            string response = await MakeGitLabAPIRequest(url , null);
            var mergeRequests = JsonConvert.DeserializeObject<List<MergeRequestInfo>>(response);

            return mergeRequests.Where(mr => (string.IsNullOrEmpty(filterTag) || mr.Message.Contains(filterTag))

                                      && (string.IsNullOrEmpty(filterByStatus) || mr.State == filterByStatus))

                            .ToList();
        }

        public async Task<MergeRequestApprovalStatusInfo> GetMergeRequestApprovalStatus(string projectId, string mergeRequestId)
        {
            string url = $"{BaseUrl}/projects/{projectId}/merge_requests/{mergeRequestId}/approvals";

            string response = await MakeGitLabAPIRequest(url , null);

            MergeRequestApprovalStatusInfo? mergeRequestStatus = JsonConvert.DeserializeObject<MergeRequestApprovalStatusInfo>(response);

            return mergeRequestStatus;
        }

        public async Task<List<CommitInfo>> NotifyUserAboutNewCommits(string projectId, string lastDate)
        {
            string url = $"{BaseUrl}/projects/{projectId}/repository/commits/?since={lastDate}";

            var response = await MakeGitLabAPIRequest(url , null);

            var commits = JsonConvert.DeserializeObject<List<CommitInfo>>(response);

            return commits;
        }

        public async Task<IEnumerable<Reviewer>> NotifyReviewersForMRApproval(string projectId, string mergeRequestID)
        {
            string url = $"{BaseUrl}/projects/{projectId}/merge_requests/{mergeRequestID}/reviewers";

            string response = await MakeGitLabAPIRequest(url, null);
            
            var reviewers = JsonConvert.DeserializeObject<List<Reviewer>>(response);

            return reviewers.Where(r => string.IsNullOrEmpty(r.State) || r.State != "approved");
        }
    }

}
