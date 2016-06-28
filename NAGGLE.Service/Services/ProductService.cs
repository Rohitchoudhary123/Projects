using NAGGLE.Common.ServiceResponse;
using NAGGLE.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExpressMapper;
using NAGGLE.DAL.Interfaces;
using NAGGLE.DAL.Entities;
using NAGGLE.DAL.Repository;
using NAGGLE.Service.Interfaces;
using System.Data.Entity.Validation;
using NAGGLE.Common.Utility;


namespace NAGGLE.Service.Services
{


    public class ProductService : AbstractService, IProductService
    {

        public ProductService(IUnitofWork unitwork) :
            base(unitwork)
        {

        }




        #region Attribute

        public List<AttributeModel> GetAttributeList()
        {
            var attributeList = new List<AttributeModel>();
            var attributes = _unitOfWork.attributeRespository.Get(m => m.IsDeleted == false);

            Mapper.Map(attributes, attributeList);
            for (int i = 0; i < attributes.Count; i++)
            {
                Mapper.Map(attributes[i].AttributeCategory, attributeList[i].AttributeCategoryModel);
            }

            return attributeList;
        }

        /*Get Specific Attribute Detail By id*/
        public AttributeModel GetAttributeById(int attributeId)
        {
            var attributeModel = new AttributeModel();

            var attribute = _unitOfWork.attributeRespository.Get(m => m.AttributeId == attributeId).FirstOrDefault();

            Mapper.Map(attribute, attributeModel);

            Mapper.Map(attribute.AttributeCategory, attributeModel.AttributeCategoryModel);

            return attributeModel;
        }
      
        /*Save Or Update Attribute */
        public AttributeModel AddEditAttribute(AttributeModel attributeModel)
        {
            var attribute = new NAGGLE.DAL.Entities.Attribute();
            var attributeCategory= new List<AttributeCategory>();
            Mapper.Map(attributeModel, attribute);
            Mapper.Map(attributeModel.AttributeCategoryModel, attribute.AttributeCategory);

            var categoryInfo = _unitOfWork.attributeCategoryRespository.Get(m => m.AttributeId == attributeModel.AttributeId);

            if (attributeModel.AttributeId > 0)
            {
                attribute.UpdatedBy = GetLoginUserId();
                var deleteAttributecategory = categoryInfo.Where(m => !attribute.AttributeCategory.Any(y => y.CategoryId == m.CategoryId)).ToList();
                attributeCategory = attribute.AttributeCategory.Where(m => !categoryInfo.Any(y => y.CategoryId == m.CategoryId)).ToList();
                if (deleteAttributecategory.Count > 0)
                    _unitOfWork.attributeCategoryRespository.DeleteAll(deleteAttributecategory);
                if (attributeCategory.Count > 0)
                    _unitOfWork.attributeCategoryRespository.InsertAll(attributeCategory);

                //if (categoryInfo.Count > 0)
                //    _unitOfWork.attributeCategoryRespository.DeleteAll(categoryInfo);
                //categoryInfo = attribute.AttributeCategory.ToList();
                //_unitOfWork.attributeCategoryRespository.InsertAll(categoryInfo);
                _unitOfWork.attributeRespository.Update(attribute);

            }
            else
            {
                attribute.CreatedBy = GetLoginUserId();
                _unitOfWork.attributeRespository.Insert(attribute);
                // _unitOfWork.attributeCategoryRespository.InsertAll(categoryInfo);
            }
            _unitOfWork.Save();
            Mapper.Map(attribute, attributeModel);
            Mapper.Map(attribute.AttributeCategory, attributeModel.AttributeCategoryModel);
            return attributeModel;
        }
       
        /*Update Attribute Status */
        public bool DeleteMultipleAttribute(List<AttributeModel> attributeModelList)
        {
            var attributeList = new List<NAGGLE.DAL.Entities.Attribute>();
            // var attributeCategoryList = new List<AttributeCategory>();
            Mapper.Map(attributeModelList, attributeList);

            for (int i = 0; i < attributeModelList.Count; i++)
            {
                //int attributeId=attributeModelList[i].AttributeId;
                //  var deleteAttributeCategory = _unitOfWork.attributeCategoryRespository.Get(m => m.AttributeId == attributeId);
                Mapper.Map(attributeModelList[i].AttributeCategoryModel, attributeList[i].AttributeCategory);
                if (attributeModelList[i].AttributeCategoryModel.Count > 0)
                    _unitOfWork.attributeCategoryRespository.DeleteAll(attributeList[i].AttributeCategory.ToList());
            }
            attributeList.ForEach(m => m.AttributeCategory = null);// Isdeleted property is not exist in AttributeCategory
            _unitOfWork.attributeRespository.DeleteAll(attributeList);
            _unitOfWork.Save();


            return true;
        }

