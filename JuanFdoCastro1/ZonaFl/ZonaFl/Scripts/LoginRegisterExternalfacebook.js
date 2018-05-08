function getUserDataFacebook() {


    FB.api('/me?fields=id,name,email,permissions', function (response) {

        $.ajax({
            type: "POST",
            url: "/api/Account/CheckEmail?email=" + response.email,
            success: function (msg) {

                //$("#status").ajaxComplete(function(event, request, settings){

                if (msg.errors == "") {
                    $('#modal_redes').modal();
                    $('#name_redes').val(response.email);
                    $('#FirstMiddleName').val(response.name);
                    $('#email_redes').val(response.email);
                    $("#mensajelogexternal").html("");
                }
                else {
                   
                    $('#loginusername').val(response.email)
                    if (msg.tipo.PasswordHash != null && msg.noskills == "0") {
                        //$('#modal_inicio').modal();
                        //$("#mensajelogexternal").html(msg.errors);
                        //window.location.replace( "/Static/Register.html?UserEmail=" + response.email + "&freelance=" + msg.tipo.Freelance);
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

    });
}

window.fbAsyncInit = function () {
    //SDK loaded, initialize it
    FB.init({
        appId: '1377922792228613',
        status: true,
        xfbml: true,
        version: 'v2.7'
    });

    //check user session and refresh it
    FB.getLoginStatus(function (response) {
        var action = getUrlVars()["accion"];
        
        if (response.status === 'connected') {
            //user is authorized
            //document.getElementById('loginBtnfacebook').style.display = 'none';
            if (action != "logout") {
                //getUserData();
            }
            else
            {
                FB.logout();
                window.location.href = "/";
                
            }
        } else {
            //user is not authorized
            FB.logout();
            if (action == "logout") {
                window.location.href = "/";
            }
            //window.location.href = "/";
           
        }
    });
};

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

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.com/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

var el = document.getElementById('loginBtnfacebook');
var er = document.getElementById('registerBtnfacebook');
if (el) {
    el.addEventListener('click', function () {
        //do the login
        FB.login(function (response) {
            if (response.authResponse) {
                //user just authorized your app
                //document.getElementById('loginBtnfacebook').style.display = 'none';

                getUserDataFacebook();
            }
        }, { scope: 'email,public_profile', return_scopes: true });
    }, false);
}

if (er) {
    er.addEventListener('click', function () {
        //do the login
        FB.login(function (response) {
            if (response.authResponse) {
                //user just authorized your app
                //document.getElementById('loginBtnfacebook').style.display = 'none';

                getUserDataFacebook();
            }
        }, { scope: 'email,public_profile', return_scopes: true });
    }, false);
}