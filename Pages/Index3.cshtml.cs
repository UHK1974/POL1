using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace POL1.Pages
{
    public class Index3Model : PageModel
    {

        [BindProperty]
        public string SupplierName { get; set; } = "";

        [BindProperty]
        public string SupplierCode { get; set; } = "";

        [BindProperty]
        public string ContactPerson { get; set; } = "";

        [BindProperty]
        public string ContactNumber { get; set; } = "";

        [BindProperty]
        public string Address { get; set; } = "";

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            string tableName = "AddSupplier"; // Change this based on your needs
            Dictionary<string, object> data = new Dictionary<string, object>

            {
                { "SupplierName", SupplierName },
                { "SupplierCode", SupplierCode },
                { "ContactPerson", ContactPerson },
                { "ContactNumber", ContactNumber },
                { "Address", Address },
            };

            InsertData(tableName, data);

            return RedirectToPage("/Index3");
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
