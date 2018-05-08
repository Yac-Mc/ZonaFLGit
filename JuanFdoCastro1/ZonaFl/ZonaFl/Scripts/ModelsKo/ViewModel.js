var ViewModel = (function (data) {

    if (data != null)
    {
        ko.mapping.fromJS(data, { Postulados: postuladosMapping }, self);
    }
   
    var self = this;
    
   self.getPostuladosFromServer = function () {
        var idoffer = '-1'; 
        $.get("/api/OfferR/GetOfferUsers?offerid=" + idoffer).always(function (data) {
            self.koTable.setItems(data);
            });
    };


    self.getPostuladosFilterFromServer = function (idoffer) {
       
        $.get("/api/OfferR/GetOfferUsers?offerid=" + idoffer).always(function (data) {
            $('#noPostulados').text(data.length);
            self.koTable.setItems(data);
           
        });
    };

    
    self.GetSetProjetToUser = function (data) {
        self.IdUser = ko.observable(data.IdUser);
        var offeruser = ko.utils.stringifyJson(data);
        var id =ko.utils.stringifyJson(data.Id);
        var idoffer=ko.utils.stringifyJson(data.IdOffer);
        var iduser=ko.utils.stringifyJson(data.IdUser);
        var nameuser = ko.utils.stringifyJson(data.NameUser);

        $.get("/api/OfferR/GetSetProjetToUser?Id="+id).always(function (data) {
            $('#noPostulados').text(data.length);
            self.koTable.setItems(data);
           });

    };

    //self.filteredRecords = ko.computed(function () {
    //    return ko.utils.arrayFilter(self.Postulados(), function (rec) {
    //        return (
                        
    //                     (self.search_Postulados().length == 0 ||
    //                           rec.Address().toLowerCase().indexOf(self.search_Postulados().toLowerCase()) > -1)
                           
                        
    //                  )
    //    });
    //});
    //var PostuladoDetail = function (data) {
    //    var self = this;
    //    if (data != null) {
    //        self.OfferId = ko.observable(data.OfferId);
           
    //    }
    //}

    //var postuladosMapping = {
    //    create: function (options) {
    //        return new PostuladoDetail(options.data);
    //    }
    //};

    //self.filterByidoffer = function () {
    //    alert("entro");
    //    var idoffer = 1437;
    //    $.get("/api/OfferR/GetOfferUsers?offerid="+idoffer).always(function (data) {
    //         //load the data we received
    //       self.koTable.setItems(data);
           
    //    });
    //};

  


    


    // ready is automcatically invoked when koTable is initialized.
    self.koTableReady = function () {
        // load the data form the server
        self.getPostuladosFromServer();
    };

    

});



var ViewModelRegister = (function (data) {
    self = this;
    self.skillsList16 = ko.observableArray();
    self.skillsList17 = ko.observableArray();
    self.skillsList18 = ko.observableArray();
    self.skillsList19 = ko.observableArray();
    self.skillsList20 = ko.observableArray();
    self.skillsList21 = ko.observableArray();
    self.skillsList22 = ko.observableArray();
    self.skillsList23 = ko.observableArray();
    self.skillsList24 = ko.observableArray();
    self.skillsList25 = ko.observableArray();
    self.Empresa = ko.observable(false);
   
    if (data != null) {
        ko.mapping.fromJS(data, { Skills: skillsMapping }, self);
    }

    self.verifySessionUser = function () {
        var hash = parseURL(window.location.href).hash;
        if (hash != '#contact-page') {
            $.ajax({
                url: "/Home/verifySessionUser",
                type: "POST",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                success: function (response) {
                    if (response.success) {
                        // do something clever

                        //alert("Se ha guardado correctamente la información!!!");
                        if (response.Url != "/Home/index")
                            top.location.href = response.Url; //'freelance.html?UserEmail=' + response.UserEmail;


                    } else {
                        alert('Ha ocurrido un error: ' + response.errors)





                    }


                },




                error: function (xhr, ajaxOptions, thrownError) {
                    var responseTitle = $(xhr.responseText).filter('title').get(0);
                    alert(responseTitle.text + "\n" + formatErrorMessage(xhr, thrownError));

                }

            });
        }


    }
    
    self.getSkillsFromServer = function (idcategory) {
        
        $.get("/api/OfferR/GetSkills?categoryid=" + idcategory).always(function (data) {

            if (idcategory == '16')
            {
                self.skillsList16(data);
            }
            else if (idcategory == '17') {
                self.skillsList17(data);

            }
            else if(idcategory == '18')
                {
                self.skillsList18(data);

            }
            else if (idcategory == '19') {
                self.skillsList19(data);

            }
            else if (idcategory == '20') {
                self.skillsList20(data);

            }

            else if (idcategory == '21') {
                self.skillsList21(data);

            }
            else if (idcategory == '22') {
                self.skillsList22(data);

            }
            else if (idcategory == '23') {
                self.skillsList23(data);

            }
            else if (idcategory == '24') {
                self.skillsList24(data);

            }
            else if (idcategory == '25') {
                self.skillsList25(data);

            }
            
        });
    };


   


    

    //self.filteredRecords = ko.computed(function () {
    //    return ko.utils.arrayFilter(self.Postulados(), function (rec) {
    //        return (

    //                     (self.search_Postulados().length == 0 ||
    //                           rec.Address().toLowerCase().indexOf(self.search_Postulados().toLowerCase()) > -1)


    //                  )
    //    });
    //});
    //var PostuladoDetail = function (data) {
    //    var self = this;
    //    if (data != null) {
    //        self.OfferId = ko.observable(data.OfferId);

    //    }
    //}

    //var postuladosMapping = {
    //    create: function (options) {
    //        return new PostuladoDetail(options.data);
    //    }
    //};

    //self.filterByidoffer = function () {
    //    alert("entro");
    //    var idoffer = 1437;
    //    $.get("/api/OfferR/GetOfferUsers?offerid="+idoffer).always(function (data) {
    //         //load the data we received
    //       self.koTable.setItems(data);

    //    });
    //};







    // ready is automcatically invoked when koTable is initialized.
    //self.koTableReady = function () {
    //    // load the data form the server
    //    self.getPostuladosFromServer();
    //};



});


