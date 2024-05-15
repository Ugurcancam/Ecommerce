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

        public async Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory()
        {
            var products = await _productRepository.GetProductsWithCategory();
            var productsDto = _mapper.Map<List<ProductWithCategoryDto>>(products);
            return CustomResponseDto<List<ProductWithCategoryDto>>.Success(200,productsDto);
        }
    }
} 