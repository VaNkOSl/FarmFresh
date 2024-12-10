using AutoMapper;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.Admin;
using LoggerService.Contacts;
using LoggerService.Exceptions.NotFound;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using static FarmFresh.Commons.GeneralApplicationConstants;
using static FarmFresh.Commons.MessagesConstants;

namespace FarmFresh.Services;

internal sealed class AdminService : IAdminService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    private readonly string _sendGridApiKey;

    public AdminService(IRepositoryManager repositoryManager,
                        ILoggerManager loggerManager,
                        IMapper mapper, IConfiguration configuration)
    {
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _mapper = mapper;
        _sendGridApiKey = configuration["SendGrid:ApiKey"];
    }

    public async Task ApproveProductAsync(Guid productId, bool trackChanges)
    {
        var productForApproving = await
            _repositoryManager
            .ProductRepository
            .FindProductByConditionAsync(p => p.Id == productId, trackChanges)
            .FirstOrDefaultAsync();

        CheckProductNotFound(productForApproving, productId, nameof(ApproveProductAsync));

        productForApproving.ProductStatus = ProductStatus.Approved;
        _repositoryManager.ProductRepository.UpdateProduct(productForApproving);
        await _repositoryManager.SaveAsync();
        _loggerManager.LogInfo($"[{nameof(ApproveProductAsync)}] Successfully approved produc with Id {productId}");
    }

    public async Task<AdminRejectViewModel> GetProductForRejecAsync(Guid productId,bool trackChanges)
    {
        var productForReject = await
            _repositoryManager
            .ProductRepository
            .FindProductByConditionAsync(p => p.Id == productId, trackChanges)
            .Include(c => c.Category)
            .Include(f => f.Farmer)
            .ThenInclude(u => u.User)
            .Include(ph => ph.ProductPhotos)
            .FirstOrDefaultAsync();

        CheckProductNotFound(productForReject, productId, nameof(ApproveProductAsync));

        return _mapper.Map<AdminRejectViewModel>(productForReject);
    }

    public async Task<IEnumerable<AdminAllProductDto>> GetUnapprovedProductsAsync(bool trackChanges)
    {
        var products = await
            _repositoryManager
            .ProductRepository
            .FindAllProducts(trackChanges)
            .Where(p => p.ProductStatus == ProductStatus.PendingApproval)
            .Include(f => f.Farmer)
            .Include(c => c.Category)
            .Include(ph => ph.ProductPhotos)
            .ToListAsync();

        return _mapper.Map<List<AdminAllProductDto>>(products);
    }

    public async Task RejectProductAsync(AdminRejectViewModel model, bool trackChanges)
    {
        var product = await
            _repositoryManager
            .ProductRepository
            .FindProductByConditionAsync(p => p.Id == model.Id, trackChanges)
            .FirstOrDefaultAsync();


        if (product != null && product.ProductStatus != ProductStatus.Approved)
        {
            product.ProductStatus = ProductStatus.Rejected;
            _repositoryManager.ProductRepository.UpdateProduct(product);
            await _repositoryManager.SaveAsync();
        }

        await SendRejectProductEmailAsync(model);
    }

    private async Task SendRejectProductEmailAsync(AdminRejectViewModel model)
    {
        var clien = new SendGridClient(_sendGridApiKey);

        var from = new EmailAddress(model.EmailFrom, AdminRoleName);
        var to = new EmailAddress(model.EmailTo);


        var subject = model.EmailSubject ?? ProductRejectNotification;
        var body = model.EmailBody ?? string.Format(RejectProductEmailBody, model.Name);

        var msg = MailHelper.CreateSingleEmail(from,to,subject, body, body);

        try
        {
            var response = await clien.SendEmailAsync(msg);

            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                var responseBody = await response.Body.ReadAsStringAsync();
                _loggerManager.LogError($"Failed to send rejection email to {model.EmailTo}. Response code: {response.StatusCode}");
                _loggerManager.LogError($"Error details: {responseBody}");
            }

            _loggerManager.LogInfo($"[{nameof(SendRejectProductEmailAsync)}] Successfully send email to farmer with email {model.EmailTo}");
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"Error while sending rejection email: {ex.Message}");
            throw;
        }
    }

    private void CheckProductNotFound(object product, Guid productId, string methodName)
    {
        if (product is null)
        {
            _loggerManager.LogError($"[{methodName}] Product with Id {productId} was not found at Date: {DateTime.UtcNow}");
            throw new ProductIdNotFoundException(productId);
        }
    }
}
