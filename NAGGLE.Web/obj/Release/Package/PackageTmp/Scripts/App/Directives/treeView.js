//for Autocomplete so you use isAutocomplete property.
//for hide Parent Checkbox so you use hideParentCheckbox property.
//for Autocomplete with checkbox so you use includeCheckBox property.
//for showing selected element in edit mode so you use data-model property.
//for not take object after select so you use isInt property. 

app.directive('caMultiselectTreeviewAutocomplete', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var scopeName = attrs.caMultiselectTreeviewAutocomplete;
            var controllerName = element.data("controller");
            var isAutocomplete = attrs.isautocomplete;
            var hideParentCheckbox = attrs.ishideparentcheckbox;
            var includeCheckBox = element.data("includecheckbox");
            var scopeSelectedItemsName = element.data("model");
            var displayScope = element.data("displayscope");
            var column = element.data("idcolumn");
            var isInt = element.data("isint");
            scope.$watch(function () {
                return scope[scopeName];
            }
                , function () {
                    if (scope[scopeName].length > 0) {
                        angular.forEach(scope[scopeName], function (item, index) {
                            if (item.ParentId == null) {
                                item.nocheck = hideParentCheckbox;
                            }
                            if (!item.IsSelectable) {
                                item.nocheck = "true";
                            }
                        });
                        element._zTree({
                            async: {
                                autoParam: ["Id=CategoryId"],
                                url: "/User/GetChildCategory",
                                dataFilter: ajaxDataFilter_MsTvAc
                            },
                            check: {
                                enable: true,
                            },
                            callback: {
                                onCheck: zTreeOnCheck_MsTvAc,
                                onClick: zTreeOnCheck_MsTvAc
                            }
                        }, scope[scopeName], isAutocomplete == "false" ? null : {
                            serviceUrl: '/User/SearchCategories',
                            paramName: 'searchText',
                            onSelect: function (data, ztree, Selectcheckbox) {
                                var searchScope = angular.element($("div [ng-controller=" + controllerName + "]")).scope();
                                var zTree = $.fn.zTree.getZTreeObj(ztree.attr("id"));
                                var node = zTree.getNodeByParam("Id", data.data);
                                var ngModel = element.data("ngmodel");
                                var isInt = element.data("isint");
                                if (!Selectcheckbox && includeCheckBox) {
                                    node.checked = false;
                                    zTree.updateNode(node);
                                }
                                else {
                                    if (node != null) {
                                        node.checked = true;
                                        zTree.updateNode(node);
                                    } else {
                                        zTree.addNodes(null, { Id: data.data, name: data.value, checked: true, isHidden: true }, true);
                                    }
                                }
                                var selectedNode = zTree.getCheckedNodes(true);
                                var nodeCount = selectedNode.length;
                                var selectedUnits = "";
                                var selectedUnitIds = "";
                                scope[ngModel] = [];
                                scope[displayScope] = [];
                                //searchScope.BusinessUnitItems = [];
                                for (var i = 0; i < nodeCount; i++) {
                                    var parentNode = getParentNode(selectedNode[i], true);
                                    selectedUnits += parentNode + ", ";
                                    if (isInt != undefined && isInt == true)
                                        scope[ngModel][i] = selectedNode[i].Id;
                                    else
                                        scope[ngModel][i] = { Id: selectedNode[i].Id, Name: selectedNode[i].Name, ParentId: selectedNode[i].ParentId };
                                    if (displayScope != undefined) {
                                        scope[displayScope][i] = { Id: selectedNode[i].Id, Name: parentNode }
                                    }
                                }
                                if (selectedUnits.length > 0) {
                                    selectedUnits = selectedUnits.substr(0, selectedUnits.length - 2);
                                }
                                element.val(selectedUnits);
                                scope.$apply(function () {
                                    scope[displayScope];
                                });
                            }, autocompleteResponseCallback: includeCheckBox ? autocompleteResponseCallback = function (container, data) {
                                var treeId = element.parent().find('.ztree')[0].id;
                                var treeObj = $.fn.zTree.getZTreeObj(treeId);
                                var selectedNodes = treeObj.getCheckedNodes();
                                for (var i = 0; i < selectedNodes.length; i++) {
                                    $(".sugg-container input.autocomplete-suggestion").each(function () {
                                        if (selectedNodes[i].name == $(this).parent().text()) {
                                            $(this).attr('checked', true);
                                            return;
                                        }
                                    });
                                }
                            } : null
                        });
                    }
                });
            scope.RemoveAssignCategorie = function (Id) {
                for (var i = 0; i < scope[scopeSelectedItemsName].length; i++)
                    if (scope[scopeSelectedItemsName][i] === Id ||scope[scopeSelectedItemsName][i].Id === Id) {
                        scope[scopeSelectedItemsName].splice(i, 1);
                        CheckNodesMS(scope[scopeSelectedItemsName], element, column, displayScope == undefined ? undefined : displayScope);
                        var zTree1 = $.fn.zTree.getZTreeObj(element.parent().find(".ztree-main-container>.ztree").attr("id"));
                        var node = zTree1.getNodeByParam("Id", Id);
                        if (node) {
                            node.checked = false;
                            zTree1.updateNode(node);
                        }
                        break;
                    }
            }
            scope.$watchCollection(function () { return (scope[scopeSelectedItemsName]); }, function (value) {
                if (scope[scopeSelectedItemsName] != undefined) {
                    CheckNodesMS(scope[scopeSelectedItemsName], element, column, displayScope == undefined ? undefined : displayScope);
                }
            });
        }
    };
});
function ajaxDataFilter_MsTvAc(treeId, parentNode, responseData) {
    if (responseData) {
        var zTree = $.fn.zTree.getZTreeObj(treeId);
        for (var i = 0; i < responseData.length; i++) {
            if (!responseData[i].IsSelectable) {
                responseData[i].nocheck = "true";
            }
            var node = zTree.getNodeByParam("Id", responseData[i].Id);
            if (node != null) {
                zTree.removeNode(node);
                responseData[i].checked = node.checked == true ? true : false;
            }
        }
    }
    return responseData;
};
function zTreeOnCheck_MsTvAc(event, treeId, treeNode, mainElementId) {
    var element = $("#" + mainElementId);
    var zTree = $.fn.zTree.getZTreeObj(treeId);
    if (event.type == "click") {

        zTree.checkNode(treeNode);
        $("#" + treeId).parent('.ztree-main-container').hide();
    }
    var selectedNode = zTree.getCheckedNodes();
    var nodeCount = selectedNode.length;
    var selectedUnits = "";
    var selectedUnitIds = "";
    var controllerName = element.data("controller");
    var scope = angular.element($("div [ng-controller=" + controllerName + "]")).scope();
    var ngModel = element.data("ngmodel");
    var isInt = element.data("isint");
    var idColumn = element.data("idcolumn");
    var displayScope = element.data("displayscope");
    scope[ngModel] = [];
    scope[displayScope] = [];
    for (var i = 0; i < nodeCount; i++) {
        var currenNode = selectedNode[i];
        selectedUnits += currenNode.name + ", ";
        //   scope[ngModel][i] = selectedNode[i].Id, selectedNode[i].name;
        if (isInt != undefined && isInt == true)
            scope[ngModel][i] = (idColumn == undefined || idColumn == "") ? selectedNode[i].Id : selectedNode[i][idColumn];
        else
            scope[ngModel][i] = { Id: (idColumn == undefined || idColumn == "") ? selectedNode[i].Id : selectedNode[i][idColumn], Name: selectedNode[i].Name, ParentId: selectedNode[i].ParentId };
        if (displayScope != undefined) {
            scope[displayScope][i] = { Id: selectedNode[i].Id, Name: currenNode.name }
        }
    }
    if (selectedUnits.length > 0) {
        selectedUnits = selectedUnits.substr(0, selectedUnits.length - 2);
    }
    element.val(selectedUnits);
    scope.$apply(function () {
        scope[displayScope];
    });
};

