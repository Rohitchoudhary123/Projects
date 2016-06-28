

var BROADCAST_GET = "get";
var BROADCAST_EDIT = "edit";
var BROADCAST_DELETE = "delete";
var BROADCAST_CANCEL = "cancel";
var BROADCAST_ADDUPDATE = "addupdate";

var container = $("#contactContainer");

app.controller('contactController', ['$scope', '$filter', 'contactService', 'loader', 'bootboxModal', 'notification', function ($scope, $filter, contactService, loader, bootboxModal, notification) {
   
    $scope.Mode = { List: 1, AddEdit: 2 };
    $scope.CurrentMode = $scope.Mode.List;
    $scope.Contact = {};
    $scope.Countries = [];
    $scope.States = [];
    $scope.Contacts = [];
    $scope.SearchParam = {};
    $scope.SelectAccountId = [];
    $scope.edit = function (id) {
        $scope.$broadcast(BROADCAST_EDIT, id, function () {


            // function to be executed after successfull broadcast event excuted
        });
    }

    /// <summary>
    /// Method will catch the broadcast and edit the contact 
    /// </summary>
    /// <param name="event">event of broadcast</param>  
    /// <param name="id">contact id</param>  
    /// <param name="callBack">function to be exceuted</param>  
    $scope.$on(BROADCAST_EDIT, function (event, id, callBack) {
        contactService.getContactById(id).then(function (response) {
            $scope.Contact = response;
            $scope.Contact.StateId = response.StateId!=null?response.StateId.toString():null;
            $scope.CurrentMode = $scope.Mode.AddEdit;
            if (callBack)
                callBack();
        });
    });



    $scope.delete = function (id, index) {
        $scope.$broadcast(BROADCAST_DELETE, id, index, function () {
            // function to be executed after successfull broadcast event excuted
        });
    }

    $scope.getAll = function () {
        $scope.$broadcast(BROADCAST_GET);
    }

    $scope.$on(BROADCAST_DELETE, function (event, id, index, callBack) {
        bootbox.confirm("Are you sure  you want to remove this contact ?", function (result) {
            if (result) {
                contactService.DeleteContact(id).then(function (response) {
                    $scope.Contacts.splice(index, 1);
                    notification.success("Deleted Successfully.");
                });
            }
        });
    });


    $scope.cancel = function () {
        $scope.$broadcast(BROADCAST_CANCEL, function () {
            // function to be executed after successfull broadcast event excuted
        });
    }

    $scope.$on(BROADCAST_CANCEL, function (event, callBack) {
        if (callBack)
            callBack();
        $scope.CurrentMode = $scope.Mode.List;
    });

    $scope.filterContacts = function () {
        getContacts($scope.SearchParam);
    }

    var getContacts = function (searchParam) {
        contactService.getContactList(searchParam).then(function (response) {
            $scope.Countries = response.Countries;
            $scope.States = response.States;
            $scope.Contacts = response.Contacts;
            $scope.Accounts = response.Accounts;
            loader.hidePreLoader();
            container.show();
        });
    }
    //this click  add contact button in Account page 
    $scope.addContact = function () {
        $scope.Contact = {}
        $("#add-contact").modal("show");
    }

    $scope.addNewContact = function () {
        $scope.CurrentMode = $scope.Mode.AddEdit;
        $scope.Contact = {};
    }
    $scope.save = function (contactDetail, $event) {
        loader.show($event.currentTarget);
        $scope.$broadcast(BROADCAST_ADDUPDATE, contactDetail, function () {
            // function to be executed after successfull broadcast event excuted
        });

    }
    $scope.$on(BROADCAST_ADDUPDATE, function (event, contactDetail, callBack) {
     
        contactService.addContactDetail(contactDetail).then(function (response) {
            if (response.Success) {
                if (contactDetail.Id == undefined) {
                    var selectedAccount = $filter('filter')($scope.Accounts, { AccountId: response.Data.AccountId }, true)[0];
                    //var selectedState = $filter('filter')($scope.States, { StateId: response.Data.StateId!=null?response.Data.StateId.toString():null }, true)[0];
                    response.Data.BusinessName = selectedAccount.BusinessName;
                    //response.Data.StateName = response.Data.StateId != null ? selectedState.Name : null;
                    response.Data.Id = response.Data.Id;
                    $scope.Contacts.push(response.Data);
                    notification.success("Added Successfully.");
                } else {
                    var contact = $filter('filter')($scope.Contacts, { Id: response.Data.Id }, true)[0];
                    var selectedAccount1 = $filter('filter')($scope.Accounts, { AccountId: response.Data.AccountId }, true)[0];
                    //contact.AccountId = response.Data.AccountId;
                    contact.BusinessName = selectedAccount1.BusinessName;
                    contact.Id = response.Data.Id.toString();
                    contact.Email = response.Data.Email;
                    contact.FirstName = response.Data.FirstName;
                    //contact.Telephone = response.Data.Telephone;
                   // contact.Designation = response.Data.Designation;
                    //contact.City = response.Data.City;
                   // contact.StateId = response.Data.StateId!=null?response.Data.StateId.toString():null;
                   // contact.StateName = response.Data.StateName;
                    // contact.
                    notification.success("Updated Successfully.");
                }
                $scope.CurrentMode = $scope.Mode.List;
                if (callBack)
                    callBack();
                loader.hide();
            } else {
                bootboxModal.alert(response.Message);
                loader.hide();
            };


        });
    });

    $scope.getStates = function (CountryType) {
        if (CountryType != "") {
            if (CountryType.Code != 'US') {
                $scope.Contact.StateId = null;
                $scope.Contact.State = null;
            }
        }
    }

    $scope.filterContacts = function (accountId) {
        $scope.SearchParam.AccountId = accountId != null ? accountId : null;
        contactService.filterContacts($scope.SearchParam).then(function (response) {
            $scope.Contacts = [];
            $scope.Contacts = response;
        });
    }

    $scope.$on(BROADCAST_GET, function (event, data) {
        if (data) {
            $scope.Countries = data.Countries;
            $scope.States = data.States;
            $scope.Contacts = data.Contacts;
        }
        else {
            loader.preLoader();
            getContacts();
        }
    });

}
]);


