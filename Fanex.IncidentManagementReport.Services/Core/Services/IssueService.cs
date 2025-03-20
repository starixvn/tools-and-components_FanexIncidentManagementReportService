namespace Fanex.IncidentManagementReport.Services.Core.Services;

using Enums;
using Fanex.IncidentManagementReport.Services.Models.Problem;
using Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using IncidentManagementReport.Services.Models;

public class IssueService : BaseHttpClientService, IIssueService
{
    private readonly AppSettings appSettings;

    public IssueService(RedmineApiSettings settings, IHttpClientFactory httpClientFactory, AppSettings appSettings) : base(settings, httpClientFactory)
    {
        this.appSettings = appSettings;
    }

    public async Task<IList<ProblemIssue>> GetAllIssuesAsync(TrackerEnum tracker)
    {
        var url = $"issues.json?status_id=*&limit=100&tracker_id={(int)tracker}&include=relations";
        var issueResponse = await ExecuteAsync<ProblemIssueReponse>(url);
        var issues = issueResponse.Issues;
        foreach (var issue in issues)
        {
            CalculateIssueData(issue);
        }
        return issues;
    }

    public async Task<IList<IssuesViewModel>> GetClosedIssuesAsync(DateTime fromDate, DateTime toDate, TrackerEnum tracker)
    {
        var url = $"issues.json?start_date<={toDate:yyyy-MM-dd}&due_date=><{fromDate:yyyy-MM-dd}|{toDate:yyyy-MM-dd}&status_id=5&limit=100&tracker_id={(int)tracker}";
        var issueResponse = await ExecuteAsync<IssuesModel>(url);
        return issueResponse.Issues;
    }

    public async Task<IList<IssuesViewModel>> GetCreateNewIssuesAsync(DateTime fromDate, DateTime toDate, TrackerEnum tracker)
    {
        var url = $"issues.json?start_date=><{fromDate:yyyy-MM-dd}|{toDate:yyyy-MM-dd}&status_id=*&limit=100&tracker_id={(int)tracker}";
        var issueResponse = await ExecuteAsync<IssuesModel>(url);
        return issueResponse.Issues;
    }

    public async Task<ProblemIssue> GetIssueByIdAsync(string issueId)
    {
        var url = $"issues/{issueId}.json";
        var result = await ExecuteAsync<SingleIssueResult>(url);
        var issue = result.Issue;
        CalculateIssueData(issue);

        return issue;
    }

    public async Task<IList<IssuesViewModel>> GetP1IssuesAsync(DateTime fromDate, DateTime toDate, TrackerEnum tracker)
    {
        var url = $"issues.json?start_date=><{fromDate:yyyy-MM-dd}|{toDate:yyyy-MM-dd}&status_id=*&priority_id=1&limit=100&tracker_id={(int)tracker}";
        var issueResponse = await ExecuteAsync<IssuesModel>(url);
        return issueResponse.Issues;
    }

    public async Task<IList<IssuesViewModel>> GetP1IssuesDailyAsync(DateTime fromDate, DateTime toDate, TrackerEnum tracker)
    {
        var url = $"issues.json?created_on=><{fromDate:yyyy-MM-dd}|{toDate:yyyy-MM-dd}&status_id=*&priority_id=1&limit=100&tracker_id={(int)tracker}";
        var issueResponse = await ExecuteAsync<IssuesModel>(url);
        return issueResponse.Issues;
    }

    public async Task<IList<IssuesViewModel>> GetPendingIssuesAsync(DateTime startDate, TrackerEnum tracker)
    {
        var url = $"issues.json?start_date<={startDate:yyyy-MM-dd}status_id=!5|6&limit=100&tracker_id={(int)tracker}";
        var issueResponse = await ExecuteAsync<IssuesModel>(url);
        return issueResponse.Issues;
    }

    public async Task<IList<IssuesViewModel>> GetPendingIssuesDailyAsync(TrackerEnum tracker)
    {
        var url = $"issues.json?status_id=!5|6&priority_id=!1&limit=100&tracker_id={(int)tracker}";
        var issueResponse = await ExecuteAsync<IssuesModel>(url);
        return issueResponse.Issues;
    }

    private void CalculateIssueData(ProblemIssue issue)
    {
        issue.ReportedDateTime = CalculateFromCustomFields(issue.CustomFields, appSettings.ReportedDateCustomFieldId);
        issue.ResolvedDate = CalculateFromCustomFields(issue.CustomFields, appSettings.ResolvedDateCustomFieldId);
        issue.RequestForChange = CalculateRequestForChange(issue.CustomFields, appSettings.RequestForChangeCustomFieldId);
        issue.KnowError = CalculateKnowError(issue.CustomFields, appSettings.KnowErrorCustomFieldId);
    }

    private static string CalculateKnowError(IList<CustomField> customFields, int customFieldsId)
    {
        var data = customFields.FirstOrDefault(x => x.Id == customFieldsId)?.Value.ToString();
        if (!string.IsNullOrEmpty(data))
        {
            return data == "1" ? "Yes" : "No";
        }

        return null;
    }

    private static string CalculateRequestForChange(IList<CustomField> customFields, int customFieldsId)
        => customFields.FirstOrDefault(x => x.Id == customFieldsId)?.Value.ToString();

    private static DateTime? CalculateFromCustomFields(IList<CustomField> customFields, int customFieldsId)
    {
        var data = customFields.FirstOrDefault(x => x.Id == customFieldsId)?.Value.ToString();

        if (!string.IsNullOrEmpty(data))
        {
            return DateTime.ParseExact(data, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        return null;
    }
}