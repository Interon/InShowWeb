angular.module('RDash').controller('ngCropUpload', ['$scope', 'Upload', '$timeout', function ($scope, Upload, $timeout) {
    $scope.upload = function (dataUrl, name) {
        Upload.upload({
            url: '/umbraco/api/FileApi/Upload',
            data: {
                file: Upload.dataUrltoBlob(dataUrl, name)
            },
        }).then(function (response) {
            $timeout(function () {
                $scope.result = response.data;
            });
        }, function (response) {
            if (response.status > 0) $scope.errorMsg = response.status
                + ': ' + response.data;
        }, function (evt) {
            $scope.progress = parseInt(100.0 * evt.loaded / evt.total);
        });
    }
}]);