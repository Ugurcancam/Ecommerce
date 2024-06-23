using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Core.Dtos;
using Ecommerce.Core.Dtos.Product;
using Ecommerce.Core.Entity;
using Ecommerce.Core.Repositories;
using Ecommerce.Core.Services;
using Ecommerce.Core.UnitOfWorks;

namespace Ecommerce.Service.Services
{
    public class ProductService : Service<Product>, IProductService
    {
        private readonly IProductRepository _productRepository;

        private readonly IMapper _mapper;


        public ProductService(IGenericRepository<Product> genericRepository, IUnitOfWork unitOfWork, IProductRepository productRepository, IMapper mapper) : base(genericRepository, unitOfWork)
        {
            _productRepository = productRepository;
            _mapper = mapper;

        }

        public async Task<List<Product>> GetActiveProductsWithCategory()
        {
            return await _productRepository.GetActiveProductsWithCategory();
        }

        //For API


        public Task<List<Product>> GetSimilarProducts(int categoryId)
        {
            return _productRepository.GetSimilarProducts(categoryId);
        }

        //For MVC
        public async Task<(List<ProductWithCategoryDto> Products, int TotalCount)> GetProductsWithCategory(int pageNumber, int pageSize)
        {
            var products = await _productRepository.GetProductsWithCategory(pageNumber, pageSize);
            var totalCount = await _productRepository.GetTotalProductsCount();
            var productsDto = _mapper.Map<List<ProductWithCategoryDto>>(products);
            return (productsDto, totalCount);
        }

        public async Task<int> GetTotalProductsCount()
        {
            return await _productRepository.GetTotalProductsCount();
        }
    }
}