using System.ComponentModel;

namespace Hospital.SharedKernel.Infrastructure.ExternalServices.VNPay.Constants
{
    public class VNPayResponseCode
    {
        [Description("Giao dịch thành công")]
        public const string SUCCESS = "00";

        [Description("Trừ tiền thành công. Giao dịch bị nghi ngờ (liên quan tới lừa đảo, giao dịch bất thường).")]
        public const string ABNORMAL = "07";

        [Description("Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng chưa đăng ký dịch vụ InternetBanking tại ngân hàng.")]
        public const string NO_INTERNET_BANKING = "09";

        [Description("Giao dịch không thành công do: Khách hàng xác thực thông tin thẻ/tài khoản không đúng quá 3 lần")]
        public const string OVER_3_TIMES = "10";

        [Description("Giao dịch không thành công do: Đã hết hạn chờ thanh toán. Xin quý khách vui lòng thực hiện lại giao dịch.")]
        public const string OVER_VALID_TIME = "11";

        [Description("Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng bị khóa.")]
        public const string CARD_BLOCKED = "12";

        [Description("Giao dịch không thành công do Quý khách nhập sai mật khẩu xác thực giao dịch (OTP). Xin quý khách vui lòng thực hiện lại giao dịch.")]
        public const string INVALID_OTP = "13";

        [Description("Giao dịch không thành công do: Khách hàng hủy giao dịch")]
        public const string CLIENT_CANCEL = "24";

        [Description("Giao dịch không thành công do: Tài khoản của quý khách không đủ số dư để thực hiện giao dịch.")]
        public const string INSUFFICIENT_BALANCE = "51";

        [Description("Giao dịch không thành công do: Tài khoản của Quý khách đã vượt quá hạn mức giao dịch trong ngày.")]
        public const string EXCEEDING = "65";

        [Description("Ngân hàng thanh toán đang bảo trì.")]
        public const string BANK_MAINTENANCE = "75";

        [Description("Giao dịch không thành công do: KH nhập sai mật khẩu thanh toán quá số lần quy định. Xin quý khách vui lòng thực hiện lại giao dịch")]
        public const string EXCEED_WRONG_PASSWORD = "79";

        [Description("Các lỗi khác")]
        public const string OTHER = "99";

        // Mã riêng của LMS
        [Description("Nhận miễn phí")]
        public const string FREE = "86";
    }
}