var ViewModelEdit = (function (data) {
    self = this;
    self.skillsList16 = ko.observableArray();
    self.skillsList17 = ko.observableArray();
    self.skillsList18 = ko.observableArray();
    self.skillsList19 = ko.observableArray();
    self.skillsList20 = ko.observableArray();
    self.skillsList21 = ko.observableArray();
    self.skillsList22 = ko.observableArray();
    self.skillsList23 = ko.observableArray();
    self.skillsList24 = ko.observableArray();
    self.skillsList25 = ko.observableArray();
    if (data != null) {
        ko.mapping.fromJS(data, { Skills: skillsMapping }, self);
    }


    self.getSkillsFromServer = function (idcategory,iduser) {

        $.get("/api/OfferR/GetSkillsEdit?categoryid=" + idcategory + "&iduser=" + iduser).always(function (data) {

            if (idcategory == '16') {
                self.skillsList16(data);
            }
            else if (idcategory == '17') {
                self.skillsList17(data);

            }
            else if (idcategory == '18') {
                self.skillsList18(data);

            }
            else if (idcategory == '19') {
                self.skillsList19(data);

            }
            else if (idcategory == '20') {
                self.skillsList20(data);

            }

            else if (idcategory == '21') {
                self.skillsList21(data);

            }
            else if (idcategory == '22') {
                self.skillsList22(data);

            }
            else if (idcategory == '23') {
                self.skillsList23(data);

            }
            else if (idcategory == '24') {
                self.skillsList24(data);

            }
            else if (idcategory == '25') {
                self.skillsList25(data);

            }

        });
    };







    //self.filteredRecords = ko.computed(function () {
    //    return ko.utils.arrayFilter(self.Postulados(), function (rec) {
    //        return (

    //                     (self.search_Postulados().length == 0 ||
    //                           rec.Address().toLowerCase().indexOf(self.search_Postulados().toLowerCase()) > -1)


    //                  )
    //    });
    //});
    //var PostuladoDetail = function (data) {
    //    var self = this;
    //    if (data != null) {
    //        self.OfferId = ko.observable(data.OfferId);

    //    }
    //}

    //var postuladosMapping = {
    //    create: function (options) {
    //        return new PostuladoDetail(options.data);
    //    }
    //};

    //self.filterByidoffer = function () {
    //    alert("entro");
    //    var idoffer = 1437;
    //    $.get("/api/OfferR/GetOfferUsers?offerid="+idoffer).always(function (data) {
    //         //load the data we received
    //       self.koTable.setItems(data);

    //    });
    //};







    // ready is automcatically invoked when koTable is initialized.
    //self.koTableReady = function () {
    //    // load the data form the server
    //    self.getPostuladosFromServer();
    //};



});


var ViewModelUsers = (function (data) {

    var self = this;
    self.useradmin = ko.observable("nombre");
    self.mailadmin = ko.observable("mail");
    self.nameadmin = ko.observable("name");
    self.passadmin = ko.observable("passadmin");
    self.passadmin2 = ko.observable("passadmin2");

    if (data != null) {
        ko.mapping.fromJS(data, { Users: usersMapping }, self);
    }

    var self = this;
    self.getUsersFromServer = function () {
        
        $.get("/api/OfferR/GetUsers").always(function (data) {
            self.koTable.setItems(data);
        });
    };

    self.suspendUser = function (data) {
        var status = ko.utils.stringifyJson(data.Status);
        
        
            var email = ko.utils.stringifyJson(data.Email);
            var status = ko.utils.stringifyJson(data.Status);
            $.get("/api/OfferR/GetSuspendUser?Email=" + email+"&Estado="+status).always(function (data) {
                self.koTable.setItems(data);
            });
        
           
       
    }


    self.suspendUserByIdUser = function (id) {
        //var id="@ViewBag.IdUser"
        var status = 1;
        if (window.confirm("Esta seguro(a) de darse de baja?")) {
            var email = ko.utils.stringifyJson(email);
            var status = ko.utils.stringifyJson("Suspendido");
            $.get("/api/OfferR/GetSuspendUser?id=" + id).always(function (data) {
                if (data == id) {
                    window.alert("se ha dado de baja el usuario");
                    top.location.href = "/Users/Logout";
                }
                else
                {
                    window.alert("Nos se ha dado de baja el usuario, favor comunicarse con el administrador del sistema");
                    top.location.href = "/Users/Logout";
                }
            });
        }

    }
    self.CreateAdmin = function (data) {
        var free = 0;
        var empre = 0;
        var useradmin = {
            Email: $('#mailadmin').val(),
            UserName: $('#useradmin').val(),
            PasswordHash: $('#passadmin').val(),
            confirmpassword: $('#passadmin2').val(),
            FirstMiddleName: $('#nameadmin').val(),
            Freelance: free,
            Empresa: empre

        };

        

          
        if (self.useradmin() == "") {

            alert("Favor digitar toda la información del usuario");
        }
        else {

            $.ajax({
                url: "/api/Account/Register",
                data: JSON.stringify(useradmin),
                type: "POST",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                success: function (response) {
                    if (response.success) {
                        $("#createadmin")[0].reset();
                        alert('Se ha creado correctamente el usuario administrador');


                    } else {
                        $("#createadmin")[0].reset();
                        alert('Ha ocurrido un error: ' + response.errors)





                    }


                },




                error: function (xhr, ajaxOptions, thrownError) {
                    var responseTitle = $(xhr.responseText).filter('title').get(0);
                    alert(responseTitle.text + "\n" + formatErrorMessage(xhr, thrownError));

                }

            });

        }




    }

    

    

    // ready is automcatically invoked when koTable is initialized.
    self.koTableReady = function () {
        // load the data form the server
        self.getUsersFromServer();
    };



});