var CheckNodesMS = function (scope, element, column, displayScope) {
    var zTree = $.fn.zTree.getZTreeObj(element.parent().find(".ztree-main-container>.ztree").attr("id"));
    if (zTree == null || zTree == undefined)
        return;
    zTree.checkAllNodes(false);
    var controllerName = element.data("controller");
    var scope1 = angular.element($("div [ng-controller=" + controllerName + "]")).scope();
    var treeSiteNames = "";
    var ngModel = element.data("ngmodel");
    var isInt = element.data("isint");
    scope1[displayScope] = [];
    for (var i = 0; i < scope.length; i++) {
        var node = zTree.getNodeByParam(column == undefined ? "Id" : column, scope[i].Id != undefined ? scope[i].Id : scope[i]);
        if (node != null && !node.checked) {
            if (node.ParentId != null) {
                zTree.expandNode(node.getParentNode(), true, null, null);
            }
            var parentNode = getParentNode(node, true);
            treeSiteNames += parentNode + ", ";
            zTree.checkNode(node);
            if (isInt != undefined && isInt == true)
                scope1[ngModel][i] = scope[i].Id != undefined ? scope[i].Id : scope[i];
            else
                scope1[ngModel][i] = { Id: scope[i].Id, Name: scope[i].Name, ParentId: scope[i].ParentId };
        }
        else {
            zTree.addNodes(null, { Id: scope[i].Id, name: scope[i].Name, checked: true, isHidden: true }, true);
            var selectedNode = zTree.getCheckedNodes(true);
            scope1[ngModel] = [];
            for (var j = 0; j < selectedNode.length; j++) {
                var parentNode = getParentNode(selectedNode[j], true);
                treeSiteNames += parentNode + ", ";
                if (isInt != undefined && isInt == true)
                    scope1[ngModel][j] = selectedNode[j].Id;
                else
                    scope1[ngModel][j] = { Id: selectedNode[j].Id, Name: selectedNode[j].Name, ParentId: selectedNode[j].ParentId };

            }
        }
   
        if (displayScope != undefined) {
            scope1[displayScope][i] = { Id: scope[i].Id != undefined ? scope[i].Id : scope[i], Name: parentNode };
        }
    }

    if (treeSiteNames.length > 0) {
        treeSiteNames = treeSiteNames.substr(0, treeSiteNames.length - 2);
    }
    if (treeSiteNames != undefined)
        element.val(treeSiteNames);

    // }
}
var parentNode_MsTvAc = "";
var getParentNode = function (node, reset) {
    if (reset) {
        parentNode = "";
    }

    if (node.getParentNode() != null) {
        getParentNode(node.getParentNode(), false);
    }
    parentNode = node.name;
    return parentNode;
}