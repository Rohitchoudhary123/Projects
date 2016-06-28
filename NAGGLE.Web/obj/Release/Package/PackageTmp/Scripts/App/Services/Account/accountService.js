
'use strict';

app.factory('accountService', ['$http', '$q', function ($http, $q) {
    var accountServiceFactory = {};

    var _createaccount = function (modeldata) {
        
        var deferred = $q.defer();
        $http.post('/Account/SignUp', modeldata).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }

    var _loginuser = function (logindata) {
        
        var deferred = $q.defer();
        $http.post('/Account/SignIn', logindata).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }

    var _setPassword = function (data) {
        var deferred = $q.defer();
        $http.post('/Account/SetPassword', data).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }
    
    accountServiceFactory.setPassword = _setPassword;
    accountServiceFactory.createaccount = _createaccount;
    accountServiceFactory.loginuser = _loginuser;
    return accountServiceFactory;
}]);