var httpStuff = {
    statusCodes:
        {
            OK: 200,
            Created: 201,
            Accepted: 202,
            NoContent: 204,
            BadRequest: 400,
            Unauthorized: 401,
            Forbidden: 403,
            NotFound: 404,
            NotAllowed: 405,
            ServerError: 500
        }
};
function ViewModel() {
    var self = this;
    self.contacts = ko.observableArray();
    self.addContacts = function (contacts) {
        for (var contactIndex in contacts) {
            self.addContact(contacts[contactIndex]);
        }
    };
    this.deleteContact = function (contact) {
        var url = contact.Url;
        $.ajax({
            url: url,
            type: "DELETE",
            success: function () {
                self.contacts.remove(contact);
            },
            error: function (response, status, statusMessage) {
                alert(response.status + ": " + statusMessage);
            }
        });

    };
    this.addContact = function (contact) {
        self.contacts.unshift(contact);
    };
    /* animations */
    this.showContactElement = function (elem, maybe, object) {
        if (elem.nodeType === 1) {
            $(elem).hide().slideDown('normal', function () {
                $("form", this).ajaxForm({
                    iframe: true,
                    success: function (data, message, response, form) {
                        object.HasImage(true);
                    }
                });
            });
        }
    };
    this.deleteContactElement = function (elem) {
        if (elem.nodeType === 1)
            $(elem).slideUp(function () {
                $(elem).remove();
            });
    };
}

$(function () {
    var contactContainer = $("#contacts");
    var contactUrl = contactContainer.attr("data-contact-url");
    var imageUrl = contactContainer.attr("data-contact-image-url");
    var model = new ViewModel();
    ko.applyBindings(model);
    $.getJSON(contactUrl, function (data, message, response) {
        if (response.status == httpStuff.statusCodes.OK) {
            for (var contactIndex in data) {
                createKOContact(data[contactIndex]);
            }
            model.addContacts(data);
        }
    });
    function createKOContact(contact) {
        contact.Url = contactUrl + "/" + contact.Id;
        contact.VCardUrl = contactUrl + "/" + contact.Id + "?format=vcard";
        contact.ImageGet = contactUrl + "/" + contact.Id + "?format=jpg";
        contact.ImagePost = imageUrl + "/" + contact.Id;
        var hasImage = contact.HasImage;
        contact.HasImage = ko.observable(hasImage);
        return contact;
    }

    $("#addContact", this).ajaxForm({
        dataType: "json",
        clearForm: true,
        success: function (data) {
            var contact = createKOContact(data);
            model.addContact(contact);
        },
        error: function (error, status, response) {
            if (error.status === httpStuff.statusCodes.Unauthorized) {
                alert(response);
            }
            else if (error.status == httpStuff.statusCodes.BadRequest) {
                var errorMessage = "Validation error: ";
                var errors = $.parseJSON(error.responseText);
                for (var errorIndex in errors) {
                    var error = errors[errorIndex];
                    errorMessage += "\n" + error.Message;
                }
                alert(errorMessage);
            }
            else {
                alert("Some error:" + response);
            }
        }
    });
    $("#login").ajaxForm({
        dataType: "json",
        success: function (data) {
            $("#login-container").slideUp();
        },
        error: function (error, status, response) {
            alert(response);
        }
    });
});
