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
    public class PaymentRequest
    {
        /// <summary>
        /// Mã tham chiếu giao dịch (Transaction Reference). Đây là mã số duy nhất dùng để xác định giao dịch.  
        /// Lưu ý: Giá trị này bắt buộc và cần đảm bảo không bị trùng lặp giữa các giao dịch.
        /// </summary>
        public required long PaymentId { get; set; }

        /// <summary>
        /// Thông tin mô tả nội dung thanh toán, không dấu và không bao gồm các ký tự đặc biệt
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Số tiền phải nằm trong khoảng 5.000 (VND) đến 1.000.000.000 (VND).
        /// </summary>
        public required double Money { get; set; }

        /// <summary>
        /// Địa chỉ IP của thiết bị thực hiện giao dịch.  
        /// </summary>
        public required string IpAddress { get; set; }

        /// <summary>
        /// Mã phương thức thanh toán, mã loại ngân hàng hoặc ví điện tử thanh toán. Nếu mang giá trị <c>BankCode.ANY</c> thì chuyển hướng người dùng sang VNPAY chọn phương thức thanh toán.
        /// </summary>
        public BankCode BankCode { get; set; } = BankCode.ANY;

        /// <summary>
        /// Thời điểm khởi tạo giao dịch. Giá trị mặc định là ngày và giờ hiện tại tại thời điểm yêu cầu được khởi tạo.  
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
        /// </summary>
        public Currency Currency { get; set; } = Currency.VND;

        /// <summary>
        /// Ngôn ngữ hiển thị trên giao diện thanh toán của VNPAY, mặc định là tiếng Việt.  
        /// </summary>
        public DisplayLanguage Language { get; set; } = DisplayLanguage.Vietnamese;

        public Order? Order { get; set; }
    }

    /// <summary>
    /// Thông tin phản hồi thanh toán
    /// </summary>
    public class PaymentResponse
    {
        /// <summary>
        /// Mã phản hồi từ hệ thống do VNPAY định nghĩa. 
        /// </summary>
        public ResponseCode Code { get; set; }

        /// <summary>
        /// Mô tả chi tiết về mã phản hồi, cung cấp thông tin bổ sung về trạng thái giao dịch.
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// Phản hồi từ VNPAY sau khi thực hiện giao dịch thanh toán.
    /// </summary>
    public class PaymentResult
    {
        /// <summary>
        /// Mã tham chiếu giao dịch (Transaction Reference). Đây là mã số duy nhất dùng để xác định giao dịch.
        /// </summary>
        public long PaymentId { get; set; }

        /// <summary>
        /// Trạng thái thành công của giao dịch. 
        /// Giá trị là <c>true</c> nếu chữ ký chính xác, <see cref="PaymentResponse.ResponseCode"/> và <see cref="TransactionStatus"/> đều bằng <c>0</c>.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Thông tin mô tả nội dung thanh toán, viết bằng tiếng Việt không dấu.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Thời gian phản hồi từ VNPAY, được ghi nhận tại thời điểm giao dịch kết thúc.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Mã giao dịch được ghi nhận trên hệ thống VNPAY, đại diện cho giao dịch duy nhất tại VNPAY.
        /// </summary>
        public long VnpayTransactionId { get; set; }

        /// <summary>
        /// Phương thức thanh toán được sử dụng, ví dụ: thẻ tín dụng, ví điện tử, hoặc chuyển khoản ngân hàng.
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Phản hồi chi tiết từ hệ thống VNPAY về giao dịch.
        /// </summary>
        public PaymentResponse PaymentResponse { get; set; }

        /// <summary>
        /// Trạng thái giao dịch sau khi thực hiện, ví dụ: Chờ xử lý, Thành công, hoặc Thất bại.
        /// </summary>
        public TransactionStatus TransactionStatus { get; set; }

        /// <summary>
        /// Thông tin ngân hàng liên quan đến giao dịch, bao gồm tên ngân hàng và mã ngân hàng.
        /// </summary>
        public BankingInfor BankingInfor { get; set; }

        public int OrderId { get; set; }
    }

    /// <summary>
    /// Trạng thái của giao dịch sau khi được xử lý.
    /// </summary>
    public class TransactionStatus
    {
        /// <summary>
        /// Mã trạng thái của giao dịch do VNPAY định nghĩa.
        /// </summary>
        public TransactionStatusCode Code { get; set; }

        /// <summary>
        /// Mô tả chi tiết về trạng thái giao dịch.
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// Thông tin về ngân hàng liên quan đến giao dịch.
    /// </summary>
    public class BankingInfor
    {
        /// <summary>
        /// Mã ngân hàng thực hiện giao dịch. ví dụ: VCB (Vietcombank), BIDV (Ngân hàng Đầu tư và Phát triển Việt Nam).
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// Mã giao dịch ở phía ngân hàng, được dùng để theo dõi và đối soát giao dịch với ngân hàng.
        /// </summary>
        public string BankTransactionId { get; set; }
    }
}
