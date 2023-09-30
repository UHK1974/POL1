using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace POL1.Pages
{
    public class Index2Model : PageModel
    {

        // Model properties
        [BindProperty]
        public string Date { get; set; }

        [BindProperty]
        public string ItemName { get; set; }

        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public string ItemCode { get; set; }

        [BindProperty]
        public string Cotegory { get; set; }

        [BindProperty]
        public string UOM { get; set; }
        public Index2Model()
        {
            // Initialize your model properties here if needed
            Date = "";
            ItemName = "";
            Description = "";
            ItemCode = "";
            Cotegory = "";
            UOM = "";
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            // Handle form submission here
            // You can access the form values via the properties like Date, ReceiptNumber, and TransactionType
            // Perform any necessary processing and return an appropriate response
            // For example, you can redirect to another page or return a PartialViewResult
            return Page();
        }
    }
}