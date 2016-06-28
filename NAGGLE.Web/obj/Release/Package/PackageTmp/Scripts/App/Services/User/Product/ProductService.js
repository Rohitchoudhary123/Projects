

app.factory('productService', ['$http', '$q', function ($http, $q) {

    var productServiceFactory = {};

    var _updateStatus = function (model) {

        var deferred = $q.defer();
        $http.post('/User/UpdateProductStatus', model).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }
    var _getProducts = function ()
    {
        var defferred = $q.defer();
        $http.get('/User/GetProductList').success(function (response) {
            defferred.resolve(response);
        }).error(function (err,status) {
            defferred.reject(err);
        });
        return defferred.promise;
    }

    var _getProductById = function (productId) {
        var defferred = $q.defer();
        $http.get('/User/GetProductById?eProductId='+ productId).success(function (response) {
            defferred.resolve(response);
        }).error(function (err, status) {
            defferred.reject(err);
        });
        return defferred.promise;
    }

    var _addEditProduct = function (model) {
        var defferred = $q.defer();
        $http.post('/User/AddEditProduct',model).success(function (response) {
            defferred.resolve(response);
        }).error(function (err, status) {
            defferred.reject(err);
        });
        return defferred.promise;
    }
    var _deleteProduct = function (productId) {
        var defferred = $q.defer();
        $http.post('/User/DeleteProduct?eProductId=' + productId).success(function (response) {
            defferred.resolve(response);
        }).error(function (err, status) {
            defferred.reject(err);
        });
        return defferred.promise;
    }
    var _getAttributeByCategoryId= function (data) {
        var defferred = $q.defer();
        $http.post('/User/GetAttributeByCategoryId',data).success(function (response) {
            defferred.resolve(response);
        }).error(function (err, status) {
            defferred.reject(err);
        });
        return defferred.promise;
    }

    var _filterProducts = function (data) {
        var defferred = $q.defer();
        $http.post('/User/FilterProducts', data).success(function (response) {
            defferred.resolve(response);
        }).error(function (err, status) {
            defferred.reject(err);
        });
        return defferred.promise;
    }

    productServiceFactory.filterProducts = _filterProducts;

    productServiceFactory.getAttributeByCategoryId = _getAttributeByCategoryId;

    productServiceFactory.deleteProduct = _deleteProduct;

    productServiceFactory.getProductById = _getProductById;
    productServiceFactory.addEditProduct = _addEditProduct;


    productServiceFactory.getProducts= _getProducts;
    productServiceFactory.updateStatus = _updateStatus;

    return productServiceFactory;
}
]);