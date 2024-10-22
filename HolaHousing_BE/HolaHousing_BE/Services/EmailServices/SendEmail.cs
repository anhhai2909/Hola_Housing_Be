using MimeKit;
namespace HolaHousing_BE.Services.EmailServices
{
    public class SendEmail
    {

        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _username = "hola.housing.1306@gmail.com";
        private readonly string _password = "vaap gqqy pvta qklp";

        public async Task SendEmailRegularAsync(string toEmail, string subject, string mesage, string username)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Hola Housing", _username));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = subject;

            var body = new TextPart("html")
            {
                Text = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n  <head>\r\n    <meta charset=\"UTF-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"ie=edge\" />\r\n    <title>Static Template</title>\r\n\r\n    <link\r\n      href=\"https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600&display=swap\"\r\n      rel=\"stylesheet\"\r\n    />\r\n  </head>\r\n  <body\r\n    style=\"\r\n      margin: 0;\r\n      font-family: 'Poppins', sans-serif;\r\n      background: #ffffff;\r\n      font-size: 14px;\r\n    \"\r\n  >\r\n    <div\r\n      style=\"\r\n        max-width: 680px;\r\n        margin: 0 auto;\r\n        padding: 45px 30px 60px;\r\n        background: #f4f7ff;\r\n        background-image: url(https://archisketch-resources.s3.ap-northeast-2.amazonaws.com/vrstyler/1661497957196_595865/email-template-background-banner);\r\n        background-repeat: no-repeat;\r\n        background-size: 800px 452px;\r\n        background-position: top center;\r\n        font-size: 14px;\r\n        color: #434343;\r\n      \"\r\n    >\r\n\r\n      <main>\r\n        <div\r\n          style=\"\r\n            margin-bottom: 60px;\r\n            margin-top: 70px;\r\n            padding: 92px 30px 80px;\r\n            background: #ffffff;\r\n            border-radius: 30px;\r\n            text-align: center;\r\n          \"\r\n        >\r\n          <div style=\"width: 100%; max-width: 489px; margin: 0 auto;\">\r\n            <h1\r\n              style=\"\r\n                margin: 0;\r\n                font-size: 24px;\r\n                font-weight: 500;\r\n                color: #1f1f1f;\r\n              \"\r\n            >\r\n              Hola Housing\r\n            </h1>\r\n            <p\r\n              style=\"\r\n                margin: 0;\r\n                margin-top: 17px;\r\n                font-size: 16px;\r\n                font-weight: 500;\r\n              \"\r\n            >\r\n              Xin chào " + username + ",\r\n            </p>\r\n            <p\r\n              style=\"\r\n                margin: 0;\r\n                margin-top: 17px;\r\n                font-weight: 500;\r\n                letter-spacing: 0.56px;\r\n              \"\r\n            >\r\n              "+mesage+" \r\n            </p>\r\n\r\n          </div>\r\n        </div>\r\n\r\n        <p\r\n          style=\"\r\n            max-width: 400px;\r\n            margin: 0 auto;\r\n            margin-top: 90px;\r\n            text-align: center;\r\n            font-weight: 500;\r\n            color: #8c8c8c;\r\n          \"\r\n        >\r\n          Nếu cần trợ giúp, vui lòng liên hệ \r\n          <a\r\n            href=\"mailto:archisketch@gmail.com\"\r\n            style=\"color: #499fb6; text-decoration: none;\"\r\n            >hola.housing.1306@gmail.com</a\r\n          >\r\n        </p>\r\n      </main>\r\n\r\n      <footer\r\n        style=\"\r\n          width: 100%;\r\n          max-width: 490px;\r\n          margin: 20px auto 0;\r\n          text-align: center;\r\n          border-top: 1px solid #e6ebf1;\r\n        \"\r\n      >\r\n        <p\r\n          style=\"\r\n            margin: 0;\r\n            margin-top: 40px;\r\n            font-size: 16px;\r\n            font-weight: 600;\r\n            color: #434343;\r\n          \"\r\n        >\r\n          HolaHousing Team\r\n        </p>\r\n        <p style=\"margin: 0; margin-top: 8px; color: #434343;\">\r\n          Thạc Hòa, Thạch Thất, Hà Nội\r\n        </p>\r\n        <div style=\"margin: 0; margin-top: 16px;\">\r\n          <a href=\"\" target=\"_blank\" style=\"display: inline-block;\">\r\n                 </a>       \r\n        </div>\r\n        <p style=\"margin: 0; margin-top: 16px; color: #434343;\">\r\n          Copyright © 2024 Company. All rights reserved.\r\n        </p>\r\n      </footer>\r\n    </div>\r\n  </body>\r\n</html>\r\n"
            };

            email.Body = body;

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_username, _password);

