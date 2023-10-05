using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace POL1.Pages
{
    public class Index8Model : PageModel
    {

        [BindProperty]
        public string Date { get; set; } = "";

        [BindProperty]
        public string EmployeeID { get; set; } = "";

        [BindProperty]
        public string EmployeeName { get; set; } = "";

        [BindProperty]
        public string FatherName { get; set; } = "";

        [BindProperty]
        public string JoiningDate { get; set; } = "";

        [BindProperty]
        public string DateOfLeaving { get; set; } = "";

        [BindProperty]
        public string CNICNumber { get; set; } = "";

        [BindProperty]
        public string EOBINumber { get; set; } = "";

        [BindProperty]
        public string Department { get; set; } = "";

        [BindProperty]
        public string SocialSecurityNumber { get; set; } = "";
        
        [BindProperty]
        public string Address { get; set; } = "";

        [BindProperty]
        public string Salary { get; set; } = "";

        [BindProperty]
        public string ContractorName { get; set; } = "";

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            string tableName = "ContractorStaff"; 
            Dictionary<string, object> data = new Dictionary<string, object>

            {
                { "Date", Date },
                { "EmployeeID", EmployeeID },
                { "EmployeeName", EmployeeName },
                { "FatherName", FatherName },
                { "JoiningDate", JoiningDate },
                { "DateOfLeaving", DateOfLeaving },
                { "CNICNumber", CNICNumber },
                { "EOBINumber", EOBINumber },
                { "Department", Department },
                { "SocialSecurityNumber", SocialSecurityNumber },
                { "Address", Address },
                { "Salary", Salary },
                { "ContractorName", ContractorName },
            };

            InsertData(tableName, data);

            return RedirectToPage("/Index8");
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
