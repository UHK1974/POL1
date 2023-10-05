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
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace POL1.Pages
{
    public class Index7Model : PageModel
    {
        [BindProperty]
        public string Date { get; set; } = "";
        [BindProperty]
        public string ReceiptNumber { get; set; } = "";
        [BindProperty]
        public string TransactionType { get; set; } = "";
        [BindProperty]
        public string DriverName1 { get; set; } = "";
        [BindProperty]
        public string VehicleNumber1 { get; set; } = "";
        [BindProperty]
        public string ContactNumber1 { get; set; } = "";
        [BindProperty]
        public string DriverName2 { get; set; } = "";
        [BindProperty]
        public string VehicleNumber2 { get; set; } = "";
        [BindProperty]
        public string ContactNumber2 { get; set; } = "";
        [BindProperty]
        public string Purpose1 { get; set; } = "";
        [BindProperty]
        public string Source1 { get; set; } = "";
        [BindProperty]
        public string Remarks1 { get; set; } = "";
        [BindProperty]
        public string Purpose2 { get; set; } = "";
        [BindProperty]
        public string Source2 { get; set; } = "";
        [BindProperty]
        public string Remarks2 { get; set; } = "";
        [BindProperty]
        public string Purpose3 { get; set; } = "";
        [BindProperty]
        public string Source3 { get; set; } = "";
        [BindProperty]
        public string Remarks3 { get; set; } = "";
        [BindProperty]
        public string Purpose4 { get; set; } = "";
        [BindProperty]
        public string Source4 { get; set; } = "";
        [BindProperty]
        public string Remarks4 { get; set; } = "";
        [BindProperty]
        public string RequestedBy1 { get; set; } = "";
        [BindProperty]
        public string Department1 { get; set; } = "";
        [BindProperty]
        public string Reason1 { get; set; } = "";
        [BindProperty]
        public string RequestedBy2 { get; set; } = "";
        [BindProperty]
        public string Department2 { get; set; } = "";
        [BindProperty]
        public string Reason2 { get; set; } = "";
        [BindProperty]
        public string ItemName { get; set; } = "";
        [BindProperty]
        public string ItemCode { get; set; } = "";
        [BindProperty]
        public string Description { get; set; } = "";
        [BindProperty]
        public string UOM { get; set; } = "";
        [BindProperty]
        public string Quantity { get; set; } = "";
        [BindProperty]
        public string UnitPrice { get; set; } = "";
        [BindProperty]
        public int TotalAmount { get; set; }
        [BindProperty]
        public string Currency { get; set; } = "";
        [BindProperty]
        public string Status { get; set; } = "";
        [BindProperty]
        public String POWorkOderNumber1 { get; set; } = "";
        [BindProperty]
        public string DCInvoicNumber1 { get; set; } = "";
        [BindProperty]
        public string ChargeTo1 { get; set; } = "";
        public List<ItemModel> Items { get; set; }
        public IEnumerable<InventoryItems> InvItems { get; set; }

        [BindProperty]

        public List<InventoryItems> InventoryItem { get; set; }

        public List<ReturnableItem> ReturnableItems { get; set; } = new List<ReturnableItem>();

        public static string receiptNumber = "NA";
        public void OnGet()
        {
            
        }
        List<Dictionary<string, object>> mainList;

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
            DateTime myDate = DateTime.Now;


            string tableName = "FormEntries"; // Change this based on your needs
            Dictionary<string, object> data = null;
            if (TransactionType == "Outward")
            {
                data = new Dictionary<string, object>
            {
                { "Date", Date },
                { "ReceiptNumber", ReceiptNumber },
                { "TransactionType", TransactionType },
                { "DriverName", DriverName1 },
                { "VehicleNumber", VehicleNumber1 },
                { "ContactNumber", ContactNumber1 },
                { "RequestedBy", null},
                { "Department", null},
                { "Reason", null },
                { "Purpose", Purpose1 },
                { "Source", Source1 },
                { "Remarks", Remarks1 },
                { "Status", Status },
                { "POWorkOderNumber", null },
                { "DCInvoicNumber", null },
                { "ChargeTo", null },
                // Add other columns and values as needed
            };
            }
            else if (TransactionType == "Inward")
            {
                data = new Dictionary<string, object>
             {
                 { "Date",  Date},
                 { "ReceiptNumber", ReceiptNumber },
                 { "TransactionType", TransactionType },
                 { "DriverName", DriverName2 },
                 { "VehicleNumber", VehicleNumber2 },
                 { "ContactNumber", ContactNumber2 },
                 { "RequestedBy", null},
                 { "Department", null},
                 { "Reason", null },
                 { "Purpose", Purpose2 },
                 { "Source", Source2 },
                 { "Remarks", Remarks2 },
                 { "Status", Status },
                 { "POWorkOderNumber", POWorkOderNumber1 },
                 { "DCInvoicNumber", DCInvoicNumber1 },
                 { "ChargeTo", null },
                 // Add other columns and values as needed
             };
            }
            else if (TransactionType == "Issuance")
            {
                data = new Dictionary<string, object>
             {
                 { "Date",  Date },
                 { "ReceiptNumber", ReceiptNumber },
                 { "TransactionType", TransactionType },
                 { "DriverName", null},
                 { "VehicleNumber", null },
                 { "ContactNumber", null},
                 { "RequestedBy", RequestedBy1},
                 { "Department", Department1},
                 { "Reason", Reason1 },
                 { "Purpose", Purpose3 },
                 { "Source", Source3 },
                 { "Remarks", Remarks3 },
                 { "Status", Status },
                 { "POWorkOderNumber", null },
                 { "DCInvoicNumber", null },
                 { "ChargeTo", ChargeTo1 },
                 // Add other columns and values as needed
             };
            }
            else if (TransactionType == "Receive")
            {
                data = new Dictionary<string, object>
             {
                 { "Date", Date },
                 { "ReceiptNumber", ReceiptNumber },
                 { "TransactionType", TransactionType },
                 { "DriverName", null},
                 { "VehicleNumber", null },
                 { "ContactNumber", null},
                 { "RequestedBy", RequestedBy2},
                 { "Department", Department2},
                 { "Reason", Reason2 },
                 { "Purpose", Purpose4 },
                 { "Source", Source4 },
                 { "Remarks", Remarks4 },
                 { "Status", Status },
                 { "POWorkOderNumber", null },
                 { "DCInvoicNumber", null },
                 { "ChargeTo", null },
                 // Add other columns and values as needed
             };
            }
            UpdateData(tableName, data, ReceiptNumber);

            tableName = "FormInventory"; // Change to the target table


            mainList = new List<Dictionary<string, object>>();

            // mainList.add(data);
            List<mitems> mitems = new List<mitems>();

            List<string> ItemNamel = new List<string>();
            List<string> ItemCodel = new List<string>();
            List<string> Descriptionl = new List<string>();
            List<string> UOMl = new List<string>();
            List<string> Quantityl = new List<string>();
            List<string> UnitPricel = new List<string>();
            List<string> TotalAmountl = new List<string>();
            List<string> Currencyl = new List<string>();
            List<string> Statusl = new List<string>();
            List<string> ReceiptNumberl = new List<string>();


            foreach (var dataX in formDataList)
            {
                if (dataX.Key == "ItemName")
                {
                    string[] par = dataX.Value.ToString().Split(',');
                    for (int i = 0; i < par.Count(); i++)
                    {
                        ItemNamel.Add(par[i]);
                    }
                }
                else if (dataX.Key == "ItemCode")
                {
                    string[] parse = dataX.Value.ToString().Split(',');
                    for (int i = 0; i < parse.Count(); i++)
                    {
                        ItemCodel.Add(parse[i]);
                    }
                }
                else if (dataX.Key == "Description")
                {
                    string[] parse = dataX.Value.ToString().Split(',');
                    for (int i = 0; i < parse.Count(); i++)
                    {
                        Descriptionl.Add(parse[i]);
                    }
                }
                else if (dataX.Key == "UOM")
                {
                    string[] parse = dataX.Value.ToString().Split(',');
                    for (int i = 0; i < parse.Count(); i++)
                    {
                        UOMl.Add(parse[i]);
                    }
                }
                else if (dataX.Key == "Quantity")
                {
                    string[] parse = dataX.Value.ToString().Split(',');
                    for (int i = 0; i < parse.Count(); i++)
                    {
                        Quantityl.Add(parse[i]);
                    }
                }
                else if (dataX.Key == "UnitPrice")
                {
                    string[] parse = dataX.Value.ToString().Split(',');
                    for (int i = 0; i < parse.Count(); i++)
                    {
                        UnitPricel.Add(parse[i]);
                    }
                }
                else if (dataX.Key == "TotalAmount")
                {
                    string[] parse = dataX.Value.ToString().Split(',');
                    for (int i = 0; i < parse.Count(); i++)
                    {
                        TotalAmountl.Add(parse[i]);
                    }
                }
                else if (dataX.Key == "Currency")
                {
                    string[] parse = dataX.Value.ToString().Split(',');
                    for (int i = 0; i < parse.Count(); i++)
                    {
                        Currencyl.Add(parse[i]);
                    }
                }
                else if (dataX.Key == "Status")
                {
                    string[] parse = dataX.Value.ToString().Split(',');
                    for (int i = 0; i < parse.Count(); i++)
                    {
                        Statusl.Add(parse[i]);
                    }
                }
                else if (dataX.Key == "ReceiptNumber")
                {
                    string[] parse = dataX.Value.ToString().Split(',');
                    for (int i = 0; i < parse.Count(); i++)
                    {
                        ReceiptNumberl.Add(parse[i]);
                    }
                }

            }
            int x = 0;
            string dateString = myDate.ToString();
            foreach (string dat in ItemNamel)
            {
                Dictionary<string, object> item = new Dictionary<string, object>()
               {
                    { "ItemName", ItemNamel[x] },
                    { "ItemCode", ItemCodel[x] },
                    { "Description", Descriptionl[x] },
                    { "UOM", UOMl[x] },
                    { "Quantity", Quantityl[x] },
                    { "UnitPrice", UnitPricel[x] },
                    { "TotalAmount", TotalAmountl[x] },
                    { "Currency", Currencyl[x] },
                    { "UpdatedStatus", Statusl[x] },
                    { "ReceiptNumber", ReceiptNumber },
                    {"LastUpDate",dateString }
                };

                mainList.Add(item);
                string sih = getMainQuantity(ItemNamel[x]);
                int stock = int.Parse(sih);
                if (TransactionType == "Inward" || TransactionType == "InternelReceive")
                {
                    int quantity = int.Parse(Quantityl[x]);
                    int finalQUantity = stock + quantity;
                    updateMainQuantity(ItemNamel[x], finalQUantity.ToString());
                }
                else
                {
                    int quantity = int.Parse(Quantityl[x]);
                    int finalQUantity = stock - quantity;
                    updateMainQuantity(ItemNamel[x], finalQUantity.ToString());
                }
                x++;

            }
            int z = 0;
            foreach (Dictionary<string, object> y in mainList)
            {
                UpdateIData("FormInventory", y, ReceiptNumber, ItemCodel[z]);
                z++;
            }
            // data = new Dictionary<string, object>();

            //{
            //    { "ItemName", ItemName },
            //    { "ItemCode", ItemCode },
            //    { "Description", Description },
            //    //{ "Category", Category },
            //    { "UOM", UOM },
            //    { "Quantity", Quantity },
            //    { "UnitPrice", UnitPrice },
            //    { "TotalAmount", TotalAmount },
            //    { "Currency", Currency },
            //    { "Status", Status },
            //    { "ReceiptNumber", receiptNumber },
            //};



            // InsertData(tableName, data);

            return RedirectToPage("/Index7");
        }

       
        public IActionResult OnGetGetReceiptNumbers(DateTime? date, string transactionType)
        {
            if (!date.HasValue || string.IsNullOrEmpty(transactionType))
            {
                // Return an empty result if date is null or transactionType is empty
                return new EmptyResult();
            }

            List<string> receiptNumbers = new List<string>();

            // Connection string for your database
            ConnectionClass cc = new();
            string connectionString = cc.connection; ;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // SQL query
                string query = "SELECT ReceiptNumber FROM [dbo].[FormEntries] WHERE DATEPART(DAY, Date) like DATEPART(DAY, @Date) " +
                       "AND DATEPART(MONTH, Date) like DATEPART(MONTH, @Date) " +
                       "AND DATEPART(YEAR, Date) like DATEPART(YEAR, @Date) " +
                       "AND TransactionType like @TransactionType";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters
                    command.Parameters.AddWithValue("@Date", date.Value);
                    command.Parameters.AddWithValue("@TransactionType", transactionType);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Read and collect the ReceiptNumber values
                            receiptNumbers.Add(reader["ReceiptNumber"].ToString());
                        }
                    }
                }
            }

            // Return the receiptNumbers as a JSON response
            return new JsonResult(receiptNumbers);
        }
        public IActionResult OnGetGetTableData(string transactionType, string receiptNumber)
        {
            try
            {
                List<ItemModel> Items = new List<ItemModel>(); // Declare the list outside the if block

                ConnectionClass cc = new();
                string connectionString = cc.connection; 
                if (transactionType == "Outward")
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "Select DriverName, VehicleNumber, ContactNumber, Purpose, Source, Remarks From [dbo].[FormEntries] where ReceiptNumber like @ReceiptNumber";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Add parameters
                            command.Parameters.AddWithValue("@ReceiptNumber", receiptNumber);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Items.Add(new ItemModel
                                    {
                                        DriverName = reader.GetString("DriverName"),
                                        VehicleNumber = reader.GetString("VehicleNumber"),
                                        ContactNumber = reader.GetString("ContactNumber"),
                                        Purpose = reader.GetString("Purpose"),
                                        Source = reader.GetString("Source"),
                                        Remarks = reader.GetString("Remarks"),
                                    });
                                }
                            }

                        }
                    }
                }
                else if (transactionType == "Inward")
                {


                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "Select DriverName, VehicleNumber, ContactNumber,POWorkOderNumber,DCInvoicNumber, Purpose, Source, Remarks From [dbo].[FormEntries] where ReceiptNumber like @ReceiptNumber";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Add parameters
                            command.Parameters.AddWithValue("@ReceiptNumber", receiptNumber);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Items.Add(new ItemModel
                                    {
                                        DriverName = reader.GetString("DriverName"),
                                        VehicleNumber = reader.GetString("VehicleNumber"),
                                        ContactNumber = reader.GetString("ContactNumber"),
                                        Purpose = reader.GetString("Purpose"),
                                        POWorkOderNumber = reader.GetString("POWorkOderNumber"),
                                        DCInvoicNumber = reader.GetString("DCInvoicNumber"),
                                        Source = reader.GetString("Source"),
                                        Remarks = reader.GetString("Remarks"),
                                    });
                                }
                            }

                        }
                    }

                }
                else if (transactionType == "Issuance")
                {


                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "Select RequestedBy, Department, ChargeTo, Purpose, Source, Remarks From [dbo].[FormEntries] where ReceiptNumber like @ReceiptNumber";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Add parameters
                            command.Parameters.AddWithValue("@ReceiptNumber", receiptNumber);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Items.Add(new ItemModel
                                    {
                                        RequestedBy = reader.GetString("RequestedBy"),
                                        Department = reader.GetString("Department"),
                                        ChargeTo = reader.GetString("ChargeTo"),
                                        Purpose = reader.GetString("Purpose"),
                                        Source = reader.GetString("Source"),
                                        Remarks = reader.GetString("Remarks"),
                                    });
                                }
                            }

                        }
                    }

                }
                else
                {


                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "Select RequestedBy, Department, Reason, Purpose, Source, Remarks From [dbo].[FormEntries] where ReceiptNumber like @ReceiptNumber";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Add parameters
                            command.Parameters.AddWithValue("@ReceiptNumber", receiptNumber);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Items.Add(new ItemModel
                                    {
                                        RequestedBy = reader.GetString("RequestedBy"),
                                        Department = reader.GetString("Department"),
                                        Reason = reader.GetString("Reason"),
                                        Purpose = reader.GetString("Purpose"),
                                        Source = reader.GetString("Source"),
                                        Remarks = reader.GetString("Remarks"),
                                    });
                                }
                            }

                        }
                    }
                }

                return new JsonResult(Items);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public IActionResult OnGetGetInventoryTableData(string receiptNumber) 
        {
            try
            {
                List<InventoryItems> InvItems = new List<InventoryItems>(); // Declare the list outside the if block

                ConnectionClass cc = new();
                string connectionString = cc.connection;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "Select * From [dbo].[FormInventory] where ReceiptNumber like @ReceiptNumber";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters
                        command.Parameters.AddWithValue("@ReceiptNumber", receiptNumber);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                InvItems.Add(new InventoryItems
                                {
                                    ItemName = reader.GetString(reader.GetOrdinal("ItemName")),
                                    ItemCode = reader.GetString(reader.GetOrdinal("ItemCode")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    UOM = reader.GetString(reader.GetOrdinal("UOM")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    UnitPrice = reader.GetInt32(reader.GetOrdinal("UnitPrice")),
                                    TotalAmount = reader.GetInt32(reader.GetOrdinal("TotalAmount")),
                                    Currency = reader.GetString(reader.GetOrdinal("Currency")),
                                    Status = reader.GetString(reader.GetOrdinal("Status")),
                                });
                            }
                        }

                    }
                }
                return new JsonResult(InvItems);

            }
            catch (Exception ex) {
                return new JsonResult(ex.Message); // You can return the error message if needed

            }
        }
        private static void UpdateData(string tableName, Dictionary<string, object> updateData, string condition)
        {
            try
            {
                ConnectionClass cc = new();
                string connectionString = cc.connection;

                using SqlConnection connection = new(connectionString);
                connection.Open();

                List<string> setStatements = updateData.Keys.Select(key => $"{key} = @{key}").ToList();

                string setClause = string.Join(", ", setStatements);
                string query = $"UPDATE {tableName} SET {setClause} WHERE CONVERT(NVARCHAR(255),ReceiptNumber) = @condition";

                using SqlCommand command = new(query, connection);
                foreach (var entry in updateData)
                {
                   if(entry.Value == null)
                    {
                        command.Parameters.Add($"@{entry.Key}", GetSqlDbType(entry.Value)).Value = "NULL";
                    }
                    else
                    {
                        command.Parameters.Add($"@{entry.Key}", GetSqlDbType(entry.Value)).Value = entry.Value;
                    }
                }
                command.Parameters.Add("@condition", SqlDbType.NVarChar).Value = condition;

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Handle the exception
            }
        }
        private static void UpdateIData(string tableName, Dictionary<string, object> updateData, string condition1, string condition2)
        {
            try
            {
                ConnectionClass cc = new();
                string connectionString = cc.connection;

                using SqlConnection connection = new(connectionString);
                connection.Open();

                List<string> setStatements = updateData.Keys.Select(key => $"{key} = @{key}").ToList();

                string setClause = string.Join(", ", setStatements);
                string query = $"UPDATE {tableName} SET {setClause} WHERE CONVERT(NVARCHAR(255), ReceiptNumber) = @condition1 AND CONVERT(VARCHAR(MAX),ItemCode) = @condition2";

                using SqlCommand command = new(query, connection);
                foreach (var entry in updateData)
                {
                    if (entry.Value == null)
                    {
                        command.Parameters.Add($"@{entry.Key}", GetSqlDbType(entry.Value)).Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters.Add($"@{entry.Key}", GetSqlDbType(entry.Value)).Value = entry.Value;
                    }
                }
                command.Parameters.Add("@condition1", SqlDbType.NVarChar).Value = condition1;
                command.Parameters.Add("@condition2", SqlDbType.NVarChar).Value = condition2;

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Handle the exception
            }
        }


        public string getMainQuantity(string itemName)
        {
            string sih = null;
            try
            {
                ConnectionClass cc = new ConnectionClass();
                string connection = cc.connection;

                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();
                    string query = "SELECT StockInHand from MainInventory where CONVERT(VARCHAR(MAX),itemName )= \'" + itemName + "\'";

                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                sih = reader.GetString(reader.GetOrdinal("StockInHand"));

                            }
                        }
                    }
                }
                return sih;
            }
            catch (Exception ex) { return null; }
        }
        public bool updateMainQuantity(string itemName, string newStockInHand)
        {
            try
            {
                ConnectionClass cc = new ConnectionClass();
                string connection = cc.connection;

                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();

                    string updateQuery = "UPDATE MainInventory SET StockInHand = @UpdatedStock WHERE CONVERT(VARCHAR(MAX),itemName) = @ItemName";
                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, con))
                    {
                        updateCommand.Parameters.AddWithValue("@UpdatedStock", newStockInHand);
                        updateCommand.Parameters.AddWithValue("@ItemName", itemName);
                        int rowsAffected = updateCommand.ExecuteNonQuery();


                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
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


    public class ItemModel
    {
        public string DriverName { get; set; }
        public string VehicleNumber { get; set; }
        public string ContactNumber { get; set; }
        public string Purpose { get; set; }
        public string Source { get; set; }

        public string Remarks { get; set; }

        public string POWorkOderNumber { get; set; }

        public string DCInvoicNumber { get; set; }
        public string RequestedBy { get; set; }
        public string ChargeTo { get; set; }
        public string Department { get; set; }
        public string Reason { get; set; }
        // Add properties for other columns
    }
    public class InventoryItems
    {
        public string ItemName { get; set; } 
        public string ItemCode { get; set; } 
        public string Description { get; set; } 
        
        public string UOM { get; set; } 
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int TotalAmount { get; set; }

        public string Currency { get; set; }

        public string Status { get; set; }



    }
}

