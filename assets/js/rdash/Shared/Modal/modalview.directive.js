angular.module('RDash').directive('modaluploader', function () {
    var directive = {
        scope: {
            Type: "@type",
            NodeId: "@nodeid",
            PropertyAlias: "@propertyalias",
            DataType: "@datatype"

        },

        controller: 'ModalViewCtrl',
        templateUrl: '/templates/ModalFileUpload.html',
        restrict: 'E'
    };
    return directive;
});