var ViwModelNotifications = (function () {
    var self = this;
    self.notificationsUser = ko.observableArray([]);
    self.getNotificationsByUser = function (IdUser) {
        $.get("/api/NotificationsR/GetNotificationsByUser?userid="+IdUser).always(function (data) {
                self.notificationsUser(data);
        });
    }
});


//var VieModelHvExperience = {
//    setExperiences: function () {
//      alert("entro")
//    }
//};

//var VieModelHvExperience = {
//    self :this,
//    self.company : ko.observable(),
//self.dateini : ko.observable(),
//self.dateend : ko.observable(),
//self.currentemploy : ko.observable(),
//self.position : ko.observable(),
//self.functionposition : ko.observable();
//    absorbEnter: function (data, event) {
//        return event.keyCode !== 13;
//    },
//    setExperiences: function () {
//        console.log("submitting", arguments);
//    }
//};
function parseJsonDate(jsonDateString) {
    if (jsonDateString != undefined) {
        var date = new Date(parseInt(jsonDateString.substr(6)));
        var displayDate = $.datepicker.formatDate("dd/mm/yy", date);
        return displayDate;
    }
    else {
        return "";
    }
   
}
var VieModelHvExperience = (function (data,userid) {

    selfExp = this;
    //if (data.length > 0) {
    //    $.each(data, function (i, obj) {
    if (data.Company != undefined) {
        selfExp.Company = ko.observable(data.Company);

        selfExp.DateIni = ko.observable(data.DateIni);
        selfExp.DateEnd = ko.observable(data.DateEnd);
        selfExp.CurrentEmploy = ko.observable(data.CurrentEmploy);
        selfExp.Position = ko.observable(data.Position);
        selfExp.FunctionPosition = ko.observable(data.FunctionPosition);
        selfExp.UserId = ko.observable(userid);
    }
    else {
        selfExp.Company = ko.observable("");
        selfExp.DateIni = ko.observable("");
        selfExp.DateEnd = ko.observable("");
        selfExp.CurrentEmploy = ko.observable("");
        selfExp.Position = ko.observable("");
        selfExp.FunctionPosition = ko.observable("");
        selfExp.UserId = ko.observable(userid);
    }

    if (data.Company2 != undefined) {
        selfExp.Company2 = ko.observable(data.Company2);
        selfExp.DateIni2 = ko.observable(data.DateIni2);
        selfExp.DateEnd2 = ko.observable(data.DateEnd2);
        selfExp.CurrentEmploy2 = ko.observable(data.CurrentEmploy2);
        selfExp.Position2 = ko.observable(data.Position2);
        selfExp.FunctionPosition2 = ko.observable(data.FunctionPosition2);

    }
    else {
        selfExp.Company2 = ko.observable("");
        selfExp.DateIni2 = ko.observable("");
        selfExp.DateEnd2 = ko.observable("");
        selfExp.CurrentEmploy2 = ko.observable("");
        selfExp.Position2 = ko.observable("");
        selfExp.FunctionPosition2 = ko.observable("");

    }

    if (data.Company3 != undefined) {
        selfExp.Company3 = ko.observable(data.Company3);
        selfExp.DateIni3 = ko.observable(data.DateIni3);
        selfExp.DateEnd3 = ko.observable(data.DateEnd3);
        selfExp.CurrentEmploy3 = ko.observable(data.CurrentEmploy3);
        selfExp.Position3 = ko.observable(data.Position3);
        selfExp.FunctionPosition3 = ko.observable(data.FunctionPosition3);

    }
    else {
        selfExp.Company3 = ko.observable("");
        selfExp.DateIni3 = ko.observable("");
        selfExp.DateEnd3 = ko.observable("");
        selfExp.CurrentEmploy3 = ko.observable("");
        selfExp.Position3 = ko.observable("");
        selfExp.FunctionPosition3 = ko.observable("");

    }

    //    });
    //}
    //else
    //{
    //    selfExp.Company = ko.observable("");
    //    selfExp.DateIni = ko.observable("");
    //    selfExp.DateEnd = ko.observable("");
    //    selfExp.CurrentEmploy = ko.observable("");
    //    selfExp.Position = ko.observable("");
    //    selfExp.FunctionPosition = ko.observable("");
    //    selfExp.UserId = ko.observable(userid);

    //    selfExp.Company2 = ko.observable("");
    //    selfExp.DateIni2 = ko.observable("");
    //    selfExp.DateEnd2 = ko.observable("");
    //    selfExp.CurrentEmploy2 = ko.observable("");
    //    selfExp.Position2 = ko.observable("");
    //    selfExp.FunctionPosition2 = ko.observable("");

    //    selfExp.Company3 = ko.observable("");
    //    selfExp.DateIni3 = ko.observable("");
    //    selfExp.DateEnd3 = ko.observable("");
    //    selfExp.CurrentEmploy3 = ko.observable("");
    //    selfExp.Position3 = ko.observable("");
    //    selfExp.FunctionPosition3 = ko.observable("");
    //}
    

   


    selfExp.setExperiences = function () {
        var jsonData = ko.toJSON(selfExp);


        $.ajax({
            url: "/Curriculum/InsertExperienceLab",
            type: "POST",
            data: jsonData,
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (returnedData) {
                if (returnedData) {
                    alert("Se ha guardado correctamente la información, continue con la siguiente pestaña");
                }
                else
                {
                    alert("Error al guardar la información, favor comunicarse con el administrador del sistema");
                }
            }
        });
        //$.post("/Curriculum/InsertExperienceLab", jsonData, function (returnedData) {
        //    if(returnedData.rta)
        //    {
        //        alert("Se ha guardado correctamente la información");
        //    }
        //    else
        //    {
        //        alert("Error al guardar la información, favor comunicarse con el administrador del sistema");
        //    }
        //        })
    };
});


