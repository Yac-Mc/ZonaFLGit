function getUserDataGoogle(response) {


   

        $.ajax({
            type: "POST",
            url: "/api/Account/CheckEmail?email=" + response.emails[0].value,
            success: function (msg) {

                //$("#status").ajaxComplete(function(event, request, settings){
                "first-name", "last-name"
                if (msg.errors == "") {
                    $('#modal_redes').modal();
                    $('#name_redes').val(response.emails[0].value);
                    $('#FirstMiddleName').val(response.displayName);
                    $('#email_redes').val(response.emails[0].value);
                    $("#mensajelogexternal").html("");
                }
                else {
                   
                    $('#loginusername').val(response.emails[0].value)
                    if (msg.tipo.PasswordHash != null && msg.noskills == "0") {
                        //$('#modal_inicio').modal();
                        //$("#mensajelogexternal").html(msg.errors);
                        //window.location.replace("/Static/Register.html?UserEmail=" + response.emails[0].value + "&freelance=" + msg.tipo.Freelance);
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


function onLoadCallback() {
   
   
    var action = getUrlVars()["accion"];
    if (action != "logout") {
        signOut();
        console.log("Logout Goggle")
    }
    else {
        console.log("Logout Goggle and Redirect")
        signOut();
        window.location.href = "/";


    }
}



function googleAuth() {
    gapi.auth.signIn({
        callback: gPSignInCallback,
        clientid: '95546251444-al728h8j90g13mshia8q50hcf5grg15j.apps.googleusercontent.com',
        cookiepolicy: "single_host_origin",
        requestvisibleactions: "http://schema.org/AddAction",
        'immediate': false,
        scope: "https://www.googleapis.com/auth/plus.login email"
    })
}

function gPSignInCallback(e) {
    if (e["status"]["signed_in"]) {
        gapi.client.load("plus", "v1", function () {
            if (e["access_token"]) {
                getProfile()
            } else if (e["error"]) {
                console.log("There was an error: " + e["error"])
            }
        })
    } else {
        console.log("Sign-in state: " + e["error"])
    }
}

function getProfile() {
    var e = gapi.client.plus.people.get({
        userId: "me"
    });
    e.execute(function (e) {
        if (e.error) {
            console.log(e.message);
            return
        } else if (e.id) {
            getUserDataGoogle(e.result);
        }
    })
} (function () {
    var e = document.createElement("script");
    e.type = "text/javascript";
    e.async = true;
    e.src = "https://apis.google.com/js/client:platform.js?onload=gPOnLoad";
    var t = document.getElementsByTagName("script")[0];
    t.parentNode.insertBefore(e, t)
})()


function signOut() {
   
    gapi.auth.signOut();
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





