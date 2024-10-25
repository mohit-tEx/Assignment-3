using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_3
{
    internal interface ISourceControlProvider
    {
        //Branch Management
        //Create Project
        public Task<int> CreateProject(string projectName, string? projectDescription = null);
        //Create Branch
        public Task CreateBranch(string branchName, string projectId);
        //View Branches
        public Task<List<BranchInfo>> GetBranches(string projectId, string? filterByTag = null, string? filterByMsg = null, string? filterByName = null );



        //Commit Management
        //View Commits
        public Task<List<CommitInfo>> GetCommits(string projectID, string? filterByTag = null, string? filterByMsg = null, string? filterByName = null);

        //Commit Status Sync
        public Task<string> SyncCommitStatus(string projectId, string branchName);



        //Merge Request Management
        //View Merge Requests
        public Task<List<MergeRequestInfo>> GetMergeRequests(string projectId, string? filterTag = null, string? filterByName = null, string? filterByStatus = null);
        //Merge Request Approval Process
        public Task<MergeRequestApprovalStatusInfo> GetMergeRequestApprovalStatus(string projectId, string mergeRequestId);


        //Notification and Alert Management
        //Task Commit alerts
        public Task<List<CommitInfo>> NotifyUserAboutNewCommits(string projectId, string lastDate);

        //Merge Request Approval Reminders
        public Task<IEnumerable<Reviewer>> NotifyReviewersForMRApproval(string projectId, string mergeRequestID);
    }
}
