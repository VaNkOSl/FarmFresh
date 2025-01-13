using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
using FarmFresh.Services.Helpers;
using FarmFresh.ViewModels.Review;
using LoggerService.Contacts;
using LoggerService.Exceptions.InternalError.Review;
using Microsoft.EntityFrameworkCore;
using FarmFresh.Repositories.Extensions;
using FarmFresh.Data.Models.Enums;

namespace FarmFresh.Services;

internal sealed class ReviewService : IReviewService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    public ReviewService(IRepositoryManager repositoryManager,
                        ILoggerManager loggerManager,
                        IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _mapper = mapper;
    }

    public async Task CreateProductReviewAsync(ProductReviewCreateDto model, Guid userId, bool trackChanges)
    {
        var user = await 
            _repositoryManager
            .UserRepository
            .FindUsersByConditionAsync(u => u.Id == userId, trackChanges)
            .FirstOrDefaultAsync();

        var product = await
            _repositoryManager
            .ProductRepository
            .FindProductByConditionAsync(p => p.Id == model.ProductId, trackChanges)
            .FirstOrDefaultAsync();

        ProductHelper.CheckProductNotFound(product, product.Id, nameof(CreateProductReviewAsync), _loggerManager);
        AccountHelper.ChekIfUserIsNull(user, userId, nameof(CreateProductReviewAsync), _loggerManager);

        try
        {
            var productReview = _mapper.Map<Review>(model);
            productReview.ProductId = product.Id;
            productReview.UserId = userId;
            product.Reviews.Add(productReview);

            await _repositoryManager.ReviewRepository.CreateReviewAsync(productReview);
            await _repositoryManager.SaveAsync(productReview);
            _loggerManager.LogInfo($"[{nameof(CreateProductReviewAsync)}] User with Id {userId} successfully created review for product with ID {product.Id} and name {product.Name}");
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"An unexpected error occurred: {ex.Message}");
            throw new CreateReviewException();
        }
    }

    public async Task<IEnumerable<AllReviewDto>> GetAllReviewsAsync(Guid userId, bool trackChanges)
    {
        var orders = await _repositoryManager.OrderRepository
         .FindOrderByConditionAsync(r => r.UserId == userId, trackChanges)
         .Where(o => o.OrderStatus == OrderStatus.ReadyForPickup)
         .GetAllProductsForReviewByUserAsync()
         .ToListAsync();

        var productsWithoutReview = orders
              .SelectMany(o => o.OrderProducts)
              .Select(op => op.Product)
              .Where(p => !p.Reviews.Any(r => r.UserId == userId))
              .ToList();

        var reviewDtos = _mapper.Map<List<AllReviewDto>>(productsWithoutReview);
        return reviewDtos;

    }

    public async Task<IEnumerable<ProductReviewDto>> GetReviewedProductsAsync(Guid userId, bool trackChanges)
    {
        var review = await _repositoryManager.ReviewRepository
            .FindReviewByConditionAsync(r => r.UserId == userId, trackChanges)
            .GetProductReviewWithDetailsAsync()
            .ToListAsync();

        var reviewDtos = _mapper.Map<List<ProductReviewDto>>(review);

        return reviewDtos;
    }
}