        public AttributeModel UpdateAttributeStatus(AttributeModel attributeModel)
        {
            var attribute = new NAGGLE.DAL.Entities.Attribute();
            Mapper.Map(attributeModel, attribute);
            _unitOfWork.attributeRespository.Update(attribute);
            _unitOfWork.Save();
            Mapper.Map(attribute, attributeModel);

            return attributeModel;

        }

        public List<FeatureTypeModel> AllFeatureTypes()
        {
            var featureModel = new List<FeatureTypeModel>();

            var featureTypes = _unitOfWork.featureTypeRespository.Get();



            Mapper.Map(featureTypes, featureModel);


            return featureModel;
        }
        #endregion

        #region Product

        public List<ProductModel> FilterProducts(FilterParameter searchParameter)
        {
            var productModel = new List<ProductModel>();

          //  int AccountTypeId = searchParameter.AccountTypeId.Decrypt();
            int CategoryId = searchParameter.CategoryId.Decrypt();
            //int AccountId = searchParameter.AccountId.Decrypt();


            var products = _unitOfWork.productRespository.GetAsync(m => m.IsDeleted == false 
                
                  && (CategoryId != 0 ? m.ProductCategory.Where(x => x.CategoryId == CategoryId).Count() > 0 : true)
                  && (searchParameter.IsPublic != null ? m.IsPublic== searchParameter.IsPublic: true)
                  && (searchParameter.SearchText != null ? (m.ProductName.Contains(searchParameter.SearchText) ||m.ProductCode.Contains(searchParameter.SearchText) || m.ProductTag.Contains(searchParameter.SearchText)) : true)).Result;// && (parameters.AccountId!=null)? m.AccountId==parameters.AccountId:true).Result;


            Mapper.Map(products, productModel);

            for (int i = 0; i < products.Count; i++)
            {
                Mapper.Map(products[i].ProductClass, productModel[i].ProductClassModel);
            }

            return productModel;
        }


        /*Get Product List*/
        public List<ProductModel> GetProductList()
        {
            var productModel = new List<ProductModel>();

            var products = _unitOfWork.productRespository.Get();

            Mapper.Map(products, productModel);

            for (int i = 0; i < products.Count; i++)
            {
                Mapper.Map(products[i].ProductClass, productModel[i].ProductClassModel);
            }

            return productModel;
        }

        public List<AttributeModel> GetAttributeByCategoryId(List<int> categoryIds)
        {
            var attributeModelList = new List<AttributeModel>();
          
            var attributeCategory= _unitOfWork.attributeCategoryRespository.Get(m => categoryIds.Contains(m.CategoryId)).ToList();

            var attrId = new List<int>();
            for (int i = 0; i < attributeCategory.Count; i++)
            {
                var attributeModel = new AttributeModel();
                var isSameAttribute=attrId.Contains(attributeCategory[i].Attribute.AttributeId);//Distinct AttributeId
                if (!isSameAttribute)
                {
                    attrId.Add(attributeCategory[i].Attribute.AttributeId);
                    
                    Mapper.Map(attributeCategory[i].Attribute, attributeModel);
                    attributeModelList.Add(attributeModel);
                }
            }
            //attributeModelList.DistinctBY
           // Mapper.Map(attribute.AttributeCategory, attributeModel.AttributeCategoryModel);

            return attributeModelList;
        }


