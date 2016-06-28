'use strict';

app.factory('accountService', ['$http', '$q', function ($http, $q)
{
    var accountServiceFactory = {};

    /*Get States By CountryId*/
    var _getStates = function (countryId) {
        var deferred = $q.defer();
        $http.get('/User/GetStatesById?CountryId=' + countryId).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }

    /* Get All Account */
    var _getAccounts = function ()
    {
        var deferred = $q.defer();
        $http.post('/User/GetAccountList').success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;        
    }

    var _filterAccounts= function (data) {
        var deferred = $q.defer();
        $http.post('/User/FilterAccounts', data).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }



    /* Save Account Detail  */
    var _saveAccountDetail = function (data) {
        var deferred = $q.defer();
        $http.post('/User/AddEditAccount', data).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }



    /*Get Specific Account by Id*/
    var _getAccountsById = function (accountId) {
        var deferred = $q.defer();
        $http.post('/User/GetAccountById?accountId=' + accountId).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }
    var _deleteAccount = function (accountId) {
        var deferred = $q.defer();
        $http.post('/User/DeleteAccount?accountId=' + accountId).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }
    accountServiceFactory.DeleteAccount = _deleteAccount;
    accountServiceFactory.filterAccounts = _filterAccounts;

    accountServiceFactory.getAccountsById = _getAccountsById;
    accountServiceFactory.getStatesByCountryId = _getStates;
    accountServiceFactory.getAccounts = _getAccounts;
    accountServiceFactory.saveAccountDetail = _saveAccountDetail;
    return accountServiceFactory;
}
]);