var VieModelHvEducation = (function (data,userid) {

    selfEdu = this;

    selfEdu.items = ['Afghanistan', 'Albania', 'Algeria', 'American Samoa', 'Andorra', 'Angola', 'Anguilla', 'Antarctica', 'Antigua And Barbuda', 'Argentina', 'Armenia', 'Aruba', 'Australia', 'Austria', 'Azerbaijan', 'Bahamas The', 'Bahrain', 'Bangladesh', 'Barbados', 'Belarus', 'Belgium', 'Belize', 'Benin', 'Bermuda', 'Bhutan', 'Bolivia', 'Bosnia and Herzegovina', 'Botswana', 'Bouvet Island', 'Brazil', 'British Indian Ocean Territory', 'Brunei', 'Bulgaria', 'Burkina Faso', 'Burundi', 'Cambodia', 'Cameroon', 'Canada', 'Cape Verde', 'Cayman Islands', 'Central African Republic', 'Chad', 'Chile', 'China', 'Christmas Island', 'Cocos (Keeling) Islands', 'Colombia', 'Comoros', 'Congo', 'Congo The Democratic Republic Of The', 'Cook Islands', 'Costa Rica', 'Cote DIvoire (Ivory Coast)', 'Croatia (Hrvatska)', 'Cuba', 'Cyprus', 'Czech Republic', 'Denmark', 'Djibouti', 'Dominica', 'Dominican Republic', 'East Timor', 'Ecuador', 'Egypt', 'El Salvador', 'Equatorial Guinea', 'Eritrea', 'Estonia', 'Ethiopia', 'External Territories of Australia', 'Falkland Islands', 'Faroe Islands', 'Fiji Islands', 'Finland', 'France', 'French Guiana', 'French Polynesia', 'French Southern Territories', 'Gabon', 'Gambia The', 'Georgia', 'Germany', 'Ghana', 'Gibraltar', 'Greece', 'Greenland', 'Grenada', 'Guadeloupe', 'Guam', 'Guatemala', 'Guernsey and Alderney', 'Guinea', 'Guinea-Bissau', 'Guyana', 'Haiti', 'Heard and McDonald Islands', 'Honduras', 'Hong Kong S.A.R.', 'Hungary', 'Iceland', 'India', 'Indonesia', 'Iran', 'Iraq', 'Ireland', 'Israel', 'Italy', 'Jamaica', 'Japan', 'Jersey', 'Jordan', 'Kazakhstan', 'Kenya', 'Kiribati', 'Korea North', 'Korea South', 'Kuwait', 'Kyrgyzstan', 'Laos', 'Latvia', 'Lebanon', 'Lesotho', 'Liberia', 'Libya', 'Liechtenstein', 'Lithuania', 'Luxembourg', 'Macau S.A.R.', 'Macedonia', 'Madagascar', 'Malawi', 'Malaysia', 'Maldives', 'Mali', 'Malta', 'Man (Isle of)', 'Marshall Islands', 'Martinique', 'Mauritania', 'Mauritius', 'Mayotte', 'Mexico', 'Micronesia', 'Moldova', 'Monaco', 'Mongolia', 'Montserrat', 'Morocco', 'Mozambique', 'Myanmar', 'Namibia', 'Nauru', 'Nepal', 'Netherlands Antilles', 'Netherlands The', 'New Caledonia', 'New Zealand', 'Nicaragua', 'Niger', 'Nigeria', 'Niue', 'Norfolk Island', 'Northern Mariana Islands', 'Norway', 'Oman', 'Pakistan', 'Palau', 'Palestinian Territory Occupied', 'Panama', 'Papua new Guinea', 'Paraguay', 'Peru', 'Philippines', 'Pitcairn Island', 'Poland', 'Portugal', 'Puerto Rico', 'Qatar', 'Reunion', 'Romania', 'Russia', 'Rwanda', 'Saint Helena', 'Saint Kitts And Nevis', 'Saint Lucia', 'Saint Pierre and Miquelon', 'Saint Vincent And The Grenadines', 'Samoa', 'San Marino', 'Sao Tome and Principe', 'Saudi Arabia', 'Senegal', 'Serbia', 'Seychelles', 'Sierra Leone', 'Singapore', 'Slovakia', 'Slovenia', 'Smaller Territories of the UK', 'Solomon Islands', 'Somalia', 'South Africa', 'South Georgia', 'South Sudan', 'Spain', 'Sri Lanka', 'Sudan', 'Suriname', 'Svalbard And Jan Mayen Islands', 'Swaziland', 'Sweden', 'Switzerland', 'Syria', 'Taiwan', 'Tajikistan', 'Tanzania', 'Thailand', 'Togo', 'Tokelau', 'Tonga', 'Trinidad And Tobago', 'Tunisia', 'Turkey', 'Turkmenistan', 'Turks And Caicos Islands', 'Tuvalu', 'Uganda', 'Ukraine', 'United Arab Emirates', 'United Kingdom', 'United States', 'United States Minor Outlying Islands', 'Uruguay', 'Uzbekistan', 'Vanuatu', 'Vatican City State (Holy See)', 'Venezuela', 'Vietnam', 'Virgin Islands (British)', 'Virgin Islands (US)', 'Wallis And Futuna Islands', 'Western Sahara', 'Yemen', 'Yugoslavia', 'Zambia', 'Zimbabwe'];
    //selfEdu.SelectedCountry = ko.observable(data.Country);
    
    //if (data.length > 0) {
    //    $.each(data, function (i, obj) {
    if (data.Institution != undefined) {

        selfEdu.Institution = ko.observable(data.Institution);
        selfEdu.Country = ko.observable(data.Country);
        selfEdu.Title = ko.observable(data.Title);
        selfEdu.DateIniE = ko.observable(data.DateIniE);
        selfEdu.DateEndE = ko.observable(data.DateEndE);
        selfEdu.UserId = ko.observable(userid);
        selfEdu.Actually = ko.observable(data.Actually);
            }
            else {
                selfEdu.Institution = ko.observable("");
                selfEdu.Country = ko.observable("");
                selfEdu.Title = ko.observable("");
                selfEdu.DateIniE = ko.observable("");
                selfEdu.DateEndE = ko.observable("");
                selfEdu.UserId = ko.observable(userid);
                selfEdu.Actually = ko.observable(false);
            }

    if (data.Institution2 != undefined) {
        selfEdu.Institution2 = ko.observable(data.Institution2);
        selfEdu.Country2 = ko.observable(data.Country2);
        selfEdu.Title2 = ko.observable(data.Title2);
        selfEdu.DateIni2E = ko.observable(data.DateIni2E);
        selfEdu.DateEnd2E = ko.observable(data.DateEnd2E);
        selfEdu.Actually2 = ko.observable(data.Actually2);
            }
            else {
                selfEdu.Institution2 = ko.observable("");
                selfEdu.Country2 = ko.observable("");
                selfEdu.Title2 = ko.observable("");
                selfEdu.DateIni2E = ko.observable("");
                selfEdu.DateEnd2E = ko.observable("");
                selfEdu.Actually2 = ko.observable(false);
            }


    if (data.Institution3 != undefined) {

        selfEdu.Institution3 = ko.observable(data.Institution3);
        selfEdu.Country3 = ko.observable(data.Country3);
        selfEdu.Title3 = ko.observable(data.Title3);
        selfEdu.DateIni3E = ko.observable(data.DateIni3E);
        selfEdu.DateEnd3E = ko.observable(data.DateEnd3E);
        selfEdu.Actually3 = ko.observable(data.Actually3);
               
            }
            else {
                selfEdu.Institution3 = ko.observable("");
                selfEdu.Country3 = ko.observable("");
                selfEdu.Title3 = ko.observable("");
                selfEdu.DateIni3E = ko.observable("");
                selfEdu.DateEnd3E = ko.observable("");
                selfEdu.Actually3 = ko.observable(data.Actually3);
               
            }
        
    
    //else
    //{
    //    selfEdu.Institution = ko.observable("");
    //    selfEdu.Country = ko.observable("");
    //    selfEdu.Title = ko.observable("");
    //    selfEdu.DateIniE = ko.observable("");
    //    selfEdu.DateEndE = ko.observable("");
    //    selfEdu.UserId = ko.observable(userid);

    //    selfEdu.Institution2 = ko.observable("");
    //    selfEdu.Country2 = ko.observable("");
    //    selfEdu.Title2 = ko.observable("");
    //    selfEdu.DateIni2E = ko.observable("");
    //    selfEdu.DateEnd2E = ko.observable("");


    //    selfEdu.Institution3 = ko.observable("");
    //    selfEdu.Country3 = ko.observable("");
    //    selfEdu.Title3 = ko.observable("");
    //    selfEdu.DateIni3E = ko.observable("");
    //    selfEdu.DateEnd3E = ko.observable("");
        
    //}

    selfEdu.setEducations = function () {
        var jsonData = ko.toJSON(selfEdu);
        $.ajax({
            url: "/Curriculum/InsertEducation",
            type: "POST",
            data: jsonData,
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (returnedData) {
                if (returnedData) {
                    alert("Se ha guardado correctamente la información, continue con la siguiente pestaña");
                }
                else {
                    alert("Error al guardar la información, favor comunicarse con el administrador del sistema");
                }
            }
        });

        //$.post("/Curriculum/InsertEducation", jsonData, function (returnedData) {
        //    if (returnedData.rta) {
        //        alert("Se ha guardado correctamente la información");
        //    }
        //    else {
        //        alert("Error al guardar la información, favor comunicarse con el administrador del sistema");
        //    }
        //})
    };
});