        /*Get Specific product by Id*/
        public ProductModel GetProductById(int productId)
        {
            var productModel = new ProductModel();

            Product product = _unitOfWork.productRespository.Get(m => m.ProductId == productId).FirstOrDefault();

            Mapper.Map(product, productModel);
            Mapper.Map(product.ProductClass, productModel.ProductClassModel);
            Mapper.Map(product.ProductAccountPrice, productModel.ProductAccountPriceModel);
            for (int i = 0; i < product.ProductAccountPrice.Count; i++)
            {
                productModel.ProductAccountPriceModel[i].AccountModel = new AccountModel();
                Mapper.Map(product.ProductAccountPrice.ToList()[i].Account, productModel.ProductAccountPriceModel[i].AccountModel);
            }
            Mapper.Map(product.ProductAvailability, productModel.ProductAvailabilityModel);
            for (int i = 0; i < product.ProductAvailability.Count; i++)
            {
                productModel.ProductAvailabilityModel[i].AvailabilityModel = new AvailabilityModel();
                Mapper.Map(product.ProductAvailability.ToList()[i].Availability, productModel.ProductAvailabilityModel[i].AvailabilityModel);
            }

            Mapper.Map(product.ProductPricing, productModel.ProductPricingModel);

            Mapper.Map(product.ProductDivision, productModel.ProductDivisionModel);
            Mapper.Map(product.ProductCategory, productModel.ProductCategoryModel);
            Mapper.Map(product.ProductAttribute, productModel.ProductAttributeModel);
            return productModel;
        }
        /*Add New Product Detail*/
        public ServiceResponse SaveProduct(ProductModel productModel)
        {
            var product = new Product();
            var productAccountPrice = new ProductAccountPrice();
            var productAvailability = new ProductAvailability();
            var productPricing = new ProductPricing();
            Mapper.Map(productModel, product);
            Mapper.Map(productModel.ProductAccountPriceModel, product.ProductAccountPrice);
            Mapper.Map(productModel.ProductAvailabilityModel, product.ProductAvailability);
            Mapper.Map(productModel.ProductPricingModel, product.ProductPricing);
            Mapper.Map(productModel.ProductDivisionModel, product.ProductDivision);
            Mapper.Map(productModel.ProductCategoryModel, product.ProductCategory);
            Mapper.Map(productModel.ProductAttributeModel, product.ProductAttribute);

            product.CreatedBy = GetLoginUserId();
            product.ProductAccountPrice.ToList().ForEach(m => m.CreatedBy = GetLoginUserId());
            product.ProductPricing.ToList().ForEach(m => m.CreatedBy = GetLoginUserId());

            _unitOfWork.productRespository.Insert(product);
            _unitOfWork.Save();
            Mapper.Map(product, productModel);
            Mapper.Map(product.ProductClass, productModel.ProductClassModel);
            Mapper.Map(product.ProductAccountPrice, productModel.ProductAccountPriceModel);
            Mapper.Map(product.ProductAvailability, productModel.ProductAvailabilityModel);
            Mapper.Map(product.ProductPricing, productModel.ProductPricingModel);
            Mapper.Map(product.ProductDivision, productModel.ProductDivisionModel);
            Mapper.Map(product.ProductCategory, productModel.ProductCategoryModel);
            Mapper.Map(product.ProductAttribute, productModel.ProductAttributeModel);

            return new ServiceResponse { Success = true, Message = "Saved", Data = productModel };

        }
        /*Update Product Detail */
        public ServiceResponse UpdateProduct(ProductModel productModel)
        {
            var product = new Product();
            var productAccountPrice = new List<ProductAccountPrice>();
            var productAvailability = new List<ProductAvailability>();
            var productPricingInfo = new List<ProductPricing>();
            var productDivision = new List<ProductDivision>();
            var productCategory = new List<ProductCategory>();

            var productinfo = _unitOfWork.productRespository.Get(m => m.ProductId == productModel.ProductId).FirstOrDefault();
            var productDivisionInfo = _unitOfWork.productDivisionRespository.Get(m => m.ProductId == productModel.ProductId);
            var productCategoryInfo = _unitOfWork.productCategoryRespository.Get(m => m.ProductId == productModel.ProductId);
            var productAttributeInfo= _unitOfWork.productAttributeRespository.Get(m => m.ProductId == productModel.ProductId);
          //  var productAccountPriceInfo = _unitOfWork.productAccountPriceRespository.Get(m => m.ProductId == productModel.ProductId);
            var productAvailabilityInfo = _unitOfWork.productAvailabilityRespository.Get(m => m.ProductId == productModel.ProductId);
           
            Mapper.Map(productModel, productinfo);
            Mapper.Map(productModel.ProductAccountPriceModel, product.ProductAccountPrice);
            Mapper.Map(productModel.ProductAvailabilityModel, product.ProductAvailability);
            Mapper.Map(productModel.ProductPricingModel, product.ProductPricing);
            Mapper.Map(productModel.ProductDivisionModel, product.ProductDivision);
            Mapper.Map(productModel.ProductCategoryModel, product.ProductCategory);
            Mapper.Map(productModel.ProductAttributeModel, product.ProductAttribute);


            if (product.ProductPricing.Count > 0)
            {
                var deleteProduct = product.ProductPricing.Where(m => m.IsDeleted == true && m.ProductPricingId != 0).ToList();
                if (deleteProduct.Count > 0)
                    _unitOfWork.productPricingRespository.DeleteAll(deleteProduct);

                productPricingInfo = product.ProductPricing.Where(m => m.ProductPricingId == 0 && m.IsDeleted == false).ToList();
                if (productPricingInfo.Count > 0)
                {
                    product.ProductPricing.ToList().ForEach(m => m.UpdatedBy = GetLoginUserId());
                    _unitOfWork.productPricingRespository.InsertAll(productPricingInfo);
                }
            }



            if (product.ProductAccountPrice.Count > 0)
            {
                var deleteProduct = product.ProductAccountPrice.Where(m => m.IsDeleted == true && m.ProductAccountPriceId != 0).ToList();
                if (deleteProduct.Count > 0)
                    _unitOfWork.productAccountPriceRespository.DeleteAll(deleteProduct);

                productAccountPrice = product.ProductAccountPrice.Where(m => m.ProductAccountPriceId == 0 && m.IsDeleted == false).ToList();
                if (productAccountPrice.Count > 0)
                {
                    product.ProductPricing.ToList().ForEach(m => m.UpdatedBy = GetLoginUserId());
                    _unitOfWork.productAccountPriceRespository.InsertAll(productAccountPrice);
                }
            }
            
                var deleteProductAvail = productAvailabilityInfo.Where(m => !product.ProductAvailability.Any(y => y.AvailabilityId== m.AvailabilityId)).ToList();
                productAvailability = product.ProductAvailability.Where(m => !productAvailabilityInfo.Any(y => y.AvailabilityId == m.AvailabilityId)).ToList();
                if (deleteProductAvail.Count > 0)
                    _unitOfWork.productAvailabilityRespository.DeleteAll(deleteProductAvail);
                if (productAvailability.Count > 0)
                    _unitOfWork.productAvailabilityRespository.InsertAll(productAvailability);

                var deleteProductDivision = productDivisionInfo.Where(m => !product.ProductDivision.Any(y => y.DivisionId == m.DivisionId)).ToList();
                 productDivision = product.ProductDivision.Where(m => !productDivisionInfo.Any(y => y.DivisionId == m.DivisionId)).ToList();
                 if (deleteProductDivision.Count > 0)
                     _unitOfWork.productDivisionRespository.DeleteAll(deleteProductDivision);
                if (productDivision.Count > 0)
                    _unitOfWork.productDivisionRespository.InsertAll(productDivision);
           
                var deleteProductCategory= productCategoryInfo.Where(m => !product.ProductCategory.Any(y => y.CategoryId == m.CategoryId)).ToList();//Which records are not selected 
                productCategory = product.ProductCategory.Where(m => !productCategoryInfo.Any(y => y.CategoryId == m.CategoryId)).ToList();//which records are selected 
                if (deleteProductCategory.Count > 0)
                    _unitOfWork.productCategoryRespository.DeleteAll(deleteProductCategory);
                if (productCategory.Count > 0)
                    _unitOfWork.productCategoryRespository.InsertAll(productCategory);

                var deleteProductAttr= productAttributeInfo.Where(m => !product.ProductAttribute.Any(y => y.AttributeId== m.AttributeId)).ToList();//Which records are not selected 
                productAttributeInfo = product.ProductAttribute.Where(m => !productAttributeInfo.Any(y => y.AttributeId == m.AttributeId)).ToList();//which records are selected 
                if (deleteProductAttr.Count > 0)
                    _unitOfWork.productAttributeRespository.DeleteAll(deleteProductAttr);
                if (productAttributeInfo.Count > 0)
                    _unitOfWork.productAttributeRespository.InsertAll(productAttributeInfo);

            



            //if (productAccountPriceInfo.Count > 0)
            //{
            //    _unitOfWork.productAccountPriceRespository.DeleteAll(productAccountPriceInfo);
            //    productAccountPriceInfo = product.ProductAccountPrice.ToList();
            //    product.ProductAccountPrice.ToList().ForEach(m => m.UpdatedBy = GetLoginUserId());
            //    _unitOfWork.productAccountPriceRespository.InsertAll(productAccountPriceInfo);
            //}
            //if (productAvailabilityInfo.Count > 0)
            //{
            //    _unitOfWork.productAvailabilityRespository.DeleteAll(productAvailabilityInfo);
            //    productAvailabilityInfo = product.ProductAvailability.ToList();
            //    _unitOfWork.productAvailabilityRespository.InsertAll(productAvailabilityInfo);
            //}
            ////if (productDivisionInfo.Count > 0)
            ////    _unitOfWork.productDivisionRespository.DeleteAll(productDivisionInfo);
            ////productDivisionInfo = product.ProductDivision.ToList();
            ////product.ProductPricing.ToList().ForEach(m => m.UpdatedBy = GetLoginUserId());
            ////_unitOfWork.productDivisionRespository.InsertAll(productDivisionInfo);

            //var results = persons2.Where(m => !diffids.Contains(m.Id)).ToList()
            //var insertList1 = productPricingInfo.Where(m =>!product.ProductPricing.Contains(m.ProductId)).ToList();
            //productPricingInfo = insertList;

            //var rateShipmentList = productPricingInfo.Where(m => product.ProductPricing.Any(y => y.ProductId!= m.ProductId)).ToList();
            //var rateShipmentList1 = product.ProductPricing.Where(m => productPricingInfo.Any(y => y.ProductId != m.ProductId)).ToList();

            //if (productPricingInfo.Count > 0)
            //    _unitOfWork.productPricingRespository.DeleteAll(productPricingInfo);
            
            productinfo.UpdatedBy = GetLoginUserId();
            _unitOfWork.productRespository.Update(productinfo);
            _unitOfWork.Save();

            Mapper.Map(productinfo, productModel);

            Mapper.Map(product.ProductAccountPrice, productModel.ProductAccountPriceModel);
            Mapper.Map(product.ProductAvailability, productModel.ProductAvailabilityModel);
            Mapper.Map(product.ProductPricing, productModel.ProductPricingModel);
            Mapper.Map(product.ProductDivision, productModel.ProductDivisionModel);
            Mapper.Map(product.ProductCategory, productModel.ProductCategoryModel);
            Mapper.Map(product.ProductAttribute, productModel.ProductAttributeModel);
            return new ServiceResponse { Data = productModel, Message = "Update", Success = true };
        }

