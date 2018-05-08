function getUserDataLinkedin(response) {


   

        $.ajax({
            type: "POST",
            url: "/api/Account/CheckEmail?email=" + response.emailAddress,
            success: function (msg) {

                //$("#status").ajaxComplete(function(event, request, settings){
                "first-name", "last-name"
                if (msg.errors == "") {
                    $('#modal_redes').modal();
                    $('#name_redes').val(response.emailAddress);
                    $('#FirstMiddleName').val(response.first-name +" "+ response.last-name);
                    $('#email_redes').val(response.emailAddress);
                    $("#mensajelogexternal").html("");
                }
                else {
                   
                    $('#loginusername').val(response.emailAddress)
                    if (msg.tipo.PasswordHash != null && msg.noskills == "0") {
                        //$('#modal_inicio').modal();
                        //$("#mensajelogexternal").html(msg.errors);
                        //window.location.replace("/Static/Register.html?UserEmail=" + response.emailAddress + "&freelance=" + msg.tipo.Freelance);
                        window.location.replace("/Freelance/DetailsById/" + msg.tipo.Id);

                    }
                    else
                    {
                        window.location.replace("/Offer/Index/" + msg.tipo.Id);
                        
                    }
                }

                //});

            },
            error: function (xhr, ajaxOptions, thrownError) {
                var responseTitle = $(xhr.responseText).filter('title').get(0);
                alert(responseTitle.text + "\n" + formatErrorMessage(xhr, thrownError));
                disableButton("createUser")
            }
        });



       
        //document.getElementById('response').innerHTML = 'Hello ' + response.name + " email:" + response.email;

   
}

function LinkedINAuth() {
    IN.UI.Authorize().place();
}

function onLinkedInInit() {
    var action = getUrlVars()["accion"];
    if (action != "logout") {
        IN.User.logout();
    }
    else {
        IN.User.logout();
        window.location.href = "/";
        
    }
}

function onLinkedInLoad() {
    


    LinkedINAuth();
    IN.Event.on(IN, "auth", function () { onLinkedInLogin(); });
    IN.Event.on(IN, "logout", function () { onLinkedInLogout(); });
}

function onLinkedInLogin() {
    //IN.API.Profile("me").result(ShowProfileData);

    
    

    IN.API.Profile("me").fields("first-name", "last-name", "email-address").result(function (data) {
        getUserDataLinkedin(data.values[0]);
    }).error(function (data) {
        console.log(data);
    });
}



function onLinkedInLogout() {
    window.location.href = "/";
}

function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}