var VieModelHvCertifications = (function (data, userid) {

    selfCert = this;
    //if (data.length > 0) {
    //    $.each(data, function (i, obj) {
    if (data.Certificate != undefined) {
        selfCert.Certificate = ko.observable(data.Certificate);
        selfCert.Otorgante = ko.observable(data.Otorgante);
        selfCert.Description = ko.observable(data.Description);
        selfCert.DateCert = ko.observable(data.DateCert);
        selfCert.Actually = ko.observable(data.Actually);
        selfCert.UserId = ko.observable(userid);

    }
    else {
        selfCert.Certificate = ko.observable("");
        selfCert.Otorgante = ko.observable("");
        selfCert.Description = ko.observable("");
        selfCert.DateCert = ko.observable("");
        selfCert.Actually = ko.observable("");
        selfCert.UserId = ko.observable(userid);


    }
    if (data.Certificate2 != undefined) {
        selfCert.Certificate2 = ko.observable(data.Certificate2);
        selfCert.Otorgante2 = ko.observable(data.Otorgante2);
        selfCert.Description2 = ko.observable(data.Description2);
        selfCert.DateCert2 = ko.observable(data.DateCert2);
        selfCert.Actually2 = ko.observable(data.Actually2);


    }
    else {
        selfCert.Certificate2 = ko.observable("");
        selfCert.Otorgante2 = ko.observable("");
        selfCert.Description2 = ko.observable("");
        selfCert.DateCert2 = ko.observable("");
        selfCert.Actually2 = ko.observable("");



    }

    if (data.Certificate3 != undefined) {
        selfCert.Certificate3 = ko.observable(data.Certificate3);
        selfCert.Otorgante3 = ko.observable(data.Otorgante3);
        selfCert.Description3 = ko.observable(data.Description3);
        selfCert.DateCert3 = ko.observable(data.DateCert3);
        selfCert.Actually3 = ko.observable(data.Actually3);


    }
    else {
        selfCert.Certificate3 = ko.observable("");
        selfCert.Otorgante3 = ko.observable("");
        selfCert.Description3 = ko.observable("");
        selfCert.DateCert3 = ko.observable("");
        selfCert.Actually3 = ko.observable("");



    }


        //});
    //}
    //else
    //{
    //    self.Certificate = ko.observable("");
    //    self.Otorgante = ko.observable("");
    //    self.Description = ko.observable("");
    //    self.DateCert = ko.observable("");
    //    self.Actually = ko.observable("");
    //    self.UserId = ko.observable(userid);

    //    self.Certificate2 = ko.observable("");
    //    self.Otorgante2 = ko.observable("");
    //    self.Description2 = ko.observable("");
    //    self.DateCert2 = ko.observable("");
    //    self.Actually2 = ko.observable("");


    //    self.Certificate3 = ko.observable("");
    //    self.Otorgante3 = ko.observable("");
    //    self.Description3 = ko.observable("");
    //    self.DateCert3 = ko.observable("");
    //    self.Actually3 = ko.observable("");

    //}

  


    selfCert.setCertifications = function () {
        var jsonData = ko.toJSON(selfCert);
        $.ajax({
            url: "/Curriculum/InsertCertificationLab",
            type: "POST",
            data: jsonData,
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (returnedData) {
                if (returnedData) {
                    alert("Se ha guardado correctamente la información, continue con la siguiente pestaña");
                }
                else {
                    alert("Error al guardar la información, favor comunicarse con el administrador del sistema");
                }
            }
        });

        //$.post("/Curriculum/InsertCertificationLab", jsonData, function (returnedData) {
        //    if (returnedData.rta) {
        //        alert("Se ha guardado correctamente la información");
        //    }
        //    else {
        //        alert("Error al guardar la información, favor comunicarse con el administrador del sistema");
        //    }
        //})
    };
});