        public bool DeleteProduct(int productId)
        {
            var product = _unitOfWork.productRespository.Get(m => m.ProductId == productId);
            var productPricing = _unitOfWork.productPricingRespository.Get(m => m.ProductId == productId);
            var productDivision = _unitOfWork.productDivisionRespository.Get(m => m.ProductId == productId);
            var productAvailability = _unitOfWork.productAvailabilityRespository.Get(m => m.ProductId == productId);
            var productAccountPrice = _unitOfWork.productAccountPriceRespository.Get(m => m.ProductId == productId);
            var productCategory= _unitOfWork.productCategoryRespository.Get(m => m.ProductId == productId);

            if (productPricing.Count > 0)
                _unitOfWork.productPricingRespository.DeleteAll(productPricing);

            if (productDivision.Count > 0)
                _unitOfWork.productDivisionRespository.DeleteAll(productDivision);


            if (productAvailability.Count > 0)
                _unitOfWork.productAvailabilityRespository.DeleteAll(productAvailability);

            if (productAccountPrice.Count > 0)
                _unitOfWork.productAccountPriceRespository.DeleteAll(productAccountPrice);
           
            if (productCategory.Count > 0)
                _unitOfWork.productCategoryRespository.DeleteAll(productCategory);

            if (product.Count > 0)
                _unitOfWork.productRespository.Delete(productId);

            _unitOfWork.Save();
            return true;
        }


        public ProductModel UpdateProductStatus(ProductModel productModel)
        {
            var product = new Product();
            Mapper.Map(productModel, product);
            _unitOfWork.productRespository.Update(product);
            _unitOfWork.Save();
            Mapper.Map(product, productModel);

            return productModel;

        }

        public List<ProductClassModel> GetAllProductClasses()
        {
            var productClassModel = new List<ProductClassModel>();
            var productClassList = _unitOfWork.productClassRespository.Get();
            Mapper.Map(productClassList, productClassModel);
            return productClassModel;
        }

        /*Get Division List*/
        public List<DivisionModel> GetAllDivisions()
        {
            var divisionModel = new List<DivisionModel>();
            var divisionList = _unitOfWork.divisionRespository.Get();
            Mapper.Map(divisionList, divisionModel);
            return divisionModel;
        }
        public List<AvailabilityModel> GetAllAvailablities()
        {
            var availabilityModel = new List<AvailabilityModel>();
            var availablityList = _unitOfWork.availabilityRespository.Get();
            Mapper.Map(availablityList, availabilityModel);
            return availabilityModel;

        }

        #endregion


    }
}
