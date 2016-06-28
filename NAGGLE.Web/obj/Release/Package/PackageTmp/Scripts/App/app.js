//Angular App
var app = angular.module('appNaggle', ['validation', 'validation.rule']); //, 'ngFlag'
//Disable ajax cache
app.config([
    '$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push('noCacheInterceptor');
    }
]).factory('noCacheInterceptor', function () {
    return {
        request: function (config) {
            if (config.method == 'GET') {
                var separator = config.url.indexOf('?') === -1 ? '?' : '&';
                config.url = config.url + separator + 'noCache=' + new Date().getTime();
            }
            return config;
        }
    };
});



/// <summary>
/// It's use to shown Validation message(Format) 
/// </summary>

app.config(['$validationProvider', function ($validationProvider) {
    $validationProvider.showSuccessMessage = false;
    $validationProvider.showErrorMessage = true;
    $validationProvider.setValidMethod('submit');
}]);
// Notification factory
app.factory('notification', function () {
    return {
        success: function (message) {
            new PNotify({
                title: "Success",
                text: message,
                delay: 1000,
                type: 'success'
            });
        },
        error: function (message) {
            new PNotify({
                title: "Error",
                text: message == undefined ? "Something went wrong." : message,
                delay: 1000,
                type: 'error'
            });
        },
        notice: function (title, message) {
            new PNotify({
                title: title,
                text: message,
                delay: 1000,
            });
        },
        info: function (title, message) {
            new PNotify({
                title: title,
                text: message,
                delay: 1000,
                type: 'info'
            });
        }
    }
});
app.config(['$validationProvider', function ($validationProvider) {

    $validationProvider
        .setExpression({
            confirmpassword: function (value, scope, element, attrs, param) {
                if (attrs.pwd != "" && attrs.pwd == value)
                    return true;
                else if (attrs.pwd != "")
                    return false;
                return true;
            },//it Made for Password Length atleast 6 characters
            passwordlength: function (value, scope, element, attrs, param) {
                if (value.length > 5)
                    return true;
                else
                    return false;
            },
            morethanzero: function (value, scope, element, attrs, param) {


                if (value > 0) {
                    return true;
                }
                else
                    return false;
            },
            specialsymbols: function (value, scope, element, attrs, param) {

                var regex = new RegExp("^[0-9.]+$");
                if (!regex.test(value)) {
                    //var dhg = value.contains('.');

                    return false;

                } else
                    return true;

            },
            objectrequired: function (value, scope, element, attrs, param) {      // This validation is for that controls, those are having object selected in ng-model. Such as for dropdown control.
                
                var obj = scope.$eval(attrs.ngModel);
                if (obj[attrs.validatecolumn] == undefined || obj[attrs.validatecolumn] == 0 || obj[attrs.validatecolumn] == "") {
                    return false;
                } else {
                    return true;
                }

            },           
            contactNo: function(value,scope,element,attrs,param)
            {
                if (value.length >= 10) {
                    return true;
                }
                else {
                    return false;
                }
            }


        })
        .setDefaultMsg({
            confirmpassword: {
                error: 'Password doesnot match'
            },
            passwordlength: {
                error: 'Password must be of minimium six character long'
            },
            morethanzero: {
                error: 'Not Valid',
            },
            specialsymbols: {
                error: 'only enter decimal value',
            },
            objectrequired: {
                error: 'This should be Required!!',
            },
            contactNo:
            {
                error: 'Contact no. must be of minimum ten digit long'
            }
        });

}]);

//Loader factory
app.factory('loader', function () {
    return {
        show: function (currentElement) {
            var l = Ladda.create(currentElement);
            l.start();
        },
        hide: function () {
            Ladda.stopAll();
        },
        preLoader: function () {
            $("#preloader").show();
        },
        hidePreLoader: function (container) {
            
            $("#preloader").hide();
            if (container) {
                container.show();
            }
        }
    };
});

