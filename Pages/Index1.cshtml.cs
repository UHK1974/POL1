using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Drawing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using PdfSharp.Drawing.Layout;

namespace POL1.Pages
{
    public class Index1Model : PageModel
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
        //[BindProperty]
        //public string Category { get; set; } = "";
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
        public IEnumerable<itemName> itemName { get; set; }

        public List<ReturnableItem> ReturnableItems { get; set; } = new List<ReturnableItem>();

        public static string receiptNumber = "NA";
        public void OnGet()
        {
            itemName = GetItemsName();
            string timestamp = DateTime.Now.ToString("ddMMyy-HHmm");
            Random random = new Random();
            string uniqueIdentifier = random.Next(1000, 9999).ToString();
            ReceiptNumber = "POL-" + timestamp + uniqueIdentifier;
            receiptNumber = ReceiptNumber;
        }
        List<Dictionary<string, object>> mainList;
        private IWebHostEnvironment _webHostEnvironment;

        public Index1Model(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult OnGetGetData(string itemName)
        {
            ConnectionClass cc = new();
            string connectionString = cc.connection;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT ItemCode, Description, UOM FROM [MainInventory] WHERE ItemName like @ItemName";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemName", itemName);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var itemData = new
                            {
                                itemCode = reader["ItemCode"].ToString(),
                                description = reader["Description"].ToString(),
                                uom = reader["UOM"].ToString()
                            };
                            return new JsonResult(itemData);
                        }
                        else
                        {
                            // Handle the case where the item name is not found
                            var itemData = new
                            {
                                itemCode = "",
                                description = "",
                                uom = ""
                            };
                            return new JsonResult(itemData);
                        }
                    }
                }
                return NotFound();
            }

       // Handle other errors here if needed
        }
        // Replace this with your actual data access logic

        public IEnumerable<itemName> GetItemsName()
        {
            ConnectionClass cc = new();
            string connectionString = cc.connection;
            // Connect to the database and retrieve items
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                List<itemName> items = new List<itemName>();

                using (SqlCommand command = new SqlCommand("SELECT ItemName FROM MainInventory", connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new itemName
                        {
                            Name = reader.GetString(reader.GetOrdinal("ItemName"))

                        });
                    }
                }

                    return items;
            }
        }
        public IActionResult OnPost()
        {
            

            if (Request.Form["generatePDF"].Count>0) 
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

              
                var pdfContent = GeneratePdfContent(formDataList);

                
                string namePDF = receiptNumber+".pdf";
                return File(pdfContent, "application/pdf", namePDF);
            }
            else {
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

                // string receiptNumber = "WLJND" + DateTime.Now.ToString("ddMMyyHHmmss");

                string tableName = "FormEntries"; // Change this based on your needs
                Dictionary<string, object> data = null;
                if (TransactionType == "Outward")
                {
                    data = new Dictionary<string, object>
            {
                { "Date", myDate },
                { "ReceiptNumber", receiptNumber },
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
                 { "Date", myDate },
                 { "ReceiptNumber", receiptNumber },
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
                 { "Date", myDate },
                 { "ReceiptNumber", receiptNumber },
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
                 { "Date", myDate },
                 { "ReceiptNumber", receiptNumber },
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
                InsertData(tableName, data);

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
                    { "Status", Statusl[x] },
                    {"UpdatedStatus",Statusl[x] },
                    { "ReceiptNumber", receiptNumber },
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
                foreach (Dictionary<string, object> y in mainList)
                {
                    InsertData("FormInventory", y);
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

                return RedirectToPage("/Index1");
            }
        }
        private byte[] GeneratePdfContent(List<KeyValuePair<string, string>> data)
        {
            using (PdfDocument document = new PdfDocument())
            {
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Arial", 10);
                XFont font1 = new XFont("Arial", 10, XFontStyle.Bold);
                XFont fontX = new XFont("Arial", 8);
                XPoint startPoint = new XPoint(25, 20);


                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "img", "pol.png");
                XImage image = XImage.FromFile(imagePath);

                gfx.DrawImage(image, 400, 5, 100, 100);
                startPoint.Y = 50;

                if (TransactionType == "Outward")
                {
                    gfx.DrawString("Date            " + ":" + Date, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Receipt Number  " + ":" + receiptNumber, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Transaction Type" + ":" + TransactionType, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Driver Name     " + ":" + DriverName1, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Vehicle Number  " + ":" + VehicleNumber1, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Contact Number  " + ":" + ContactNumber1, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Purpose         " + ":" + Purpose1, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Source          " + ":" + Source1, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Remarks         " + ":" + Remarks1, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                   // gfx.DrawString("Status          " + ":" + Status, font, XBrushes.Black, startPoint); startPoint.Y += 20;

                }
                else if (TransactionType == "Inward")
                {

                    gfx.DrawString("Date            " + ":" + Date, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Receipt Number  " + ":" + receiptNumber, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Transaction Type" + ":" + TransactionType, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Driver Name     " + ":" + DriverName2, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Vehicle Number  " + ":" + VehicleNumber2, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Contact Number  " + ":" + ContactNumber2, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Purpose         " + ":" + Purpose2, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Source          " + ":" + Source2, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Remarks         " + ":" + Remarks2, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                   // gfx.DrawString("Status" + ":" + Status, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("PO Work Oder Number" + ":" + POWorkOderNumber1, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("DC Invoice Number" + ":" + DCInvoicNumber1, font, XBrushes.Black, startPoint); startPoint.Y += 20;



                }
                else if (TransactionType == "Issuance")
                {

                    gfx.DrawString("Date" + ":" + Date, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Receipt Number" + ":" + receiptNumber, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Transaction  Type" + ":" + TransactionType, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("RequestedBy" + ":" + RequestedBy1, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Department" + ":" + Department1, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Reason" + ":" + Reason1, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Purpose" + ":" + Purpose3, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Source" + ":" + Source3, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Remarks" + ":" + Remarks3, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                   // gfx.DrawString("Status" + ":" + Status, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Charge To" + ":" + ChargeTo1, font, XBrushes.Black, startPoint); startPoint.Y += 20;


                }
                else if (TransactionType == "Receive")
                {
                    gfx.DrawString("Date" + ":" +Date, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Receipt Number" + ":" + receiptNumber, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Transaction Type" + ":" + TransactionType, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Requested By" + ":" + RequestedBy2, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Department" + ":" + Department2, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Reason" + ":" + Reason2, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Purpose" + ":" + Purpose4, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Source" + ":" + Source4, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    gfx.DrawString("Remarks" + ":" + Remarks4, font, XBrushes.Black, startPoint); startPoint.Y += 20;
                    //gfx.DrawString("Status" + ":" + Status, font, XBrushes.Black, startPoint); startPoint.Y += 20;

                }
              

                double rectWidth = 60;
                double rectHeight = 20;

                
                XPen pen = new XPen(XColors.Black, 1); 
                DrawRectangleAroundString(gfx, startPoint, rectWidth, rectHeight, font1, pen, "Item Name");
                startPoint.X += 60;

                DrawRectangleAroundString(gfx, startPoint, rectWidth, rectHeight, font1, pen, "Item Code");
                startPoint.X += 60;

                DrawRectangleAroundString(gfx, startPoint, rectWidth, rectHeight, font1, pen, "Item Desc");
                startPoint.X += 60;

                DrawRectangleAroundString(gfx, startPoint, rectWidth, rectHeight, font1, pen, "  UOM    ");
                startPoint.X += 60;

                DrawRectangleAroundString(gfx, startPoint, rectWidth, rectHeight, font1, pen, "Quantity ");
                startPoint.X += 60;

                DrawRectangleAroundString(gfx, startPoint, rectWidth, rectHeight, font1, pen, "Unit Price");
                startPoint.X += 60;

                DrawRectangleAroundString(gfx, startPoint, rectWidth, rectHeight, font1, pen, "Total  ");
                startPoint.X += 60;

                DrawRectangleAroundString(gfx, startPoint, rectWidth, rectHeight, font1, pen, "Currency  ");
                startPoint.X += 60;

                DrawRectangleAroundString(gfx, startPoint, rectWidth, rectHeight, font1, pen, "Status    ");


                


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


                foreach (var dataX in data)
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
                Boolean first = false;
                startPoint.Y += 20;
                foreach (string dat in ItemNamel)
                {
                    rectHeight = 60;

                    //if (Descriptionl[x].Length > 8 )
                    //{
                    //    rectHeight = 60;

                    //}
                    //else
                    //{
                    //    rectHeight = 60;

                    //}

                    startPoint.X = 25;



                    if (ItemNamel[x].ToString().Contains(" "))
                    {
                       
                        DrawRectangleAroundStringWithWordWrap(gfx, startPoint, rectWidth, rectHeight, fontX, pen, ItemNamel[x]);
                        startPoint.X += 60;
                        first = true;

                    }
                    else
                    {
                        DrawRectangleAroundString(gfx, startPoint, rectWidth, rectHeight, fontX, pen, ItemNamel[x]);
                        startPoint.X += 60;
                        first = false;
                    }
                    //DrawRectangleAroundString(gfx, startPoint, rectWidth, rectHeight, fontX, pen, ItemNamel[x]);
                    //startPoint.X += 60;

                    DrawRectangleAroundString(gfx, startPoint, rectWidth, rectHeight, fontX, pen, ItemCodel[x]);
                    startPoint.X += 60;

                    DrawRectangleAroundStringWithWordWrap(gfx, startPoint, rectWidth, rectHeight, fontX, pen, Descriptionl[x]);
                    startPoint.X += 60;

                    DrawRectangleAroundString(gfx, startPoint, rectWidth, rectHeight, fontX, pen, UOMl[x]);
                    startPoint.X += 60;

                    DrawRectangleAroundString(gfx, startPoint, rectWidth, rectHeight, fontX, pen, Quantityl[x]);
                    startPoint.X += 60;

                    DrawRectangleAroundString(gfx, startPoint, rectWidth, rectHeight, fontX, pen, UnitPricel[x]);
                    startPoint.X += 60;

                    DrawRectangleAroundString(gfx, startPoint, rectWidth, rectHeight, fontX, pen, TotalAmountl[x]);
                    startPoint.X += 60;

                    DrawRectangleAroundString(gfx, startPoint, rectWidth, rectHeight, fontX, pen, Currencyl[x]);
                    startPoint.X += 60;

                    DrawRectangleAroundString(gfx, startPoint, rectWidth, rectHeight, fontX, pen, Statusl[x]);
                    x++;

                    try
                    {
                        if (first)
                        {
                            startPoint.Y += 60;

                        }
                        else
                        {
                            startPoint.Y += 60;

                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    document.Save(stream, false);
                    return stream.ToArray();
                }
            }
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

        static void DrawRectangleAroundString(XGraphics gfx, XPoint startPoint, double width, double height, XFont font, XPen pen, string text)
        {
            XRect layoutRectangle = new XRect(startPoint.X, startPoint.Y, width, height);

            // Draw the rectangle
            gfx.DrawRectangle(pen, layoutRectangle);

            // Draw the string inside the rectangle
            gfx.DrawString(text, font, XBrushes.Black, layoutRectangle, XStringFormats.Center);
        }
        // Helper method to draw a rectangle around a string with word-wrapping
        static void DrawRectangleAroundStringWithWordWrap(XGraphics gfx, XPoint startPoint, double width, double height, XFont font, XPen pen, string text)
        {
            XRect layoutRectangle = new XRect(startPoint.X, startPoint.Y, width, height);

            // Draw the rectangle
            gfx.DrawRectangle(pen, layoutRectangle);

            // Calculate the number of lines required for word-wrapping
            string[] words = text.Split(' ');
            List<string> lines = new List<string>();
            string currentLine = "";

            foreach (string word in words)
            {
                string testLine = currentLine + word + " ";
                if (gfx.MeasureString(testLine, font).Width <= width)
                {
                    currentLine = testLine;
                }
                else
                {
                    lines.Add(currentLine);
                    currentLine = word + " ";
                }
            }
            lines.Add(currentLine);

            // Draw each line inside the rectangle
            XStringFormat format = new XStringFormat();
            format.Alignment = XStringAlignment.Center + 10;
            
            // You can adjust alignment as needed
            XPoint textPoint = new XPoint(layoutRectangle.Left + 2, layoutRectangle.Top + 2);
            foreach (string line in lines)
            {
                gfx.DrawString(line, font, XBrushes.Black, textPoint, format);
                textPoint.Y += 10;
            }
        }

        public string getMainQuantity(string itemName)
        {
            string sih = null;
            try
            {
                ConnectionClass cc = new ConnectionClass();
                string connection = cc.connection;

                using (SqlConnection con = new SqlConnection(connection)) {
                    con.Open();
                    string query = "SELECT StockInHand from MainInventory where CONVERT(VARCHAR(MAX),itemName )= \'" + itemName + "\'";

                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                               sih =  reader.GetString(reader.GetOrdinal("StockInHand"));

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

    public class ReturnableItem
    {
        [Required]
        public string ItemName { get; set; } = "";
        public string ItemCode { get; set; } = "";
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public string UOM { get; set; } = "";
        public int Quantity { get; set; }
    }
    public class itemName
    {
        public string Name { get; set; }
    }
}
