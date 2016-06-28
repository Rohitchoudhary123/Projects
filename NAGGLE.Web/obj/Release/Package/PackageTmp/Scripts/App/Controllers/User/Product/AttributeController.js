var accountContainer = $("#mainContainer");
var assignCategorieTree = $("#AssignCategorieTree");
app.controller('attributeController', ['$scope', '$filter', 'attributeService', 'notification', 'bootboxModal', 'loader', function ($scope, $filter, attributeService, notification, bootboxModal, loader) {

    $scope.Attributes = [];
    $scope.Categories = [];

    $scope.FeatureTypes = [];
    $scope.Attribute = {};
    $scope.Mode = { List: 1, AddEdit: 2 };
    $scope.CurrentMode = $scope.Mode.List;
    $scope.StatusList = [{
        Id: true,
        name: 'Active'
    }, {
        Id: false,
        name: 'Disabled'
    }];

   
    /*Get all attributes*/
    attributeService.getAttributes().then(function (response) {
        $scope.Categories = response.Categories;
        $scope.Attributes = response.Attributes;
        $scope.FeatureTypes = response.FeatureTypes;
        accountContainer.show();
    });
    /*Add or Edit Attribute Detail*/
    $scope.saveAttributeDetail = function (attribute, $event) {
        loader.show($event.currentTarget);

        var AssignCategories = [];
        angular.forEach($scope.SelectedCategories, function (item, key) {
            //AssignCategories.push({ CategoryId: item })
            AssignCategories.push({ CategoryId: item, AttributeId: attribute.AttributeId })
        });
        attribute.AttributeCategories = AssignCategories;

        attributeService.addEditAttribute(attribute).then(function (response) {
            var result = response.Attribute;
            if (response.Message == "Saved") {
                $scope.Attributes.push(result);
                notification.success("Saved Successfully.");
            }
            else {
                var updateAttribute = $filter('filter')($scope.Attributes, { AttributeId: result.AttributeId }, true)[0];
                updateAttribute.FeatureName = result.FeatureName;
                updateAttribute.Status = result.Status;
                updateAttribute.AttributeCategories=result.AttributeCategories;
                notification.success("Updated Successfully.");
            }
            loader.hide();
            $scope.CurrentMode = $scope.Mode.List;
        }, function (err) {
            loader.hide();
        });
    }

    /*Delete Attribute by attributeId*/
    //$scope.deleteAttribute = function (attributeId, index) {
    //    bootbox.confirm("Are you sure  you want to remove this Attribute ?", function (result) {
    //        if (result) {
    //            attributeService.deleteSingleAttribute(attributeId).then(function (response) {
    //                $scope.Attributes.splice(index, 1);
    //                notification.success("Deleted Successfully.");
    //            });
    //        }
    //    });
    //}


    /*Delete Single and Multiple Attribute*/
    $scope.deleteAttribute = function (attribute) {
        bootbox.confirm("Are you sure  you want to remove  Attribute ?", function (result) {
            if (result) {
                var seletedAttributes = attribute != undefined ?[attribute]: $filter('filter')($scope.Attributes, { checked: true });
                attributeService.deleteMultipleAttribute(seletedAttributes).then(function (response) {
                    
                    angular.forEach(seletedAttributes, function (item, index) {
                        var filterAttributeList = $filter('filter')($scope.Attributes, { AttributeId: '!' + item.AttributeId }, true);
                        $scope.Attributes = filterAttributeList;
                    });
                    //allCheckedFalse();
                    //   $scope.Attributes.splice(index, 1);
                    notification.success("Deleted Successfully.");
                });
            }
        });
    }


    /*Update Status on AttributeList Page*/
    $scope.updateStatus = function (attribute) {
        attributeService.updateStatus(attribute).then(function (response) {
            var updateAttribute = $filter('filter')($scope.Attributes, { AttributeId: response.AttributeId }, true)[0];
            updateAttribute.Status = response.Status;
            notification.success("Status Updated Successfully.");

        });
    }

    /*Open Add New Attribute Form*/
    $scope.addNewAttribute = function (attributeId) {
        $scope.Attribute = {};
        $scope.CurrentMode = $scope.Mode.AddEdit;
        allCheckedFalse();
    }
   
    /*Edit Attribute and get specific Attribute */
    $scope.editAttribute = function (attributeId) {
        allCheckedFalse();
        assignCategorieTree.val(" ");
        attributeService.getAttributeById(attributeId).then(function (response) {
            if (response.Success) {
                $scope.Attribute = response.Attribute;
                /*Getting Selected Categories name */
                $scope.SelectedCategories = [];
                angular.forEach(response.Attribute.AttributeCategories, function (item, key) {
                    $scope.SelectedCategories.push(item.CategoryId);
                });
            }
            $scope.CurrentMode = $scope.Mode.AddEdit;

        });

    }
    $scope.cancelAttribute = function () {
        $scope.CurrentMode = $scope.Mode.List;
    }

    var allCheckedFalse = function ()
    {
        $.each($scope.Attributes, function (index, value) {
            if (value.checked)
                value.checked = false;
        });
    }

}

]);