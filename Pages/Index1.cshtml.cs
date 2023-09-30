using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;


namespace POL1.Pages
{
    public class Index1Model : PageModel
    {
        // Model properties for your existing code
        [BindProperty]
        public string Date { get; set; } = "";

        [BindProperty]
        public string ReceiptNumber { get; set; } = "";

        [BindProperty]
        public string TransactionType { get; set; } = "";

       // public class Outgoing : PageModel
      //  {
            [BindProperty]
            public string DriverName { get; set; } = "";

            [BindProperty]
            public string VehicleNumber { get; set; } = "";
            [BindProperty]
            public string ContactNumber { get; set; } = "";
            [BindProperty]
            public string Purpose { get; set; } = "";
            [BindProperty]
            public string Source { get; set; } = "";
            [BindProperty]
            public string Remarks { get; set; } = "";
        [BindProperty]
        public string RequestedBy { get; set; } = "";
        [BindProperty]
        public string Department { get; set; } = "";
        [BindProperty]
        public string Reason { get; set; } = "";

        // }


        public List<ReturnableItem> ReturnableItems { get; set; } = new List<ReturnableItem>();


        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            // Handle your existing form submission here
            // You can access the form values via the properties like Date, ReceiptNumber, and TransactionType
            // Perform any necessary processing and return an appropriate response
            // For example, you can redirect to another page or return a PartialViewResult

            // For loading returnable items, you can call the new method here
            LoadReturnableItems("initialReceiptNumber");

            return Page();
        }

        // Method for loading returnable items
        private void LoadReturnableItems(string receiptNumber)
        {
            // Query your data source to get returnable items based on receiptNumber
            // Example: ReturnableItems = yourDataService.GetReturnableItems(receiptNumber);

            // Simulating data for demonstration purposes (replace with your data retrieval logic)
            ReturnableItems = new List<ReturnableItem>
            {
                new ReturnableItem { ItemName = "Item 1", ItemCode = "Code 1", Description = "Description 1", Category = "Category 1", UOM = "UOM 1", Quantity = 5 },
                new ReturnableItem { ItemName = "Item 2", ItemCode = "Code 2", Description = "Description 2", Category = "Category 2", UOM = "UOM 2", Quantity = 10 },
            };
        }
    }
}


public class ReturnableItem
    {
        public string ItemName { get; set; } = "";
        public string ItemCode { get; set; } = "";
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public string UOM { get; set; } = "";
        public int Quantity { get; set; }
    }




