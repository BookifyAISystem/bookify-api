using bookify_data.Entities;
using bookify_data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace bookify_data.Model
{
    /// <summary>
    /// Yêu cầu thanh toán gửi đến cổng thanh toán VNPAY.
    /// </summary>
    public class VnpayPaymentRequest
    {
        public required long PaymentId { get; set; }
        public required string Description { get; set; }
        public required double Money { get; set; }
        public required string IpAddress { get; set; }
        public BankCode BankCode { get; set; } = BankCode.ANY;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Currency Currency { get; set; } = Currency.VND;
        public DisplayLanguage Language { get; set; } = DisplayLanguage.Vietnamese;
    }
    public class VnpayPaymentResponse
    {
        public ResponseCode Code { get; set; }
        public string Description { get; set; }
    }

    public class VnpayPaymentResult
    {
        public long PaymentId { get; set; }
        public bool IsSuccess { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public long VnpayTransactionId { get; set; }
        public string PaymentMethod { get; set; }
        public VnpayPaymentResponse PaymentResponse { get; set; }
        public VnpayTransactionStatus TransactionStatus { get; set; }
        public VnpayBankingInfor BankingInfor { get; set; }

    }
    public class VnpayTransactionStatus
    {
        public TransactionStatusCode Code { get; set; }
        public string Description { get; set; }
    }
    public class VnpayBankingInfor
    {
        public string BankCode { get; set; }
        public string BankTransactionId { get; set; }
    }
}
