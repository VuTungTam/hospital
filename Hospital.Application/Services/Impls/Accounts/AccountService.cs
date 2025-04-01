using Hospital.SharedKernel.Application.Services.Accounts.Interfaces;
using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.CoreConfigs;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Infrastructure.Services.Emails;
using Hospital.SharedKernel.Infrastructure.Services.Emails.Models;
using Hospital.SharedKernel.Libraries.ExtensionMethods;

namespace Hospital.Application.Services.Impls.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly IEmailService _emailService;

        public AccountService(
            IEmailService emailService
        )
        {
            _emailService = emailService;
        }

        public async Task SendChangePasswordNoticeAsync(BaseUser user, RequestInfo requestInfo, CancellationToken cancellationToken)
        {
            var content = $@"
              <p>Xin chào <strong>{user.Name}</strong>,</p>
              <div style='margin-bottom: 4px'>Chúng tôi gửi thông báo này để xác nhận rằng bạn đã thay đổi mật khẩu tại <strong>HealthMate</strong>, với thông tin như sau:</div>
              <div>Địa chỉ truy cập: <strong>{requestInfo.Ip}</strong></div>
              <div>Trình duyệt: <strong>{requestInfo.Browser}</strong></div>
              <div style='margin-bottom: 8px'>Hệ điều hành: <strong>{requestInfo.OS}</strong></div>
              <div>Bây giờ, bạn đã có thể đăng nhập với mật khẩu mới.</div>
              <br/>
            ";
            var message = GetFrameMessage("Thay đổi thông tin bảo mật", content);

            await _emailService.SendAsync(new EmailOptionRequest { To = user.Email, Body = message, Subject = "[Quan trọng] Bạn vừa thay đổi mật khẩu" }, cancellationToken);
        }

        public async Task SendVerifyEmailAsync(BaseUser user, string code, CancellationToken cancellationToken)
        {
            var content = $@"
              <p>Xin chào <strong>{user.Name}</strong>,</p>
              <p>Cảm ơn bạn đã tạo tài khoản tại <strong>HealthMate</strong>, vui lòng bấm vào nút bên dưới để xác thực tài khoản của bạn</p>
              <div style='display: flex; justify-content: center; margin: 24px 0;'>
                <a href='{ClientInfoConfig.Url}/xac-thuc-tai-khoan?e={user.Email.ToBase64Encode()}&c={code}' target='_blank' style='display: block; padding: 12px 16px; margin: 0 auto; border-radius: 4px; background-color: #f0923a; color: #fff; text-decoration: none'>
                  Xác minh tài khoản
                </a>
              </div>
              <div style='margin-bottom: 24px;'>Nếu bạn cho rằng đây là sự nhầm lẫn, hãy bỏ qua email này.</div>
            ";
            var message = GetFrameMessage("Xác minh tài khoản HealthMate", content);

            await _emailService.SendAsync(new EmailOptionRequest { To = user.Email, Body = message, Subject = "Xác nhận tài khoản HealthMate" }, cancellationToken);
        }

        public async Task SendVerifyEmailWithPasswordAsync(BaseUser user, string code, CancellationToken cancellationToken)
        {
            var content = $@"
              <p>Xin chào <strong>{user.Name}</strong>,</p>
              <p>Cảm ơn bạn đã tạo tài khoản tại <strong>HealthMate</strong>, vui lòng bấm vào nút bên dưới để xác thực tài khoản của bạn</p>
              <div style='display: flex; justify-content: center; margin: 24px 0;'>
                <a href='{ClientInfoConfig.Url}/xac-thuc-tai-khoan?e={user.Email.ToBase64Encode()}&c={code}' target='_blank' style='display: block; padding: 12px 16px; margin: 0 auto; border-radius: 4px; background-color: #f0923a; color: #fff; text-decoration: none'>
                  Xác minh tài khoản
                </a>
              </div>

              <p><b>Sau đó, bạn có thể đăng nhập với mật khẩu là:</b></p>
              <div style='display: flex; padding: 16px 24px; margin: 4px 24px 12px 24px; border-radius: 4px; background-color: #f1f1f1; text-align: center;'>
                <span style='display: inline-block; margin: 0 auto; font-weight: 700;'>{user.Password}</span>
              </div>
              <div style='margin-bottom: 24px;'>Nếu bạn cho rằng đây là sự nhầm lẫn, hãy bỏ qua email này.</div>
            ";
            var message = GetFrameMessage("Xác minh tài khoản HealthMate", content);

            await _emailService.SendAsync(new EmailOptionRequest { To = user.Email, Body = message, Subject = "Xác nhận tài khoản HealthMate" }, cancellationToken);
        }

        public async Task SendForgotPwdAsync(BaseUser user, string code, CancellationToken cancellationToken)
        {
            var content = $@"
              <p>Xin chào <strong>{user.Name}</strong>,</p>
              <p>Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn. Dưới đây là mã xác nhận:</p>
              <div style='margin: 24px 0;'>
                <div style='margin-bottom: 12px; text-align: center; font-weight: 700; font-size: 20px'>{code}</div>
                <div style='text-align: center; font-style: italic; font-size: 12px;'>Mã có hiệu lực trong 1h</div>
              </div>
            ";
            var message = GetFrameMessage("Quên mật khẩu", content);

            await _emailService.SendAsync(new EmailOptionRequest { To = user.Email, Body = message, Subject = "[Quan trọng] Quên mật khẩu HealthMate" }, cancellationToken);
        }

        public async Task SendPasswordForUserAsync(BaseUser user, CancellationToken cancellationToken)
        {
            var content = $@"
              <p>Xin chào <strong>{user.Name}</strong>,</p>
              <p>Mật khẩu cho tài khoản tại HealthMate của bạn là:</p>
              <div style='display: flex; padding: 16px 24px; margin: 16px 24px; border-radius: 4px; background-color: #f1f1f1; text-align: center;'>
                <span style='display: inline-block; margin: 0 auto; font-weight: 700;'>{user.Password}</span>
              </div>
            ";
            var message = GetFrameMessage("Tài khoản tại HealthMate", content);

            await _emailService.SendAsync(new EmailOptionRequest { To = user.Email, Body = message, Subject = "[Quan trọng] Tài khoản tại HealthMate" }, cancellationToken);
        }

        #region Frame
        private string GetFrameMessage(string title, string content)
        {
            var message = $@"
            <table width='100%' height='100%' style='min-width:348px' border='0' cellspacing='0' cellpadding='0' lang='en'>
              <tbody>
                <tr height='32' style='height:32px'>
                  <td></td>
                </tr>
                <tr align='center'>
                  <td>
                    <table border='0' cellspacing='0' cellpadding='0' style='padding-bottom:20px;max-width:516px;min-width:220px'>
                      <tbody>
                        <tr>
                          <td width='8' style='width:8px'></td>
                          <td>
                            <div
                              style='border-style:solid;border-width:thin;border-color:#dadce0;border-radius:8px;padding:28px 20px'
                              align='center' class='m_-82709585843158686mdv2rw'><img
                                src='https://cdn.oapi.vn/lg-20240628153441.png?token=cvh6685467'
                                width='140px' height='96' aria-hidden='true' style='margin-bottom:8px' alt='HealthMate' class='CToWUd'
                                data-bit='iit'>
                              <div
                                style='font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif;border-bottom:thin solid #dadce0;color:rgba(0,0,0,0.87);line-height:32px;padding-bottom:24px;text-align:center;word-break:break-word'>
                                <div style='font-size:24px'>{title}</div>
                              </div>
                              <div style='font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:14px;color:rgba(0,0,0,0.87);line-height:20px;padding-top:20px;text-align:left'>
                                {content}
                                <div>Thanks,</div>
                                <div>HealthMate</div>
                              </div>
                            </div>
                          </td>
                          <td width='8' style='width:8px'></td>
                        </tr>
                      </tbody>
                    </table>
                  </td>
                </tr>
                <tr height='32' style='height:32px'>
                  <td></td>
                </tr>
              </tbody>
            </table>
            ";
            return message;
        }
        #endregion
    }
}
