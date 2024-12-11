using FarmFresh.ViewModels.Email;
using LoggerService.Contacts;
using SendGrid;
using SendGrid.Helpers.Mail;
using static FarmFresh.Commons.GeneralApplicationConstants;
using static FarmFresh.Commons.MessagesConstants;

namespace FarmFresh.Services.Helpers;

public static class AdminHelper
{
    public static async Task SendRejectEmailAsync<T>(
        T model,
        string _sendGridApiKey,
        ILoggerManager loggerManager) where T : IRejectEmailModel
    {
        var clien = new SendGridClient(_sendGridApiKey);

        var from = new EmailAddress(model.EmailFrom, AdminRoleName);
        var to = new EmailAddress(model.EmailTo);

        var subject = model.EmailSubject ?? ProductRejectNotification;
        var body = model.EmailBody ?? string.Format(RejectProductEmailBody, model.Name);

        var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);

        try
        {
            var response = await clien.SendEmailAsync(msg);

            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                var responseBody = await response.Body.ReadAsStringAsync();
                loggerManager.LogError($"Failed to send rejection email to {model.EmailTo}. Response code: {response.StatusCode}");
                loggerManager.LogError($"Error details: {responseBody}");
            }

            loggerManager.LogInfo($"[{nameof(SendRejectEmailAsync)}] Successfully send email to farmer with email {model.EmailTo}");
        }
        catch (Exception ex)
        {
            loggerManager.LogError($"Error while sending rejection email: {ex.Message}");
            throw;
        }
    }
}
