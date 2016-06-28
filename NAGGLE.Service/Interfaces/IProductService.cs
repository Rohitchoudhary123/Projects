using NAGGLE.Common.ServiceResponse;
using NAGGLE.Common.Utility;
using NAGGLE.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Service.Interfaces
{
    public interface IProductService
    {

       

        #region Attribute

        List<AttributeModel> GetAttributeList();
        AttributeModel GetAttributeById(int attributeId);
        AttributeModel AddEditAttribute(AttributeModel attributeModel);
       // bool DeleteAttribute(int attributeId);
        bool DeleteMultipleAttribute(List<AttributeModel> attributeModelList);
        AttributeModel UpdateAttributeStatus(AttributeModel attributeModel);
        List<FeatureTypeModel> AllFeatureTypes();

        #endregion
     
        
        #region Product
        List<ProductModel> FilterProducts(FilterParameter FilterParameter);
        List<ProductModel> GetProductList();
        List<AttributeModel> GetAttributeByCategoryId(List<int> categoryIds);
        ProductModel GetProductById(int productId);
        ServiceResponse SaveProduct(ProductModel productModel);
        ServiceResponse UpdateProduct(ProductModel productModel);
        bool DeleteProduct(int productId);
        ProductModel UpdateProductStatus(ProductModel productModel);
        List<DivisionModel> GetAllDivisions();
        List<AvailabilityModel> GetAllAvailablities();
        List<ProductClassModel> GetAllProductClasses();
        #endregion

    }
}