                await client.SendAsync(email);
                await client.DisconnectAsync(true);
            }
        }
        public async Task SendOTPAsync(string toEmail, string otp, string username)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Hola Housing", _username));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = "Xác thực OTP";
            var body = new TextPart("html")
            {
                Text = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n  <head>\r\n    <meta charset=\"UTF-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"ie=edge\" />\r\n    <title>Static Template</title>\r\n\r\n    <link\r\n      href=\"https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600&display=swap\"\r\n      rel=\"stylesheet\"\r\n    />\r\n  </head>\r\n  <body\r\n    style=\"\r\n      margin: 0;\r\n      font-family: 'Poppins', sans-serif;\r\n      background: #ffffff;\r\n      font-size: 14px;\r\n    \"\r\n  >\r\n    <div\r\n      style=\"\r\n        max-width: 680px;\r\n        margin: 0 auto;\r\n        padding: 45px 30px 60px;\r\n        background: #f4f7ff;\r\n        background-image: url(https://archisketch-resources.s3.ap-northeast-2.amazonaws.com/vrstyler/1661497957196_595865/email-template-background-banner);\r\n        background-repeat: no-repeat;\r\n        background-size: 800px 452px;\r\n        background-position: top center;\r\n        font-size: 14px;\r\n        color: #434343;\r\n      \"\r\n    >\r\n\r\n      <main>\r\n        <div\r\n          style=\"\r\n            margin: 0;\r\n            margin-top: 70px;\r\n            padding: 92px 30px 115px;\r\n            background: #ffffff;\r\n            border-radius: 30px;\r\n            text-align: center;\r\n          \"\r\n        >\r\n          <div style=\"width: 100%; max-width: 489px; margin: 0 auto;\">\r\n            <h1\r\n              style=\"\r\n                margin: 0;\r\n                font-size: 24px;\r\n                font-weight: 500;\r\n                color: #1f1f1f;\r\n              \"\r\n            >\r\n              Hola Housing\r\n            </h1>\r\n            <p\r\n              style=\"\r\n                margin: 0;\r\n                margin-top: 17px;\r\n                font-size: 16px;\r\n                font-weight: 500;\r\n              \"\r\n            >\r\n              Xin chào "+username+",\r\n            </p>\r\n            <p\r\n              style=\"\r\n                margin: 0;\r\n                margin-top: 17px;\r\n                font-weight: 500;\r\n                letter-spacing: 0.56px;\r\n              \"\r\n            >\r\n              Cảm ơn bạn đã tin tưởng và sử dụng hệ thống của chúng tôi, dưới đây là mã OTP được cung cấp, vui lòng sử dụng để xác thực tài khoản của bạn.\r\n              Tuyệt đối không chia sẻ mã OTP với người khác.\r\n            </p>\r\n            <p\r\n              style=\"\r\n                margin: 0;\r\n                margin-top: 60px;\r\n                font-size: 40px;\r\n                font-weight: 600;\r\n                letter-spacing: 25px;\r\n                color: #ba3d4f;\r\n              \"\r\n            >\r\n              "+otp+ "\r\n            </p>\r\n          </div>\r\n        </div>\r\n\r\n        <p\r\n          style=\"\r\n            max-width: 400px;\r\n            margin: 0 auto;\r\n            margin-top: 90px;\r\n            text-align: center;\r\n            font-weight: 500;\r\n            color: #8c8c8c;\r\n          \"\r\n        >\r\n          Nếu cần trợ giúp, vui lòng liên hệ \r\n          <a\r\n            href=\"mailto:archisketch@gmail.com\"\r\n            style=\"color: #499fb6; text-decoration: none;\"\r\n            >hola.housing.1306@gmail.com</a\r\n          >\r\n        </p>\r\n      </main>\r\n\r\n      <footer\r\n        style=\"\r\n          width: 100%;\r\n          max-width: 490px;\r\n          margin: 20px auto 0;\r\n          text-align: center;\r\n          border-top: 1px solid #e6ebf1;\r\n        \"\r\n      >\r\n        <p\r\n          style=\"\r\n            margin: 0;\r\n            margin-top: 40px;\r\n            font-size: 16px;\r\n            font-weight: 600;\r\n            color: #434343;\r\n          \"\r\n        >\r\n          HolaHousing Team\r\n        </p>\r\n        <p style=\"margin: 0; margin-top: 8px; color: #434343;\">\r\n          Thạc Hòa, Thạch Thất, Hà Nội\r\n        </p>\r\n        <div style=\"margin: 0; margin-top: 16px;\">\r\n          <a href=\"\" target=\"_blank\" style=\"display: inline-block;\">\r\n          </a>       \r\n        </div>\r\n        <p style=\"margin: 0; margin-top: 16px; color: #434343;\">\r\n          Copyright © 2024 Company. All rights reserved.\r\n        </p>\r\n      </footer>\r\n    </div>\r\n  </body>\r\n</html>\r\n"
            };

            email.Body = body;

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_username, _password);

                await client.SendAsync(email);
                await client.DisconnectAsync(true);
            }
        }
        public async Task SendDeclineAsync(string toEmail, string username,string content)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Hola Housing", _username));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = "Thông báo về bài đăng";
            var body = new TextPart("html")
            {
                Text = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n  <head>\r\n    <meta charset=\"UTF-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"ie=edge\" />\r\n    <title>Static Template</title>\r\n\r\n    <link\r\n      href=\"https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600&display=swap\"\r\n      rel=\"stylesheet\"\r\n    />\r\n  </head>\r\n  <body\r\n    style=\"\r\n      margin: 0;\r\n      font-family: 'Poppins', sans-serif;\r\n      background: #ffffff;\r\n      font-size: 14px;\r\n    \"\r\n  >\r\n    <div\r\n      style=\"\r\n        max-width: 680px;\r\n        margin: 0 auto;\r\n        padding: 45px 30px 60px;\r\n        background: #f4f7ff;\r\n        background-image: url(https://archisketch-resources.s3.ap-northeast-2.amazonaws.com/vrstyler/1661497957196_595865/email-template-background-banner);\r\n        background-repeat: no-repeat;\r\n        background-size: 800px 452px;\r\n        background-position: top center;\r\n        font-size: 14px;\r\n        color: #434343;\r\n      \"\r\n    >\r\n\r\n      <main>\r\n        <div\r\n          style=\"\r\n            margin-bottom: 60px;\r\n            margin-top: 70px;\r\n            padding: 92px 30px 80px;\r\n            background: #ffffff;\r\n            border-radius: 30px;\r\n            text-align: center;\r\n          \"\r\n        >\r\n          <div style=\"width: 100%; max-width: 489px; margin: 0 auto;\">\r\n            <h1\r\n              style=\"\r\n                margin: 0;\r\n                font-size: 24px;\r\n                font-weight: 500;\r\n                color: #1f1f1f;\r\n              \"\r\n            >\r\n              Hola Housing\r\n            </h1>\r\n            <p\r\n              style=\"\r\n                margin: 0;\r\n                margin-top: 17px;\r\n                font-size: 16px;\r\n                font-weight: 500;\r\n              \"\r\n            >\r\n              Xin chào "+username+",\r\n            </p>\r\n            <p\r\n              style=\"\r\n                margin: 0;\r\n                margin-top: 17px;\r\n                font-weight: 500;\r\n                letter-spacing: 0.56px;\r\n              \"\r\n            >\r\n              Bài đăng có tiêu đề "+content+" đã bị từ chối, vui lòng kiểm tra lại nội dung trên hệ thống.\r\n            </p>\r\n\r\n          </div>\r\n        </div>\r\n\r\n        <p\r\n          style=\"\r\n            max-width: 400px;\r\n            margin: 0 auto;\r\n            margin-top: 90px;\r\n            text-align: center;\r\n            font-weight: 500;\r\n            color: #8c8c8c;\r\n          \"\r\n        >\r\n          Nếu cần trợ giúp, vui lòng liên hệ \r\n          <a\r\n            href=\"mailto:archisketch@gmail.com\"\r\n            style=\"color: #499fb6; text-decoration: none;\"\r\n            >hola.housing.1306@gmail.com</a\r\n          >\r\n        </p>\r\n      </main>\r\n\r\n      <footer\r\n        style=\"\r\n          width: 100%;\r\n          max-width: 490px;\r\n          margin: 20px auto 0;\r\n          text-align: center;\r\n          border-top: 1px solid #e6ebf1;\r\n        \"\r\n      >\r\n        <p\r\n          style=\"\r\n            margin: 0;\r\n            margin-top: 40px;\r\n            font-size: 16px;\r\n            font-weight: 600;\r\n            color: #434343;\r\n          \"\r\n        >\r\n          HolaHousing Team\r\n        </p>\r\n        <p style=\"margin: 0; margin-top: 8px; color: #434343;\">\r\n          Thạc Hòa, Thạch Thất, Hà Nội\r\n        </p>\r\n        <div style=\"margin: 0; margin-top: 16px;\">\r\n          <a href=\"\" target=\"_blank\" style=\"display: inline-block;\">\r\n \r\n          </a>       \r\n        </div>\r\n        <p style=\"margin: 0; margin-top: 16px; color: #434343;\">\r\n          Copyright © 2024 Company. All rights reserved.\r\n        </p>\r\n      </footer>\r\n    </div>\r\n  </body>\r\n</html>\r\n"
            };

            email.Body = body;

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_username, _password);

                await client.SendAsync(email);
                await client.DisconnectAsync(true);
            }
        }
        public async Task SendAcceptAsync(string toEmail, string username, string content)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Hola Housing", _username));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = "Thông báo về bài đăng";
            var body = new TextPart("html")
            {
                Text = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n  <head>\r\n    <meta charset=\"UTF-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"ie=edge\" />\r\n    <title>Static Template</title>\r\n\r\n    <link\r\n      href=\"https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600&display=swap\"\r\n      rel=\"stylesheet\"\r\n    />\r\n  </head>\r\n  <body\r\n    style=\"\r\n      margin: 0;\r\n      font-family: 'Poppins', sans-serif;\r\n      background: #ffffff;\r\n      font-size: 14px;\r\n    \"\r\n  >\r\n    <div\r\n      style=\"\r\n        max-width: 680px;\r\n        margin: 0 auto;\r\n        padding: 45px 30px 60px;\r\n        background: #f4f7ff;\r\n        background-image: url(https://archisketch-resources.s3.ap-northeast-2.amazonaws.com/vrstyler/1661497957196_595865/email-template-background-banner);\r\n        background-repeat: no-repeat;\r\n        background-size: 800px 452px;\r\n        background-position: top center;\r\n        font-size: 14px;\r\n        color: #434343;\r\n      \"\r\n    >\r\n\r\n      <main>\r\n        <div\r\n          style=\"\r\n            margin-bottom: 60px;\r\n            margin-top: 70px;\r\n            padding: 92px 30px 80px;\r\n            background: #ffffff;\r\n            border-radius: 30px;\r\n            text-align: center;\r\n          \"\r\n        >\r\n          <div style=\"width: 100%; max-width: 489px; margin: 0 auto;\">\r\n            <h1\r\n              style=\"\r\n                margin: 0;\r\n                font-size: 24px;\r\n                font-weight: 500;\r\n                color: #1f1f1f;\r\n              \"\r\n            >\r\n              Hola Housing\r\n            </h1>\r\n            <p\r\n              style=\"\r\n                margin: 0;\r\n                margin-top: 17px;\r\n                font-size: 16px;\r\n                font-weight: 500;\r\n              \"\r\n            >\r\n              Xin chào " + username + ",\r\n            </p>\r\n            <p\r\n              style=\"\r\n                margin: 0;\r\n                margin-top: 17px;\r\n                font-weight: 500;\r\n                letter-spacing: 0.56px;\r\n              \"\r\n            >\r\n              Bài đăng có tiêu đề " + content + " đã được chúng tôi chấp thuận, cảm ơn vì đã tin tưởng sử dụng hệ thống của chúng tôi. \r\n            </p>\r\n\r\n          </div>\r\n        </div>\r\n\r\n        <p\r\n          style=\"\r\n            max-width: 400px;\r\n            margin: 0 auto;\r\n            margin-top: 90px;\r\n            text-align: center;\r\n            font-weight: 500;\r\n            color: #8c8c8c;\r\n          \"\r\n        >\r\n          Nếu cần trợ giúp, vui lòng liên hệ \r\n          <a\r\n            href=\"mailto:archisketch@gmail.com\"\r\n            style=\"color: #499fb6; text-decoration: none;\"\r\n            >hola.housing.1306@gmail.com</a\r\n          >\r\n        </p>\r\n      </main>\r\n\r\n      <footer\r\n        style=\"\r\n          width: 100%;\r\n          max-width: 490px;\r\n          margin: 20px auto 0;\r\n          text-align: center;\r\n          border-top: 1px solid #e6ebf1;\r\n        \"\r\n      >\r\n        <p\r\n          style=\"\r\n            margin: 0;\r\n            margin-top: 40px;\r\n            font-size: 16px;\r\n            font-weight: 600;\r\n            color: #434343;\r\n          \"\r\n        >\r\n          HolaHousing Team\r\n        </p>\r\n        <p style=\"margin: 0; margin-top: 8px; color: #434343;\">\r\n          Thạc Hòa, Thạch Thất, Hà Nội\r\n        </p>\r\n        <div style=\"margin: 0; margin-top: 16px;\">\r\n          <a href=\"\" target=\"_blank\" style=\"display: inline-block;\">\r\n \r\n          </a>       \r\n        </div>\r\n        <p style=\"margin: 0; margin-top: 16px; color: #434343;\">\r\n          Copyright © 2024 Company. All rights reserved.\r\n        </p>\r\n      </footer>\r\n    </div>\r\n  </body>\r\n</html>\r\n"
            };

            email.Body = body;

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_username, _password);

                await client.SendAsync(email);
                await client.DisconnectAsync(true);
            }
        }
    }
}