var VieModelHvLanguages = (function (data, userid) {

    selflan = this;
    
    //if (data.length > 0) {
    //    $.each(data, function (i, obj) {
    if (data.Name != undefined) {
        selflan.Name = ko.observable(data.Name);
        selflan.LevelLang = ko.observable(data.LevelLang);
        selflan.Certificate = ko.observable(data.Certificate);
        selflan.UserId = ko.observable(userid);
    }
    else {
        selflan.Name = ko.observable("");
        selflan.LevelLang = ko.observable("");
        selflan.Certificate = ko.observable("");
        selflan.UserId = ko.observable(userid);

    }

    if (data.Name2 != undefined) {
        selflan.Name2 = ko.observable(data.Name2);
        selflan.LevelLang2 = ko.observable(data.LevelLang2);
        selflan.Certificate2 = ko.observable(data.Certificate2);

    }
    else {
        selflan.Name2 = ko.observable("");
        selflan.LevelLang2 = ko.observable("");
        selflan.Certificate2 = ko.observable("");


    }

    if (data.Name3 != undefined) {
        selflan.Name3 = ko.observable(data.Name3);
        selflan.LevelLang3 = ko.observable(data.LevelLang3);
        selflan.Certificate3 = ko.observable(data.Certificate3);

    }
    else {
        selflan.Name3 = ko.observable("");
        selflan.LevelLang3 = ko.observable("");
        selflan.Certificate3 = ko.observable("");

    }

    //    });
    //}
    //else {
    //    self.Name = ko.observable("");
    //    self.LevelLang = ko.observable("");
    //    self.Certificate = ko.observable("");
    //    self.UserId = ko.observable(userid);

    //    self.Name2 = ko.observable("");
    //    self.LevelLang2 = ko.observable("");
    //    self.Certificate2 = ko.observable("");

    //    self.Name3 = ko.observable("");
    //    self.LevelLang3 = ko.observable("");
    //    self.Certificate3 = ko.observable("");
    //}

    selflan.setLanguages = function () {
        var jsonData = ko.toJSON(selflan);


        $.ajax({
            url: "/Curriculum/InsertLanguageLab",
            type: "POST",
            data: jsonData,
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (returnedData) {
                if (returnedData) {
                    alert("Se ha guardado correctamente la información");
                }
                else {
                    alert("Error al guardar la información, favor comunicarse con el administrador del sistema");
                }
            }
        });

        //$.post("/Curriculum/InsertLanguageLab", jsonData, function (returnedData) {
        //    if (returnedData.rta) {
        //        alert("Se ha guardado correctamente la información");
        //    }
        //    else {
        //        alert("Error al guardar la información, favor comunicarse con el administrador del sistema");
        //    }
        //})
    };
});


VieModelHvEducation.prototype.onSelect = function (ev, ui) {
    this.Country(ui.item.value);
};

VieModelHvEducation.prototype.onSelect2 = function (ev, ui) {
    this.Country2(ui.item.value);
};


VieModelHvEducation.prototype.onSelect3 = function (ev, ui) {
    this.Country3(ui.item.value);
};

function replaceImagenByValue( newvalue, jsonData) {
    var stuff = JSON.parse(jsonData);
    stuff.Imagen = newvalue;
    var result =ko.toJSON(stuff);
    return result;
}


var ViewModelPortfolio = (function (data,userid) {
        selfport = this;
   
        if (data != null) {
            selfport.Id = ko.observable(data.Id);
            selfport.Titulo = ko.observable(data.Titulo);
            selfport.UserId = ko.observable(userid);
            selfport.CategoriaId = ko.observable(data.CategoriaId);
            selfport.Categoria = ko.observable(data.Categoria);
            selfport.Description = ko.observable(data.Description);
            selfport.Imagen = ko.observable(data.Imagen);
            selfport.TermCopy = ko.observable(data.TermCopy);
            selfport.Cliente = ko.observable(data.Cliente);
            selfport.Autor = ko.observable(data.Autor);
            this.filename = ko.observable(data.Imagen);
        }
        else {
            selfport.Id = ko.observable("");
            selfport.Titulo = ko.observable("");
            selfport.UserId = ko.observable(userid);
            selfport.CategoriaId = ko.observable("");
            selfport.Categoria = ko.observable("");
            selfport.Description = ko.observable("");
            selfport.Imagen = ko.observable("");
            selfport.TermCopy = ko.observable(false);
            selfport.Cliente = ko.observable("");
            selfport.Autor = ko.observable("");
            this.filename = ko.observable("");

        }
        


        selfport.updatePortfolios = function () {


            var jsonData = ko.toJSON(selfport);

            jsonData = replaceImagenByValue($("#portfolioimg").attr("src"), jsonData)
            $.ajax({
                url: "/Portfolio/UpdatePortfolio",
                type: "POST",
                data: jsonData,
                contentType: "application/json; charset=utf-8",
                async: false,
                success: function (returnedData) {
                    if (returnedData) {
                        
                        alert("Se ha guardado correctamente la información");

                    }
                    else {
                        alert("Error al guardar la información, favor comunicarse con el administrador del sistema");
                    }
                }
            });

        }
    
   

    selfport.setPortfolios = function () {
        
       
        var jsonData = ko.toJSON(selfport);

        jsonData=  replaceImagenByValue($("#portfolioimg").attr("src"), jsonData)
        $.ajax({
            url: "/Portfolio/InsertPortfolio",
            type: "POST",
            data: jsonData,
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (returnedData) {
                if (returnedData) {
                    $('#addwork_contrat').trigger("reset");
                    alert("Se ha guardado correctamente la información");
                }
                else {
                    alert("Error al guardar la información, favor comunicarse con el administrador del sistema");
                }
            }
        });

       
    };

     selfport.deletePortfolio = function (id) {
        
       
        var jsonData = ko.toJSON(id);

        
        $.ajax({
            url: "/Portfolio/DeletePortfolio",
            type: "POST",
            data: jsonData,
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (returnedData) {
                if (returnedData) {
                    $('#addwork_contrat').trigger("reset");
                    alert("Se ha guardado correctamente la información");
                }
                else {
                    alert("Error al guardar la información, favor comunicarse con el administrador del sistema");
                }
            }
        });

       
    };



});

