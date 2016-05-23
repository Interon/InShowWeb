
angular.module('RDash').factory("fMembership", function ($resource, $http, $q, toaster, ngDialog) {
    return {
        CurrentMemberAsync: $resource('/umbraco/api/members/GetCurrentMember', {}, {
            query: { method: 'GET', params: {}, isArray: false }
        }), 
        CurrentMember: function () {
            var defer = $q.defer();
            $http.get('/umbraco/api/members/GetCurrentMember', { cache: 'true' })
           .success(function (data) {

               defer.resolve(data);

           });
            
            return defer.promise;
        },

        UpdateMember: function (id, parameters) {
            $http.post('/umbraco/api/members/UpdateMember/' + id, parameters).
        success(function (data, status, headers, config) {

            toaster.pop('success', "", "Save Successfull");

        }).
       error(function (data, status, headers, config) {
           toaster.pop('error', "", "Error Update Member");
       });
        },

        GetMemberByEmail: function (email, parameters) {
            $http.post('/umbraco/api/members/GetMemberByEmail/' + email, parameters).
        success(function (data, status, headers, config) {
            
            toaster.pop('success', "", "Save Successfull");

        }).
       error(function (data, status, headers, config) {
           toaster.pop('error', "", "Error Update Member");
       });
        },

        ChangePassword: function (id, parameters) {

            $http.post('/umbraco/api/members/ChangePassword/' + id, parameters).
        success(function (data, status, headers, config) {


            toaster.pop('success', "", "Save Successfull");
            ngDialog.close();

        }).
       error(function (data, status, headers, config) {
           toaster.pop('error', "", "Error Update Member ->" + status);
       });
        },

        AddAgent: function (id, parameters) {

            $http.post('/umbraco/api/members/AddAgent/' + id, parameters).
        success(function (data, status, headers, config) {

            toaster.pop('success', "", "Save Successfull");
            ngDialog.close();

        }).
       error(function (data, status, headers, config) {
           toaster.pop('error', "", "Error Update Member ->" + status);
       });
        },

        UpdateMemberPassword: function (id, parameters) {
            $http.post('/umbraco/api/members/UpdateMemberPassword/' + id, parameters).
        success(function (data, status, headers, config) {


            toaster.pop('success', "", "Save Successfull");

        }).
       error(function (data, status, headers, config) {
           toaster.pop('error', "", "Error Update Member");
       });
        },

        countries: $resource('../data/countries.json', {}, {
            query: { method: 'GET', params: {}, isArray: false }
        })
    };
});

angular.module('RDash').factory('fMember', ['fMembership', function (fMembership, $scope) {


    var member = { Id: 0, Type: "" };
    fMembership.CurrentMember().then(function (data) {

        var _member = data;
        member.Id = _member.Id;
        member.Type = _member.ContentTypeAlias;
        member.Name = _member.Name;
        member.Username = _member.Username;
        // member.Properties = _member.Properties;
        var myarray = [];
        myarray = _member.Properties.$values;



        for (var i = 0; i < myarray.length; i++) {
            console.log(myarray[i].Alias + '->' + myarray[i].Value);
            var myObj = new Object;
            myObj[myarray[i].Alias] = myarray[i].Value;
            angular.extend(member, myObj)
        };



        return member;

    });

    return member;


}]);





