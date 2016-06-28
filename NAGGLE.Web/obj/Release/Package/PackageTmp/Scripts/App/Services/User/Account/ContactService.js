app.factory('contactService', ['$http', '$q', function ($http, $q) {

    var contactFactory = {};

    /*Get All Contacts*/
    var _getContacts = function () {
        var deferred = $q.defer();
        $http.post('/User/GetContactList').success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }
    var _saveContact = function (data) {
        var deferred = $q.defer();
        $http.post('/User/AddEditContact', data).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }

    var _filterContacts = function (data) {
        var deferred = $q.defer();
        $http.post('/User/FilterContactsByAccount', data).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }
    var _getContactById = function (Id) {
        var deferred = $q.defer();
        $http.post('/User/GetContactById?Id='+ Id).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }
    var _deleteContact = function (contactId) {
        var deferred = $q.defer();
        $http.post('/User/DeleteContact?userId=' + contactId).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }
    contactFactory.DeleteContact = _deleteContact;
    contactFactory.getContactById = _getContactById;
    contactFactory.filterContacts = _filterContacts;
    contactFactory.addContactDetail = _saveContact;
    contactFactory.getContactList = _getContacts;

    return contactFactory;

}]);