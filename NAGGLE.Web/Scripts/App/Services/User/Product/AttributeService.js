




    app.factory('attributeService', ['$http', '$q', function ($http, $q)
    {
        var attributeServiceFactory = {};
    
        var _getAttributes = function ()
        {

            var deferred = $q.defer();
            $http.get('/User/GetAttributeList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        }
        var _getAttributeById = function (attributeId) {

            var deferred = $q.defer();
            $http.get('/User/GetAttributeById?eAttributeId='+attributeId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        }
        var _addEditAttribute = function (model) {

            var deferred = $q.defer();
            $http.post('/User/AddEditAttribute', model).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        }
        var _deleteSingleAttribute = function (attributeId) {

            var deferred = $q.defer();
            $http.get('/User/DeleteSingleAttribute?eAttributeId=' + attributeId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        }
        var _deleteMultipleAttribute = function (model) {

            var deferred = $q.defer();
            $http.post('/User/DeleteSelectedAttribute', model).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        }
        var _updateStatus = function (model) {

            var deferred = $q.defer();
            $http.post('/User/UpdateAttributeStatus', model).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        }


        attributeServiceFactory.updateStatus = _updateStatus;
        attributeServiceFactory.deleteMultipleAttribute = _deleteMultipleAttribute;
        attributeServiceFactory.addEditAttribute = _addEditAttribute;
        attributeServiceFactory.deleteSingleAttribute = _deleteSingleAttribute;
        attributeServiceFactory.getAttributes = _getAttributes;
        attributeServiceFactory.getAttributeById = _getAttributeById;


        return attributeServiceFactory;
    }]);