//Showing Alert message in Modal
app.factory('bootboxModal', function () {
    return {
        alert: function (message, callback) {
            bootbox.alert(message, callback == undefined ? null : callback);
        },
        prompt: function (message, callback) {
            bootbox.prompt(message, callback == undefined ? null : callback)
        },
        confirm: function (message, callback) {
            bootbox.confirm(message, callback == undefined ? null : callback)
        }
    };
});

app.filter('unique', function () {
    return function (collection, keyname) {

        var output = [],
            keys = [];
        angular.forEach(collection, function (item) {
            var key = item[keyname];
            if (keys.indexOf(key) === -1) {
                keys.push(key);
                output.push(item);
            }
        });
        return output;
    };
});

//app.config(function (localStorageServiceProvider) {
//    localStorageServiceProvider
//      .setPrefix('appShipkee')
//      .setStorageType('localStorage')
//      .setNotify(true, true)
//});



//app.directive('invalidParent', function () {
//    return {
//        restrict: 'A',
//        link: function (scope, elem, attrs, ngModel) {

//            scope.$watch(function () { return elem[0].className }, function (newValue, oldValue) {

//                if (newValue != undefined && elem.val() == "" && newValue.toString().indexOf("ng-invalid") > 0)
//                    elem.parents(".double-form").addClass("invalid-input");
//                else
//                    elem.parents(".double-form").removeClass("invalid-input");
//            });

//            scope.$watch(function () { return elem.val() }, function (newValue, oldValue) {

//                if (newValue != undefined && elem.val() != "")
//                    elem.parents(".double-form").removeClass("invalid-input");
//            });
//        }
//    };
//});


/* Datepicker directive
Properties:
1. disable-previous-dates  -> if attr present then it will disable the previous dates i.e. below then current date
2. format -> if present, need to give formate of date i.e. MM-DD-YYYY or, LT or, any other date format. If this attribute is not present then MM-DD-YYYY format of date should be applied.
3. linked-picker-id -> to assign datepicker any id. (Used for linking two date pickers)
4. linked-to -> to link one datepicker to another i.e. date range picker. (Used for linking two date pickers)
5. set-min-max-hour -> to set min max hour for timepicker. Values must be provided in comma seprated. 

   Explanation to use Linked picker: FromPicker should have linked-picker-id attribute. ToPicker should have linked-to attribute.

   Example to use: 
   FromPicker: <bootstrap-datepicker ng-model="FromPicker" linked-picker-id="earliestTimeFrom"></bootstrap-datepicker>
   ToPicker: <bootstrap-datepicker ng-model="ToPicker" linked-to="earliestTimeFrom"></bootstrap-datepicker>
*/


/* Spec directive
Properties:
1. disable-previous-dates  - if attr present then it will disable the previous dates i.e. below then current date
*/

//app.directive('allowDecimalNumbers', function () {
//    return {
//        restrict: 'A',
//        link: function (scope, elm, attrs, ctrl) {

//            elm.on('keydown', function (event) {

//                var $input = $(this);
//                var value = $input.val();
//                value = value.replace(/[^0-9\.]/g, '')
//                var findsDot = new RegExp(/\./g)
//                var containsDot = value.match(findsDot)
//                if (containsDot != null && ([46, 110, 190].indexOf(event.which) > -1)) {
//                    event.preventDefault();
//                    return false;
//                }
//                $input.val(value);;

//                if (event.shiftKey)
//                    return false;
//                if (event.which == 64 || event.which == 16) {
//                    // numbers  
//                    return false;
//                }
//                if ([8, 9, 13, 27, 37, 38, 39, 40].indexOf(event.which) > -1) {
//                    // backspace, enter, escape, arrows  
//                    return true;
//                }
//                else if (event.which >= 48 && event.which <= 57) {
//                    // numbers  
//                    return true;
//                } else if (event.which >= 96 && event.which <= 105) {
//                    // numpad number  
//                    return true;
//                } else if ([46, 110, 190, 123, 116].indexOf(event.which) > -1) {
//                    // dot and numpad dot  
//                    return true;
//                } else {
//                    event.preventDefault();
//                    return false;
//                }
//            });
//        }
//    }
//});


