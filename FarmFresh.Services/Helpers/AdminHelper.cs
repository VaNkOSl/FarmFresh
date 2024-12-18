using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using FarmFresh.ViewModels.Email;
using LoggerService;
using LoggerService.Contacts;
using Microsoft.EntityFrameworkCore;
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

    public static async Task<Product> GetProductByIdAsync(
        Guid productId,
        bool trackChanges,
        IRepositoryManager _repositoryManager,
        ILoggerManager _loggerManager)
    {
        var product = await _repositoryManager.ProductRepository
            .FindProductByConditionAsync(p => p.Id == productId, trackChanges)
        .FirstOrDefaultAsync();

        ProductHelper.CheckProductNotFound(product, productId, nameof(GetProductByIdAsync), _loggerManager);
        return product;
    }

    public static async Task<Farmer> GetFarmerByIdAsync(
        Guid farmerId,
        bool trackChanges,
        IRepositoryManager _repositoryManager,
        ILoggerManager _loggerManager)
    {
        var farmer = await _repositoryManager.FarmerRepository
            .FindFarmersByConditionAsync(f => f.Id == farmerId, trackChanges)
            .Include(u => u.User)
        .FirstOrDefaultAsync();

        FarmerHelper.ChekFarmerNotFound(farmer, farmerId, nameof(GetFarmerByIdAsync), _loggerManager);

        return farmer;
    }
}
