using Kanban.CrossCutting;
using Kanban.Integration.Tests.DatabaseMocks;
using Kanban.Repository.Dto.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Kanban.Integration.Tests.Helper;

public static class AuthenticationHelper
{
    public static void SetupAuthenticationHeader(HttpClient httpClient, string credentials)
    {
        string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(credentials));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Authentication, svcCredentials);
    }
}