//var ViewModelCountry2 = function (country) {
//    this.items = ['Afghanistan', 'Albania', 'Algeria', 'American Samoa', 'Andorra', 'Angola', 'Anguilla', 'Antarctica', 'Antigua And Barbuda', 'Argentina', 'Armenia', 'Aruba', 'Australia', 'Austria', 'Azerbaijan', 'Bahamas The', 'Bahrain', 'Bangladesh', 'Barbados', 'Belarus', 'Belgium', 'Belize', 'Benin', 'Bermuda', 'Bhutan', 'Bolivia', 'Bosnia and Herzegovina', 'Botswana', 'Bouvet Island', 'Brazil', 'British Indian Ocean Territory', 'Brunei', 'Bulgaria', 'Burkina Faso', 'Burundi', 'Cambodia', 'Cameroon', 'Canada', 'Cape Verde', 'Cayman Islands', 'Central African Republic', 'Chad', 'Chile', 'China', 'Christmas Island', 'Cocos (Keeling) Islands', 'Colombia', 'Comoros', 'Congo', 'Congo The Democratic Republic Of The', 'Cook Islands', 'Costa Rica', 'Cote DIvoire (Ivory Coast)', 'Croatia (Hrvatska)', 'Cuba', 'Cyprus', 'Czech Republic', 'Denmark', 'Djibouti', 'Dominica', 'Dominican Republic', 'East Timor', 'Ecuador', 'Egypt', 'El Salvador', 'Equatorial Guinea', 'Eritrea', 'Estonia', 'Ethiopia', 'External Territories of Australia', 'Falkland Islands', 'Faroe Islands', 'Fiji Islands', 'Finland', 'France', 'French Guiana', 'French Polynesia', 'French Southern Territories', 'Gabon', 'Gambia The', 'Georgia', 'Germany', 'Ghana', 'Gibraltar', 'Greece', 'Greenland', 'Grenada', 'Guadeloupe', 'Guam', 'Guatemala', 'Guernsey and Alderney', 'Guinea', 'Guinea-Bissau', 'Guyana', 'Haiti', 'Heard and McDonald Islands', 'Honduras', 'Hong Kong S.A.R.', 'Hungary', 'Iceland', 'India', 'Indonesia', 'Iran', 'Iraq', 'Ireland', 'Israel', 'Italy', 'Jamaica', 'Japan', 'Jersey', 'Jordan', 'Kazakhstan', 'Kenya', 'Kiribati', 'Korea North', 'Korea South', 'Kuwait', 'Kyrgyzstan', 'Laos', 'Latvia', 'Lebanon', 'Lesotho', 'Liberia', 'Libya', 'Liechtenstein', 'Lithuania', 'Luxembourg', 'Macau S.A.R.', 'Macedonia', 'Madagascar', 'Malawi', 'Malaysia', 'Maldives', 'Mali', 'Malta', 'Man (Isle of)', 'Marshall Islands', 'Martinique', 'Mauritania', 'Mauritius', 'Mayotte', 'Mexico', 'Micronesia', 'Moldova', 'Monaco', 'Mongolia', 'Montserrat', 'Morocco', 'Mozambique', 'Myanmar', 'Namibia', 'Nauru', 'Nepal', 'Netherlands Antilles', 'Netherlands The', 'New Caledonia', 'New Zealand', 'Nicaragua', 'Niger', 'Nigeria', 'Niue', 'Norfolk Island', 'Northern Mariana Islands', 'Norway', 'Oman', 'Pakistan', 'Palau', 'Palestinian Territory Occupied', 'Panama', 'Papua new Guinea', 'Paraguay', 'Peru', 'Philippines', 'Pitcairn Island', 'Poland', 'Portugal', 'Puerto Rico', 'Qatar', 'Reunion', 'Romania', 'Russia', 'Rwanda', 'Saint Helena', 'Saint Kitts And Nevis', 'Saint Lucia', 'Saint Pierre and Miquelon', 'Saint Vincent And The Grenadines', 'Samoa', 'San Marino', 'Sao Tome and Principe', 'Saudi Arabia', 'Senegal', 'Serbia', 'Seychelles', 'Sierra Leone', 'Singapore', 'Slovakia', 'Slovenia', 'Smaller Territories of the UK', 'Solomon Islands', 'Somalia', 'South Africa', 'South Georgia', 'South Sudan', 'Spain', 'Sri Lanka', 'Sudan', 'Suriname', 'Svalbard And Jan Mayen Islands', 'Swaziland', 'Sweden', 'Switzerland', 'Syria', 'Taiwan', 'Tajikistan', 'Tanzania', 'Thailand', 'Togo', 'Tokelau', 'Tonga', 'Trinidad And Tobago', 'Tunisia', 'Turkey', 'Turkmenistan', 'Turks And Caicos Islands', 'Tuvalu', 'Uganda', 'Ukraine', 'United Arab Emirates', 'United Kingdom', 'United States', 'United States Minor Outlying Islands', 'Uruguay', 'Uzbekistan', 'Vanuatu', 'Vatican City State (Holy See)', 'Venezuela', 'Vietnam', 'Virgin Islands (British)', 'Virgin Islands (US)', 'Wallis And Futuna Islands', 'Western Sahara', 'Yemen', 'Yugoslavia', 'Zambia', 'Zimbabwe'];
//    this.Country2 = ko.observable(country);

