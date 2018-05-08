var CascadingDDLViewModel = (function (countryCode, citycode) {
    var selfcoutry = this;
    selfcoutry.countrieses = ko.observableArray([]);
    selfcoutry.states = ko.observableArray([]);
   
     var Country = function (name, id) {
         this.CountryName = name;
         this.CountryID = id

    };


    var City = function (name, id) {
        this.Name = name;
        this.Id=id

    };

     var arraycountry = [];
     $.getJSON("/Country2/Get?SelectedCountryId="+countryCode, null, function (datacountry) {
         
         $.each(datacountry, function (index, value) {
             arraycountry.push(new Country(value.CountryName, value.CountryID));
            
        });
         selfcoutry.countrieses(arraycountry);
    });

    var arraycity = [];
    $.getJSON("/City2/Get/" + countryCode, null, function (data) {

            $.each(data, function (index, value) {
            arraycity.push(new City(value.Name, value.Id));
            
        });
         selfcoutry.states(arraycity);
    });

    selfcoutry.fetchStates = function () {
        arraycity.length = 0
        var countryCode = $("#Country").val();
        $.getJSON("/City2/Get/" + countryCode, null, function (data) {


            $.each(data, function (index, value) {
                arraycity.push(new City(value.Name, value.Id));

            });



            selfcoutry.states(arraycity);
        });

    }

    
});




