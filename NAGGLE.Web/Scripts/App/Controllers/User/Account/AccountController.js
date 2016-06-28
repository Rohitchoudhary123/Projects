var assignCategorieTree = $("#AssignCategorieTree");
var accountContainer = $("#mainContainer");

app.controller('userAccountController', ['$scope', '$rootScope', '$interval', '$filter', 'accountService', 'bootboxModal', 'notification', 'loader', function ($scope, $rootScope, $interval, $filter, accountService, bootboxModal, notification, loader) {

    $scope.Account = {};
    $scope.Account.AccounType = {};
    $scope.Account.User = {};//User For UserModel
    $scope.Categories = [];
    $scope.Countries = [];
    $scope.States = [];
    $scope.Accounts = [];
    $scope.AccountList = []; //Get all accounts
    $scope.AccountTypes = [];
    $scope.searchParam = {};
    $scope.IsReadOnlyEmail = false;
    $scope.FilterAccountId = [];
    $scope.FilterCategoryId = [];
    $scope.FilterAccountTypeId = [];
    $scope.SearchText = [];
    $scope.SelectedAssignCategorieName = [];
    $scope.Mode = { List: 1, View: 2, AddEdit: 3 };
    $scope.CurrentMode = $scope.Mode.List;


    var contactController = angular.element('[ng-controller=contactController]').scope()

    /*Active Side Menu functionality on User dashboard*/

    /* Get All Accounts  */
    var getAccounts = function () {
        loader.preLoader();
        accountService.getAccounts().then(function (response) {
            /*These are master table*/
            $scope.Categories = response.Categories;
            $scope.AccountTypes = response.AccountTypes;
            $scope.Countries = response.Countries;
            $scope.States = response.States;
            $scope.Accounts = response.Accounts;
            $scope.AccountList = response.Accounts;
            loader.hidePreLoader();
            accountContainer.show();
        });
    }

    /*filter Accounts by AccountType*/
    $scope.accountFilter = function () {
        $scope.searchParam.AccountTypeId = $scope.FilterAccountTypeId;
        $scope.searchParam.CategoryId = $scope.FilterCategoryId;
        $scope.searchParam.AccountId = $scope.FilterAccountId;
        $scope.searchParam.SearchText = $scope.SearchText;
        if ($scope.FilterAccountTypeId != "" || $scope.FilterCategoryId != "" || $scope.FilterAccountId != "" || $scope.SearchText != "") {
            accountService.filterAccounts($scope.searchParam).then(function (response) {
                if (response.Success) {
                    $scope.Accounts = response.Accounts;
                }
            });
        }
        else {
            getAccounts();

        }
    }


    /* Get States by country id */
    $scope.getStates = function (CountryType) {
        if (CountryType != "" && CountryType != null) {
            if (CountryType.Code != 'US') {
                $scope.Account.StateId = null;
                $scope.Account.State = null;
            }
        }
    }

    $scope.viewAccount = function (account) {
        accountService.getAccountsById(account.AccountId).then(function (response) {
            if (response.Success) {
                /*Filter Selected state by stateid*/
                var selectedState = $filter('filter')($scope.States, { StateId: response.Account.StateId }, true)[0];//it's use for showing State Name
                response.Account.StateName = selectedState != undefined ? selectedState.Name : null;
                /*take two variables,which are showing categories and accounTypes name */
                var accountTypeName = "";
                var accountCategoryName = "";

                /*Getting Selected AccountTypes name and set checked prop true*/
                $scope.SelectedAccountTypes = [];
                angular.forEach(response.Account.AccountAccountTypes, function (item, key) {
                    $scope.SelectedAccountTypes.push(item.AccountTypeId);
                    accountTypeName += item.Name + ", ";
                });
                /*Getting Selected Categories name and set checked prop true*/
                $scope.SelectedAssignCategories = [];
                $scope.SelectedAssignCategorieName = [];
                angular.forEach(response.Account.AccountCategories, function (item, key) {
                    $scope.SelectedAssignCategories.push({ Id: item.CategoryId, Name: item.Name });
                    accountCategoryName += item.Name + ", ";
                });
                /*Remove last Character(,)*/
                response.Account.AccountTypeNames = accountTypeName.slice(0, -2);
                response.Account.AccountCategoryNames = accountCategoryName.slice(0, -2);
                $scope.Account = response.Account;
                $scope.Account.StateId = response.Account.StateId;
                angular.forEach(response.Contacts, function (value, key) {
                    value.State = $filter('filter')($scope.States, { StateId: value.StateId }, true)[0];//it's use for showing State Name
                });

                $rootScope.$broadcast(BROADCAST_GET, { Contacts: response.Contacts, Countries: $scope.Countries, States: $scope.States });

                $scope.CurrentMode = $scope.Mode.View;
            } else {
                bootboxModal.alert(response.Message)
            }
        });
    }
    /*Add and Update Account Detail*/
    $scope.addEditAccount = function (accountInfo, $event) {
        loader.show($event.currentTarget);
        accountInfo.CountryId = accountInfo.CountryType != null ? accountInfo.CountryType.CountryId : null;

        //accountInfo.AccountAccountTypes = $filter('filter')($scope.AccountTypes, { checked: true });
        var AccountTypes = [];
        angular.forEach($scope.SelectedAccountTypes, function (item, key) {
            AccountTypes.push({ AccountTypeId: item })
        });
        accountInfo.AccountAccountTypes = AccountTypes;
        //accountInfo.AccountCategories = $filter('filter')($scope.Categories, { checked: true });
        var AssignCategories = [];
        angular.forEach($scope.SelectedAssignCategories, function (item, key) {
            AssignCategories.push({ CategoryId: item })
        });
        accountInfo.AccountCategories = AssignCategories;
        accountService.saveAccountDetail(accountInfo).then(function (response) {
            if (response.Success) {
                if (response.Message == "Saved") {
                    // $scope.Accounts.push(response.Data);
                    notification.success("Saved Successfully.");
                }
                else {
                    var accounts = $filter('filter')($scope.Accounts, { AccountId: response.Data.AccountId }, true)[0];
                    accounts.BusinessName = response.Data.BusinessName;
                    accounts.Email = response.Data.Email;
                    notification.success("Updated Successfully.");
                }
                accountContainer.hide();
                getAccounts();
                $scope.FilterAccountId = [];
                $scope.FilterCategoryId = [];
                $scope.FilterAccountTypeId = [];
                $scope.SearchText = [];
                $scope.CurrentMode = $scope.Mode.List;
            }
            else { bootboxModal.alert(response.Message); }
            loader.hide();
        }, function (response) { loader.hide(); });
    }

    /*Delete Account by accountId*/
    $scope.deleteAccount = function (accountId, index) {
        bootbox.confirm("Are you sure  you want to remove this account ?", function (result) {
            if (result) {
                accountService.DeleteAccount(accountId).then(function (response) {
                    $scope.Accounts.splice(index, 1);
                    notification.success("Deleted Successfully.");
                });
            }
        });
    }

    $scope.editContact = function (id) {

        $scope.$broadcast("edit", id, function () {
            // function to be executed after successfull broadcast event excuted
            $("#add-contact").modal("show");
        });
    }

    $scope.Cancel = function () {
        $scope.CurrentMode = 1;
    }

    $scope.saveContact = function (contact) {
        contact.AccountId = $scope.Account.AccountId;
        $scope.$broadcast("addupdate", contact, function () {
            // function to be executed after successfull broadcast event excuted
            $("#add-contact").modal("hide");
        });
    }
    $scope.deleteContact = function (id, index) {
        $scope.$broadcast("delete", id, index, function () {
            // function to be executed after successfull broadcast event excuted
            $("#add-contact").modal("hide");
        });
    }
    /* Show Add New Account Form */
    $scope.addNewAccount = function () {
        $scope.ShowSaveButton = true;
        $scope.IsReadOnlyEmail = false;
        $scope.CurrentMode = 3;
        $scope.Account = {};
        $scope.SelectedAccountTypes = [];
        $.each($scope.Categories, function (index, value) {
            if (value.checked)
                value.checked = false;
        });
        $.each($scope.AccountTypes, function (index, value) {
            if (value.checked)
                value.checked = false;
        });
    }
  
    /*Edit Account*/
    $scope.editAccount = function (account) {
        $scope.ShowSaveButton = false;
        $scope.IsReadOnlyEmail = true;
        $scope.Account = account;
        $scope.CurrentMode = 3;
    }

    //$scope.deleteCategoryName = function (categoryId) {
    //    var accountCategory = $filter('filter')($scope.Categories, { CategoryId: categoryId }, true)[0];
    //    accountCategory.checked = false;
    //}

    //$scope.accounTypeName = function (accountTypeId) {
    //    var accountType = $filter('filter')($scope.AccountTypes, { AccountTypeId: accountTypeId }, true)[0];
    //    accountType.checked = false;
    //}

    getAccounts();
}]);;