using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace POL1.Pages
{
    public class Index4Model : PageModel
    {

        [BindProperty]
        public string Date { get; set; } = "";

        [BindProperty]
        public string ProjectID { get; set; } = "";

        [BindProperty]
        public string ProjectName { get; set; } = "";

        [BindProperty]
        public string StartDate { get; set; } = "";

        [BindProperty]
        public string EndDate { get; set; } = "";

        [BindProperty]
        public string ServiceCompany { get; set; } = "";

        [BindProperty]
        public string TeamLeadName { get; set; } = "";

        [BindProperty]
        public string TotalTeamMember { get; set; } = "";

        [BindProperty]
        public string Status { get; set; } = "";

        [BindProperty]
        public string ProjectScope { get; set; } = "";

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            string tableName = "Project"; 
            Dictionary<string, object> data = new Dictionary<string, object>

            {
                { "Date", Date },
                { "ProjectID", ProjectID },
                { "ProjectName", ProjectName },
                { "StartDate", StartDate },
                { "EndDate", EndDate },
                { "ServiceCompany", ServiceCompany },
                { "TeamLeadName", TeamLeadName },
                { "TotalTeamMember", TotalTeamMember },
                { "Status", Status },
                { "ProjectScope", ProjectScope },

            };

            InsertData(tableName, data);

            return RedirectToPage("/Index4");
        }

        private static void InsertData(string tableName, Dictionary<string, object> data)
        {
            try
            {
                ConnectionClass cc = new();
                string connectionString = cc.connection;

                using SqlConnection connection = new(connectionString);
                connection.Open();

                string columns = string.Join(", ", data.Keys);
                string values = string.Join(", ", data.Keys.Select(key => $"@{key}"));

                string query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

                using SqlCommand command = new(query, connection);
                foreach (var entry in data)
                {
                    if (entry.Value == null)
                    {
                        // Handle the null value here, such as setting it to DBNull.Value
                        command.Parameters.AddWithValue($"@{entry.Key}", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.Add($"@{entry.Key}", GetSqlDbType(entry.Value)).Value = entry.Value;
                    }
                }

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
        }

        private static SqlDbType GetSqlDbType(object value)
        {
            if (value is int)
            {
                return SqlDbType.Int;
            }
            else if (value is string)
            {
                return SqlDbType.NVarChar;
            }
            else if (value is DateTime)
            {
                return SqlDbType.DateTime;
            }

            else
            {
                return SqlDbType.NVarChar;
            }
        }
    }
}
