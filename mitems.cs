namespace POL1
{
    public class mitems
    {
        private string itemName;
        private string itemCode;
        private string description;
        private string uom;
        private string quantity;
        private string unitPrice;
        private string totalAmount;
        private string currency;
        private string status;
        private string receiptNumber;

        public string ItemName { get => itemName; set => itemName = value; }
        public string ItemCode { get => itemCode; set => itemCode = value; }
        public string Description { get => description; set => description = value; }
        public string UOM { get => uom; set => uom = value; }
        public string Quantity { get => quantity; set => quantity = value; }
        public string UnitPrice { get => unitPrice; set => unitPrice = value; }
        public string TotalAmount { get => totalAmount; set => totalAmount = value; }
        public string Currecny { get => currency; set => currency = value; }
        public string Status { get => status; set => status = value; }
        public string ReceiptNumber { get => receiptNumber; set => receiptNumber = value; }
       
    }
}
