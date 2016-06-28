app.directive('multiselectDropdown', [function () {
    return {
        link: function (scope, element, attrs) {
            element.multiselect({
                maxHeight: false,
                width: 'auto',
                buttonText: function (options) {
                    if (options.length == 0) {
                        return attrs.defaultname;
                    }
                    else {
                        var labels = [];
                        options.each(function () {
                            if ($(this).attr('label') !== undefined) {
                                labels.push($(this).attr('label'));
                            }
                            else {
                                labels.push($(this).html());
                            }
                        });
                        return labels.join(', ') + '';
                    }
                }
            });

            // Watch for any changes to the length of our select element
            scope.$watch(function () {
                return element[0].length;
            }, function () {

                element.multiselect('rebuild');
            });

            // Watch for any changes from outside the directive and refresh
            scope.$watch(attrs.ngModel, function () {
                element.multiselect('refresh');
            });

            // for enable/disable

            attrs.$observe('mode', function (val) {
                if (val == "3") {
                    element.multiselect('disable');
                } else if (val != undefined) {
                    element.multiselect('enable');
                }

            });
        }

    };

}]);