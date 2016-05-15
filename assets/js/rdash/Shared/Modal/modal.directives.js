angular.module('RDash').directive('upldr', function () {
    var directive = {

        scope: {

            Type: "@",
            NodeId: "@",
            PropertyAlias: "@",
            DataType: "@"

        },

        templateUrl: '/templates/FileUpload.html',
        restrict: 'E'
    };
    return directive;
});






angular.module('RDash').directive('modalview', function () {


    var directive = {
        scope: {
            memberkey: $scope.parent.memberkey,
            url: "=url"

        },


        templateUrl: url,
        restrict: 'E'
    };
    return directive;
});



angular.module('RDash').directive('modalviewbutton', function () {

    var directive = {

        scope: {

            Type: "=",
            NodeId: "=",
            PropertyAlias: "=",
            DataType: "="

        },

        templateUrl: '/templates/ModalView.html',
        restrict: 'E'
    };
    return directive;
});

