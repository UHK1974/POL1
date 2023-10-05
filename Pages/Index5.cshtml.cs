using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Authentication.ExtendedProtection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace POL1.Pages
{
    public class Index5Model : PageModel
    {
        [BindProperty]
        public string Date { get; set; } = "";
        [BindProperty]
        public string DocumentNumber { get; set; } = "";
        [BindProperty]
        public string ServiceCompany { get; set; } = "";
        [BindProperty]
        public string WellLocation { get; set; } = "";
        [BindProperty]
        public string JobID { get; set; } = "";
        [BindProperty]
        public string Remarks4 { get; set; } = "";
        [BindProperty]
        public string Person { get; set; } = "";
        [BindProperty]
        public string Designation { get; set; } = "";
        [BindProperty]
        public string DateIn { get; set; } = "";
        [BindProperty]
        public string TimeIn { get; set; } = "";
        [BindProperty]
        public string DateOut { get; set; } = "";
        [BindProperty]
        public string TimeOut { get; set; } = "";
        [BindProperty]
        public string Remarks1 { get; set; } = "";
        [BindProperty]
        public string Status { get; set; } = "";
        [BindProperty]
        public string EquipmentName { get; set; } = "";
        [BindProperty]
        public string Unit { get; set; } = "";
        [BindProperty]
        public string Quantity { get; set; } = "";
        [BindProperty]
        public string DateIn1 { get; set; } = "";
        [BindProperty]
        public string TimeIn1 { get; set; } = "";
        [BindProperty]
        public string DateOut1 { get; set; } = "";
        [BindProperty]
        public string TimeOut1 { get; set; } = "";
        [BindProperty]
        public string Remarks { get; set; } = "";
        [BindProperty]
        public string ProjectID { get; set; } = "";

        public List<ReturnableItem> ReturnableItems { get; set; } = new List<ReturnableItem>();

        public void OnGet()
        {
            DocumentNumber = "POL-" + DateTime.Now.ToString("ddMMyy-HHmm");
        }

        public IActionResult OnPost()
        {
            List<KeyValuePair<string, string>> formDataList = new();

            if (HttpContext.Request.Form.Count > 0)
            {
                foreach (var key in Request.Form.Keys)
                {
                    var value = Request.Form[key];
                    formDataList.Add(new KeyValuePair<string, string>(key, value));
                }
            }
            string tableName = "ThirdPartyDetails"; // Change this based on your needs
            Dictionary<string, object> data = new Dictionary<string, object>

            {
                { "Date", Date },
                { "DocumentNumber", DocumentNumber },
                { "ProjectID", ProjectID },
                { "ServiceCompany", ServiceCompany },
                { "WellLocation", WellLocation },
                { "JobID", JobID },
                { "Remarks", Remarks4 },
                { "Status", Status},
                
            };

            InsertData(tableName, data);

            tableName = "Tabulation1"; 

            data = new Dictionary<string, object>
            {
                { "Person", Person },
                { "Designation", Designation },
                { "DateIn", DateIn },
                { "TimeIn", TimeIn },
                { "DateOut", DateOut },
                { "TimeOut", TimeOut },
                { "Remarks", Remarks1 },
                { "DocumentNumber", DocumentNumber },
            };
 
            InsertData(tableName, data);

            tableName = "Tabulation2";

            data = new Dictionary<string, object>
            {
                { "EquipmentName", EquipmentName },
                { "Unit", Unit },
                { "Quantity", Quantity },
                { "DateIn", DateIn1 },
                { "TimeIn", TimeIn1 },
                { "DateOut", DateOut1 },
                { "TimeOut", TimeOut1 },
                { "Remarks", Remarks },
                { "DocumentNumber", DocumentNumber },
            };

            InsertData(tableName, data);

            return RedirectToPage("/Index5");
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
