using System;

namespace Facade.Task1.OrderPlacement
{
    public class OrderFacade
    {
        readonly InvoiceSystem invoiceSystem;
        readonly PaymentSystem paymentSystem;
        readonly ProductCatalog productCatalog;

        public OrderFacade(InvoiceSystem invoiceSystem, PaymentSystem paymentSystem, ProductCatalog productCatalog)
        {
            this.invoiceSystem = invoiceSystem ?? throw new ArgumentException("Invoice system was not passed.");
            this.paymentSystem = paymentSystem ?? throw new ArgumentException("Payment system was not passed.");
            this.productCatalog = productCatalog ?? throw new ArgumentException("Product catalog was not passed.");
        }

        public void PlaceOrder(string productId, int quantity, string email)
        {
            CheckNotEmpty(productId, "Product quantity is equal or less than 0 (zero).");
            CheckPositiveNumber(quantity, "Product quantity is equal or less than 0 (zero).");

            var product = productCatalog.GetProductDetails(productId);

            if (product == null)
            {
                throw new Exception("Product was not found");
            }

            var payment = MakePayment(product, quantity);
            SendInvoice(payment, email);
        }

        private void CheckNotEmpty(string value, string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(message);
            }
        }

        private void CheckPositiveNumber(int number, string message)
        {
            if (number <= 0)
            {
                throw new ArgumentException(message);
            }
        }

        private Payment MakePayment(Product product, int quantity)
        {
            var payment = new Payment()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = quantity,
                TotalPrice = product.Price,
            };

            paymentSystem.MakePayment(payment);

            return payment;
        }

        private void SendInvoice(Payment payment, string email)
        {
            var sendDate = DateTime.Today;

            var invoice = new Invoice()
            {
                ProductId = payment.ProductId,
                ProductName = payment.ProductName,
                Quantity = payment.Quantity,
                TotalPrice = payment.TotalPrice,
                CustomerEmail = email,
                InvoiceNumber = Guid.NewGuid(),
                SendDate = sendDate,
                DueDate = sendDate.AddDays(1),
            };

            invoiceSystem.SendInvoice(invoice);
        }
    }
}
