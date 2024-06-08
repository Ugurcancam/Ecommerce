using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Core.Dtos.Category;
using Ecommerce.Core.Entity;
using Ecommerce.Web.Models;

namespace Ecommerce.Web.Mapping
{
    public class MapProfileWeb : Profile
    {
        public MapProfileWeb()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<AltCategory, AltCategoryViewModel>().ReverseMap();
            CreateMap<Category, CategoryWithProductsDto>().ReverseMap();
        }
    }
}