
var productContainer = $("#mainContainer");
app.controller('productController', ['$scope', '$filter', 'productService', 'bootboxModal', 'loader', 'notification', function ($scope, $filter, productService, bootboxModal, loader, notification) {

    $scope.Products = [];
    $scope.Categories = [];
    $scope.Divisions = [];
    $scope.Availabilities = [];
    $scope.ProductClasses = [];
    $scope.Accounts = [];
    $scope.IsCheckedAttribute = false;
    $scope.Product = {};
    // $scope.Product.ProductPricing = [];
    $scope.SelectedAssignCategorieName = [];

    $scope.FilterIsPublic = "";
   $scope.SearchText = "";
    $scope.FilterCategoryId = "";
    $scope.searchParameter = {};

    $scope.Mode = { List: 1, AddEdit: 2 };
    $scope.CurrentMode = $scope.Mode.List;

    $scope.StatusList = [{
        Id: true,
        name: 'Active'
    }, {
        Id: false,
        name: 'Disabled'
    }];
    $scope.Stock = [{
        Id: 1,
        name: 'In Stock'
    }, {
        Id: 2,
        name: 'Out of Stock'
    }];
    $scope.Defines = [{
        Id: true,
        name: 'Defined'
    }, {
        Id: false,
        name: 'PreDefined'
    }];

    /*Get Products*/
    var getProducts = function () {
        loader.preLoader();
        productService.getProducts().then(function (response) {

            if (response.Success) {
                $scope.Products = response.Products;
                $scope.Categories = response.Categories;
                $scope.Divisions = response.Divisions;
                $scope.Availabilities = response.Availabilities;
                $scope.ProductClasses = response.ProductClasses;
                $scope.Accounts = response.Accounts;
            }
            loader.hidePreLoader();
            productContainer.show();

        });
    }

    $scope.$watch("SelectedAssignCategorieName", function (newValues, oldValues) {
        $scope.case = 1;

        if (newValues != undefined && newValues.length > 0 && newValues.length != oldValues.length) {

            productService.getAttributeByCategoryId(newValues).then(function (response) {
                
                $scope.CategoryAttributes = response.CategoryAttributes;

                /*Checked Attribute Click on Edit Product*/
                
                if ($scope.SelectedAttribute.length > 0 )
                {
                    $.each(response.CategoryAttributes, function (index, value) {
                        var selectedAttribute = ($filter)('filter')($scope.SelectedAttribute, { AttributeId: value.AttributeId }, true);
                        if (selectedAttribute.length>0)
                            value.checked = true;
                    });
                    $scope.SelectedAttribute = [];
                   
                }
                $scope.case = 2;
                
            });
        } else if (newValues.length != oldValues.length) { $scope.CategoryAttributes = []; } //If Not select any category 
    });


    /*Get Specific Product By Id*/

    $scope.getProductById = function (productId) {

        productService.getProductById(productId).then(function (response) {


            $scope.SelectedDivisions = [];
            angular.forEach(response.Product.ProductDivisions, function (item, key) {
                $scope.SelectedDivisions.push(item.DivisionId);
            });

            $scope.SelectedAssignCategories = [];
            $scope.SelectedAssignCategorieName = [];
            angular.forEach(response.Product.ProductCategories, function (item, key) {
                $scope.SelectedAssignCategories.push({ Id: item.CategoryId, Name: item.Name });
            });

           
            $scope.SelectedAttribute=response.Product.ProductAttributes;

            //$scope.IsCheckedAttribute = true;
            debugger;
            $scope.CurrentMode = $scope.Mode.AddEdit;
            $scope.Product = response.Product;
            


        });
    }

    /*Add Edit Product*/
    $scope.addEditProduct = function (product, $event) {
        loader.show($event.currentTarget);

        var ProductDivisions = [];
        angular.forEach($scope.SelectedDivisions, function (item, key) {
            ProductDivisions.push({ DivisionId: item, ProductId: product.ProductId })
        });
        $scope.Product.ProductDivisions = ProductDivisions;

        var AssignCategories = [];
        angular.forEach($scope.SelectedAssignCategories, function (item, key) {
            AssignCategories.push({ CategoryId: item, ProductId: product.ProductId })
        });
        $scope.Product.ProductCategories = AssignCategories;


        $scope.SelectedAttribute = ($filter)('filter')($scope.CategoryAttributes,{checked:true },true);
        var ProductAttributes = [];
        angular.forEach($scope.SelectedAttribute, function (item, key) {
            ProductAttributes.push({ AttributeId: item.AttributeId, ProductId: product.ProductId })
        });
        $scope.Product.ProductAttributes = ProductAttributes;


        productService.addEditProduct(product).then(function (response) {
            var updateClass = $filter('filter')($scope.ProductClasses, { ProductClassId: response.Product.ProductClassId }, true)[0];
            if (response.Message == "Saved") {
                response.Product.ProductClassName = updateClass.Name;
                $scope.Products.push(response.Product);
                notification.success("Saved Successfully")
            } else {

                var updateProduct = $filter('filter')($scope.Products, { ProductId: response.Product.ProductId }, true)[0];
                updateProduct.ProductName = response.Product.ProductName;
                updateProduct.ProductClassName = updateClass.Name;
                updateProduct.Status = response.Product.Status;
                updateProduct.ProductCode = response.Product.ProductCode;
                notification.success("Updated Successfully");
            }

            $scope.CurrentMode = $scope.Mode.List;
            loader.hide();
        }, function (err, status) { loader.hide(); });

    }

    /*InsertButton in Pricing*/
    $scope.insertPricing = function (productPricing) {
        productPricing.IsDeleted = false;

        $scope.Product.ProductPrices.push(productPricing);
        $scope.ProductPricing = {};
    }
    $scope.insertAvailability = function (productAvailability) {
        var IsExist = $filter('filter')($scope.Product.ProductAvailabilities, { AvailabilityId: productAvailability.AvailabilityId,IsDeleted:false }, true);
        if (IsExist.length == 0) {
            productAvailability.IsDeleted = false;
            $scope.Product.ProductAvailabilities.push(productAvailability);
            $scope.ProductAvailabity = null;
        } else {
            bootboxModal.alert("It's Already Exist");
        }
        
    }
    $scope.insertAccountPrice = function (productAccountPrice) {
        productAccountPrice.IsDeleted = false;
        productAccountPrice.AccountId = productAccountPrice.Account.AccountId;
        $scope.Product.ProductAccountPrices.push(productAccountPrice);
        $scope.ProductAccountPrice = {};
    }

    /*Delete pricing*/
    $scope.deletePricing = function (product, index) {

        product.IsDeleted = true;


        //$scope.Product.ProductPrices[index].IsDeleted = true;

        // $scope.Product.ProductPrices.splice(index, 1);
        //$scope.ProductPricing = {};
    }
    $scope.deleteAvailability = function (availability, index) {
        availability.IsDeleted = true;
         //$scope.Product.ProductAvailabilities.splice(index, 1);
    }
    $scope.deleteAccountPrice = function (accountPrice, index) {
        accountPrice.IsDeleted = true;
        //$scope.Product.ProductAccountPrices.splice(index, 1);
    }
    //$('#testt').attr('name', 'value');

    $scope.deleteProduct = function (productId, index) {

        bootboxModal.confirm("Are you sure  you want to remove this product ?", function (result) {
            if (result) {
                productService.deleteProduct(productId).then(function (response) {
                    $scope.Products.splice(index, 1);
                    notification.success("Deleted Successfully.");
                });
            }
        });

    }


    $scope.productFilter = function ()
    {
        $scope.searchParameter.IsPublic = $scope.FilterIsPublic;
        $scope.searchParameter.CategoryId = $scope.FilterCategoryId;
        //$scope.searchParam.AccountId = $scope.FilterAccountId;
        $scope.searchParameter.SearchText = $scope.SearchText;

        if ( $scope.FilterCategoryId != "" || $scope.IsPublic != "" || $scope.SearchText != "")
        {
            productService.filterProducts($scope.searchParameter).then(function (response) {
                $scope.Products = response.Products;
            });
        }
        else {
            getProducts();
        }
    }


    $scope.addNewProduct = function () {

        $("#btnProductAvailability").click();
        $("#btnProductPricing").click();
        $("#btnAccountPrice").click();


        $scope.Product = {};
        $scope.Product.ProductPrices = [];
        $scope.Product.ProductAvailabilities = [];
        $scope.Product.ProductAccountPrices = [];

        $scope.Product.ProductDivisions = [];
        $scope.SelectedDivisions = [];

        $scope.Product.ProductCategories = [];
        $scope.SelectedAssignCategories = [];

        $scope.Product.ProductAttributes = [];

     //   $scope.CategoryAttributes = [];//WHEN We are click on add new product then 

        allCheckedFalse();
        $scope.CurrentMode = $scope.Mode.AddEdit;
    }



    $scope.editProduct = function (product) {
        $scope.Product = product;
        $scope.CurrentMode = $scope.Mode.AddEdit;
    }

    $scope.cancelProduct = function () {
        $scope.CurrentMode = $scope.Mode.List;
    }



    /*Update Status on AttributeList Page*/
    $scope.updateStatus = function (product) {
        productService.updateStatus(product).then(function (response) {
            var updateProduct = $filter('filter')($scope.Products, { ProductId: response.ProductId }, true)[0];
            updateProduct.Status = response.Status;
            notification.success("Status Updated Successfully.");

        });
    }
    var allCheckedFalse = function () {
        $.each($scope.Categories, function (index, value) {
            if (value.checked)
                value.checked = false;
        });
        $.each($scope.Divisions, function (index, value) {
            if (value.checked)
                value.checked = false;
        });
    }
    getProducts();

}


]);