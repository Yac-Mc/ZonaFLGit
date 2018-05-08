var app = angular.module('ZonaFl', []);
app.controller('DropdownController', function ($scope, $http) {
    GetCountries();


    function GetCountries() {
        $http({
            method: 'Get',
            url: '/Api/Country'
        }).success(function (data, status, headers, config) {
            $scope.countries = data.Data;
            //$scoipe.selectedConuntry = data.selected;
            
        }).error(function (data, status, headers, config) {
            $scope.message = 'Unexpected Error';
        });
    }

    $scope.GetCities = function () {
        var countryId = $scope.country;
        if (countryId) {
            $http({
                method: 'Get',
                url: '/Api/City/' + countryId,
                data: null
            }).success(function (data, status, headers, config) {
                $scope.cities = data.Data;
            }).error(function (data, status, headers, config) {
                $scope.message = 'Unexpected Error';
            });
        }
        else {
            $scope.states = null;
        }
    }
});

app.controller('DropdownControllerEdit', function ($scope, $http) {
    $scope.idcountry = "47";
    //$scope.nameCountry = function (idcountry) {
       
    //    $scope.idcountry = idcountry;
    //    GetCountries();
    //};
  
    GetCountries();
   

    function GetCountries() {
        $http({
            method: 'Get',
            url: '/Api/Country/GetEdit?SelectedCountryId=' + $scope.idcountry,
        }).success(function (data, status, headers, config) {
            $scope.idcountry = 47;
            $scope.register = {};
            $scope.register.selectedId = $scope.idcountry;
            $scope.register.countries = data.Data;
           
           

        }).error(function (data, status, headers, config) {
            $scope.message = 'Unexpected Error';
        });
    }
    
    $scope.GetCities = function () {
        var countryId = $scope.country;
        if (countryId) {
            $http({
                method: 'Get',
                url: '/Api/City/' + countryId,
                data: null
            }).success(function (data, status, headers, config) {
                $scope.cities = data.Data;
            }).error(function (data, status, headers, config) {
                $scope.message = 'Unexpected Error';
            });
        }
        else {
            $scope.states = null;
        }
    }
});
