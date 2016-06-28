(function ($) {
    $.fn._zTree = function (options, list, acOptions) {
        var element = this;
        var zTreeOnCheck;
        if (options.callback != undefined && $.isFunction(options.callback.onCheck)) {
            var optionCheckFunction = options.callback.onCheck;
            zTreeOnCheck = function (event, treeId, treeNode) {
                optionCheckFunction(event, treeId, treeNode, element.attr("id"))
            };
            options.callback.onCheck = zTreeOnCheck;
        } else {
            zTreeOnCheck = function (event, treeId, treeNode) {
                $(element).val($(element).val(treeNode.name));
            };
        }
        var zTreeOnClick;
        if (options.callback != undefined && $.isFunction(options.callback.onClick)) {
            var optionClickFunction = options.callback.onClick;
            zTreeOnClick = function (event, treeId, treeNode) {
                optionClickFunction(event, treeId, treeNode, element.attr("id"))
            };
            options.callback.onClick = zTreeOnClick;
        }
        else {
            zTreeOnClick = function (event, treeId, treeNode) {
                $(element).val(treeNode.name);
                $("#" + treeId).parent().hide();
                element.data("id", treeNode.Id);
                element.data("name", treeNode.name);
            };
        }

        var defaults = {
            view: {
                dblClickExpand: false,
                showIcon: false
            },
            keep: {
                parent: true
            },
            data: {
                simpleData: {
                    enable: true,
                    idKey: "Id",
                    pIdKey: "ParentId"
                }
            },
            async: {
                enable: true,
                autoParam: "",
                url: "",
                type: "get",
                dataFilter: null

            },

            check: {
                enable: false,
                chkStyle: "checkbox",
                chkboxType: { "Y": "", "N": "" }
            },
            callback: {
                onClick: zTreeOnClick,
                onCheck: zTreeOnCheck
            }
        }
        var treeviewSettings = $.extend(true, {}, defaults, options);
        var init = function () {
            var elements = createContainer(element);
            element.data("treeid", elements.mainContainer.attr("id"));
            $.fn.zTree.init(elements.zTree, treeviewSettings, list);
            if (acOptions != undefined) {
                var onSelectSingleValue;
                if ($.isFunction(acOptions.onSelect)) {
                    var actualOnSelect = acOptions.onSelect;
                    onSelectSingleValue = function (suggestion, Selectcheckbox) {
                        suggestion.value = (suggestion.value != undefined && suggestion.value != "") ? suggestion.value : suggestion.hierarchy;
                        actualOnSelect(suggestion, elements.zTree, Selectcheckbox);
                        onselectCallBack();
                    }
                    acOptions.onSelect = onSelectSingleValue;
                } else {
                    onSelectSingleValue = function (suggestion) {
                        suggestion.value = (suggestion.value == undefined && suggestion.value == "") ? suggestion.value : suggestion.hierarchy;
                        $(element).data("id", suggestion.data);
                        $(element).val(suggestion.value);
                        onselectCallBack();
                    }
                }
                var onselectCallBack = function () {
                    if (!elements.includeCheckBox || elements.includeCheckBox == undefined) {
                        elements.textboxAutocomplete.val("");
                        elements.mainContainer.hide();
                        elements.zTree.show();
                    }
                }
                var autocompleteDefaults = {
                    onSelect: onSelectSingleValue,
                    transformResult: function (response) {
                        return {
                            suggestions: $.map($.parseJSON(response), function (dataItem) {
                                return { value: dataItem.Name, data: dataItem.Id, hierarchy: dataItem.Hierarchy };
                            })
                        };
                    },
                    params: { IncludeInActive: false },
                    beforeRender: function (container, data) {
                        if (data[0].hierarchy != undefined) {
                            var i = 0;
                            var $mainhtml = $("<div>" + container[0].innerHTML + "</div>");
                            var newHtml = "";
                            $mainhtml.find("div").each(function () {
                                var $html = $($(this)[0].outerHTML);

                                newHtml += data[i].hierarchy.indexOf(">") == -1 ? $(this)[0].outerHTML : $html.html($html.html() + "<br /><span class='suggestion-hierarchy'>" + data[i].hierarchy + "</span>")[0].outerHTML;
                                i++;
                            });
                            container[0].innerHTML = newHtml;
                        }
                    },
                    mainObject: element,
                    includeCheckBox: elements.includeCheckBox,
                    customSuggestionsContainer: elements.suggestionsContainer
                };
                var autocompleteSettings = $.extend(true, {}, autocompleteDefaults, acOptions);
                elements.textboxAutocomplete.autocomplete(autocompleteSettings);
            } else {

                elements.mainContainer.find(".autocomplete-search").remove();
            }
        }
        var setPosition = function (textbox, container) {
            var elementHeight = container.height();
            var elementOffset = textbox.offset();
            var scrollTop = $(window).scrollTop();
            var relativeTop = elementOffset.top - scrollTop;
            var bottom = $(window).height() - relativeTop;

            if (bottom < (elementHeight + 65 + $(this).find("button").height())) {
                container.addClass('reverse');

            } else {
                container.removeClass('reverse');
            }
        }
        var initializeEvents = function (textbox, newtextbox, zTreeContainer, checkbox, divZTree) {
            textbox.on("click", function () {
                if (zTreeContainer.css("display") == "none") {
                    zTreeContainer.show();
                    if (newtextbox.val() != "") {
                        newtextbox.focus();
                    }
                } else {
                    zTreeContainer.hide();
                }
            });
            var caret = $('<b class="caret treeview-caret"></b>');
            textbox.after(caret);
            caret.on("click", function () {
                if (zTreeContainer.css("display") == "none") {
                    if (textbox[0].disabled === false) {
                        zTreeContainer.show();
                    }
                } else {
                    zTreeContainer.hide();
                }
            });

            if (element.data("includeinactive") != undefined) {
                checkbox.on("click", function () {
                    var url = options.url;
                    if ($(this).is(":checked")) {
                        element.data("includeinactive", true);
                    } else {
                        element.data("includeinactive", false);
                    }
                    var opt = {};
                    opt.params = { IncludeInActive: element.data("includeinactive") };
                    newtextbox.autocomplete('setOptions', opt);
                    var treesettings = treeviewSettings;
                    treesettings.async.otherParam = { "IncludeInActive": element.data("includeinactive") };
                    $.ajax({
                        url: url,
                        data: { IncludeInActive: element.data("includeinactive") },
                        success: function (result) {
                            $.fn.zTree.destroy(divZTree);
                            $.fn.zTree.init(divZTree, treesettings, result);
                        }
                    });
                });
            }
            $(document).on("mouseup keypress", function (e) {
                var container = zTreeContainer;
                if (!container.is(e.target) && container.has(e.target).length === 0 && !textbox.is(e.target) && !caret.is(e.target)) {
                    container.hide();
                }
            });
        }

        var createContainer = function (obj) {
            var maincontainer = $("<div class='ztree-main-container' style='display:none;'></div>").uniqueId();
            var divZTree = $('<div class="ztree"></div>').uniqueId();
            var textBox = $("<input type='text' />");
            var textboxContainer = $("<div class='autocomplete-search'></div>");
            var checkbox = $("<input type='checkbox' />");
            var InActiveContainer = $("<div class='inactive-container pull-right mr10 mt5'></div>");
            var gylphiconSearch = $('<span class="glyphicon glyphicon-search"></span>');
            var suggestionsContainer = $("<div class='sugg-container'></div>");
            var includeCheckBox = element.data("includecheckbox");
            $(obj).after(maincontainer);

            var includeInactive = element.data("includeinactive");
            if (includeInactive != undefined) {
                var translateTextInactive = "";
                try {
                    translateTextInactive = TranslateText("Include Inactive");
                }
                catch (ex) {
                    translateTextInactive = "Include Inactive";
                }
                InActiveContainer.append("<span class='mr10'>" + translateTextInactive + "</span>");
                InActiveContainer.append(checkbox);
                maincontainer.append(InActiveContainer);
            }
            maincontainer.append(textboxContainer);
            textboxContainer.append(textBox);
            textboxContainer.append(gylphiconSearch);
            maincontainer.append(divZTree);
            maincontainer.append(suggestionsContainer);
            initializeEvents(obj, textBox, maincontainer, checkbox, divZTree);
            return { mainContainer: maincontainer, zTree: divZTree, textboxAutocomplete: textBox, suggestionsContainer: suggestionsContainer, includeCheckBox: includeCheckBox };
        }
        init();
    }
})(jQuery);