//};

//ViewModelCountry2.prototype.onSelect2 = function (ev, ui) {
//    this.Country2(ui.item.value);
//};

//var ViewModelCountry3 = function (country) {
//    this.items = ['Afghanistan', 'Albania', 'Algeria', 'American Samoa', 'Andorra', 'Angola', 'Anguilla', 'Antarctica', 'Antigua And Barbuda', 'Argentina', 'Armenia', 'Aruba', 'Australia', 'Austria', 'Azerbaijan', 'Bahamas The', 'Bahrain', 'Bangladesh', 'Barbados', 'Belarus', 'Belgium', 'Belize', 'Benin', 'Bermuda', 'Bhutan', 'Bolivia', 'Bosnia and Herzegovina', 'Botswana', 'Bouvet Island', 'Brazil', 'British Indian Ocean Territory', 'Brunei', 'Bulgaria', 'Burkina Faso', 'Burundi', 'Cambodia', 'Cameroon', 'Canada', 'Cape Verde', 'Cayman Islands', 'Central African Republic', 'Chad', 'Chile', 'China', 'Christmas Island', 'Cocos (Keeling) Islands', 'Colombia', 'Comoros', 'Congo', 'Congo The Democratic Republic Of The', 'Cook Islands', 'Costa Rica', 'Cote DIvoire (Ivory Coast)', 'Croatia (Hrvatska)', 'Cuba', 'Cyprus', 'Czech Republic', 'Denmark', 'Djibouti', 'Dominica', 'Dominican Republic', 'East Timor', 'Ecuador', 'Egypt', 'El Salvador', 'Equatorial Guinea', 'Eritrea', 'Estonia', 'Ethiopia', 'External Territories of Australia', 'Falkland Islands', 'Faroe Islands', 'Fiji Islands', 'Finland', 'France', 'French Guiana', 'French Polynesia', 'French Southern Territories', 'Gabon', 'Gambia The', 'Georgia', 'Germany', 'Ghana', 'Gibraltar', 'Greece', 'Greenland', 'Grenada', 'Guadeloupe', 'Guam', 'Guatemala', 'Guernsey and Alderney', 'Guinea', 'Guinea-Bissau', 'Guyana', 'Haiti', 'Heard and McDonald Islands', 'Honduras', 'Hong Kong S.A.R.', 'Hungary', 'Iceland', 'India', 'Indonesia', 'Iran', 'Iraq', 'Ireland', 'Israel', 'Italy', 'Jamaica', 'Japan', 'Jersey', 'Jordan', 'Kazakhstan', 'Kenya', 'Kiribati', 'Korea North', 'Korea South', 'Kuwait', 'Kyrgyzstan', 'Laos', 'Latvia', 'Lebanon', 'Lesotho', 'Liberia', 'Libya', 'Liechtenstein', 'Lithuania', 'Luxembourg', 'Macau S.A.R.', 'Macedonia', 'Madagascar', 'Malawi', 'Malaysia', 'Maldives', 'Mali', 'Malta', 'Man (Isle of)', 'Marshall Islands', 'Martinique', 'Mauritania', 'Mauritius', 'Mayotte', 'Mexico', 'Micronesia', 'Moldova', 'Monaco', 'Mongolia', 'Montserrat', 'Morocco', 'Mozambique', 'Myanmar', 'Namibia', 'Nauru', 'Nepal', 'Netherlands Antilles', 'Netherlands The', 'New Caledonia', 'New Zealand', 'Nicaragua', 'Niger', 'Nigeria', 'Niue', 'Norfolk Island', 'Northern Mariana Islands', 'Norway', 'Oman', 'Pakistan', 'Palau', 'Palestinian Territory Occupied', 'Panama', 'Papua new Guinea', 'Paraguay', 'Peru', 'Philippines', 'Pitcairn Island', 'Poland', 'Portugal', 'Puerto Rico', 'Qatar', 'Reunion', 'Romania', 'Russia', 'Rwanda', 'Saint Helena', 'Saint Kitts And Nevis', 'Saint Lucia', 'Saint Pierre and Miquelon', 'Saint Vincent And The Grenadines', 'Samoa', 'San Marino', 'Sao Tome and Principe', 'Saudi Arabia', 'Senegal', 'Serbia', 'Seychelles', 'Sierra Leone', 'Singapore', 'Slovakia', 'Slovenia', 'Smaller Territories of the UK', 'Solomon Islands', 'Somalia', 'South Africa', 'South Georgia', 'South Sudan', 'Spain', 'Sri Lanka', 'Sudan', 'Suriname', 'Svalbard And Jan Mayen Islands', 'Swaziland', 'Sweden', 'Switzerland', 'Syria', 'Taiwan', 'Tajikistan', 'Tanzania', 'Thailand', 'Togo', 'Tokelau', 'Tonga', 'Trinidad And Tobago', 'Tunisia', 'Turkey', 'Turkmenistan', 'Turks And Caicos Islands', 'Tuvalu', 'Uganda', 'Ukraine', 'United Arab Emirates', 'United Kingdom', 'United States', 'United States Minor Outlying Islands', 'Uruguay', 'Uzbekistan', 'Vanuatu', 'Vatican City State (Holy See)', 'Venezuela', 'Vietnam', 'Virgin Islands (British)', 'Virgin Islands (US)', 'Wallis And Futuna Islands', 'Western Sahara', 'Yemen', 'Yugoslavia', 'Zambia', 'Zimbabwe'];
//    this.Country3 = ko.observable(country);

//};

//ViewModelCountry3.prototype.onSelect3 = function (ev, ui) {
//    this.Country3(ui.item.value);
//};










