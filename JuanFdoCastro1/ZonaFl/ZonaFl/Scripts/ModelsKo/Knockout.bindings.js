ko.bindingHandlers.fileUpload = {
    init: function (element, valueAccessor) {
        $(element).after('<div class="progress"><div class="bar"></div><div class="percent">0%</div></div><div class="progressError"></div>');
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var options = ko.utils.unwrapObservable(valueAccessor()),
            property = ko.utils.unwrapObservable(options.property),
            url = ko.utils.unwrapObservable(options.url);

        if (property && url) {
            $(element).change(function () {
                if (element.files.length) {
                    var $this = $(this),
                        fileName = $this.val();

                    // this uses jquery.form.js plugin
                    $(element.form).ajaxSubmit({
                        url: url,
                        type: "POST",
                        dataType: "text",
                        headers: { "Content-Disposition": "attachment; filename=" + fileName },
                        beforeSubmit: function () {
                            $(".progress").show();
                            $(".progressError").hide();
                            $(".bar").width("0%")
                            $(".percent").html("0%");
                        },
                        uploadProgress: function (event, position, total, percentComplete) {
                            var percentVal = percentComplete + "%";
                            $(".bar").width(percentVal)
                            $(".percent").html(percentVal);
                        },
                        success: function (data) {
                            $(".progress").hide();
                            $(".progressError").hide();
                            if (!data.includes("Error Archivo.")) {
                               
                                var strfile = JSON.parse(data);
                                // set viewModel property to filename
                                bindingContext.$data[property](strfile);
                                $("#portfolioimg").attr("src", "/UploadedFiles/Portfolio/" + strfile)
                            }
                            else {
                                alert(data);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            $(".progress").hide();
                            $("div.progressError").html(jqXHR.responseText);
                        }
                    });
                }
            });
        }
    }
}

ko.bindingHandlers.confirmClick = {
    init: function (element, valueAccessor, allBindings, viewModel) {
        var value = valueAccessor();
        var message = ko.unwrap(value.message);
        var click = value.click;
        ko.applyBindingsToNode(element, {
            click: function () {
                if (confirm(message))
                    return click.apply(this, Array.prototype.slice.apply(arguments));
            }
        }, allBindings);
    }
}

