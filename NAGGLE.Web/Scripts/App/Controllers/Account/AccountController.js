
'use strict';


app.controller('loginAccountController', ['$scope', 'accountService', 'bootboxModal', 'loader', function ($scope, accountService, bootboxModal,loader) {

    //$scope.IsUserLoginStatus = false;

    $scope.User = {};
    $scope.Login = {};

    $scope.IsAccount;

   

    
    // <summary>
    //Clear Login Form
    // </summary>
        $scope.resetLoginForm = function ()
    {
        $scope.User = {};
        $scope.Login = {};
     }
        
    /* Sign in to registered user  */
    $scope.signInUser = function ($event) {
        loader.show($event.currentTarget);
        accountService.loginuser($scope.Login).then(function (response) {
            if (response != null && response.Success == true) {

                if (response.Type != "Supplier")
                { location.href = "../User/Dashboard/"; }
                else {
                    location.href = "../User/SupplierDashBoard";
                }

                }                            
            else {
                bootboxModal.alert(response == null ? "Oops!! Something went wrong" : response.Message);
                loader.hide();
            }
               // loader.hide();
        },
        function (response) {
            loader.hide();
            bootboxModal.alert("Oops!! Something went wrong.");
        });
    }
    

    

}]);

app.controller('signUpController', ['$scope', 'accountService', 'bootboxModal','notification', 'loader', function ($scope, accountService, bootboxModal,notification, loader) {

    $scope.User = {};
    var isAccount = $("#IsAccount").val();
    var userId = $("#UserId").val();

    if (isAccount == "value") {
        $("#Account-Confirmation").modal('show');
    }
    $scope.User.UserId = userId;

    /* It's use for create a new user  */
    $scope.createUser = function ($event) {
        loader.show($event.currentTarget);
        accountService.createaccount($scope.User).then(function (response) {
            if (response != "" && response != null && response.Success == true) {
                location.href = "../User/Dashboard/";
                
            }
            else {
                bootboxModal.alert(response.Message == null ? "Oops!! Something went wrong" : response.Message);
            }

            loader.hide();
        },
        function (response) {
            loader.hide();
            bootboxModal.alert("Oops!! Something went wrong.");
        });
    }

    $scope.savePassword = function ($event) {
        loader.show($event.currentTarget);
        accountService.setPassword($scope.User).then(function (response) {
            if (response.success) {
                $('#Account-Confirmation').modal('hide');
                notification.success("Saved Password Successfully");
                $scope.AgainLogin = { UserName: response.User.UserName, Password: response.User.PasswordHash };
                if (response.User.Type != "Supplier") {
                    location.href = "../User/Dashboard/";
                }
                else {
                    location.href = "../User/SupplierDashBoard";
                }

                //logIn($scope.AgainLogin);
               
            }
            else {
                // bootboxModal.alert(response.message);
            }
            loader.hide();
        }, function (err) {
            loader.hide();
        })
    }

    var logIn = function (login)
    {
            accountService.loginuser(login).then(function (response) {
                if (response != null && response.Success == true) {
                    if (response.Type != "Supplier")
                    {
                        location.href = "../User/Dashboard/";
                    }
                    else {
                        location.href = "../User/SupplierDashBoard";
                    }
                }
                else {
                    bootboxModal.alert(response == null ? "Oops!! Something went wrong" : response.Message);
                    loader.hide();
                }
            },
            function (response) {
                loader.hide();
                bootboxModal.alert("Oops!! Something went wrong.");
            });
    }

   
}]);