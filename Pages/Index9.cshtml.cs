using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace POL1.Pages
{
    public class Index9Model : PageModel
    {
        [BindProperty]
        public string Date { get; set; } = "";
        [BindProperty]
        public string EmployeeID { get; set; } = "";
        [BindProperty]
        public string EmployeeName { get; set; } = "";
        [BindProperty]
        public string Designation { get; set; } = "";
        [BindProperty]
        public string Department { get; set; } = "";
        [BindProperty]
        public string Status { get; set; } = "";
        [BindProperty]
        public string TimeIn { get; set; } = "";
        [BindProperty]
        public string TimeOut { get; set; } = "";
        
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            //DateTime myDate = DateTime.Now;


            string tableName = "CSAttendance"; // Change this based on your needs
            Dictionary<string, object> data = new Dictionary<string, object>

            {
                { "Date", Date },
                { "EmployeeID", EmployeeID },
                { "EmployeeName", EmployeeName },
                { "Designation", Designation },
                { "Department", Department },
                { "Status", Status },
                { "TimeIn", TimeIn },
                { "TimeOut", TimeOut },
                };

            InsertData(tableName, data);


            return RedirectToPage("/Index9");
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
