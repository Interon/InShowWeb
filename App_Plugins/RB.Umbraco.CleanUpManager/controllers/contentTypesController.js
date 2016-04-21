(function () {


    var contentTypesController = function ($scope,
                                        $filter,
                                        $timeout,
                                        notificationsService,
                                        contentTypesService) {

        $scope.totalContentTypeRecords = 0;
        $scope.totalContentTypePages = 0;
        $scope.contentTypePageSize = 10;
        $scope.contentTypeCurrentPage = 1;
        $scope.contentTypes = [];
        $scope.orderby = "Alias";
        $scope.reverse = false;
        $scope.contentTypeSearchText = "";
        $scope.cardAnimationClass = "card-animation";

        var getContentTypes = function () {
            contentTypesService.getContentTypes($scope.contentTypeCurrentPage - 1, $scope.contentTypePageSize, $scope.contentTypeSearchText)
            .then(function (data) {
                $scope.totalContentTypeRecords = data.totalRecords;
                $scope.totalContentTypePages = Math.ceil(data.totalRecords / $scope.contentTypePageSize);
                $scope.contentTypes = data.results;
                if ($scope.contentTypes.length === 0) {
                    $scope.contentTypeCurrentPage = 0;
                }
            }, function (e) {
                console.log(e);
                notificationsService.error("CleanUp Manager", "Error:  " + e);
            });
        }

        $scope.firstPage = function () {
            if (($scope.contentTypeCurrentPage - 1) >= 1) {
                $scope.contentTypeCurrentPage = 1;
                getContentTypes();
            }
        };

        $scope.previousPage = function () {
            if (($scope.contentTypeCurrentPage - 1) >= 1) {
                $scope.contentTypeCurrentPage--;
                getContentTypes();
            }
        };

        $scope.nextPage = function () {
            if (($scope.contentTypeCurrentPage + 1) <= $scope.totalContentTypePages) {
                $scope.contentTypeCurrentPage++;
                getContentTypes();
            }
        };

        $scope.lastPage = function () {
            if (($scope.contentTypeCurrentPage + 1) <= $scope.totalContentTypePages) {
                $scope.contentTypeCurrentPage = $scope.totalContentTypePages;
                getContentTypes();
            }
        };

        $scope.deleteContentTypes = function () {
            if (confirm("Are you sure you want to delete all data types?")) {
                contentTypesService.deleteContentTypes().then(function () {
                    getContentTypes();
                }, function (e) {
                    console.log(e);
                    notificationsService.success("CleanUp Manager", "Error:  " + e);
                });
            }
        };

        $scope.deleteContentType = function (id, alias) {
            if (confirm("Are you sure you want to delete " + alias + " data type?")) {
                contentTypesService.deleteContentType(id).then(function (response) {
                    console.log(response);
                    getContentTypes();
                }, function (e) {
                    console.log(e);
                    notificationsService.success("CleanUp Manager", "Error:  " + e);
                });
            }
        };


        $scope.setOrder = function (orderby) {
            // Need to implement this on the server
            //if (orderby === $scope.orderby) {
            //    $scope.reverse = !$scope.reverse;
            //}
            //$scope.orderby = orderby;
        };

        $scope.search = function () {
            $scope.contentTypesSearchForm.$setPristine();
            $scope.contentTypeCurrentPage = 1;
            getContentTypes();
        };

        $scope.clearSearch = function () {
            $scope.contentTypeSearchText = "";
            $scope.contentTypesSearchForm.$setPristine();
            $scope.contentTypeCurrentPage = 1;
            getContentTypes();
        };

        $scope.refreshContentTypes = function () {
            $scope.contentTypeCurrentPage = 1;
            getContentTypes();
        };

        $scope.showNode = function (id) {
            window.location = "/umbraco#/settings/framed/%252Fumbraco%252Fsettings%252FeditNodeTypeNew.aspx%253Fid%253D" + id;
        };

        function init() {
            getContentTypes();
        }

        init();
    };

    contentTypesController.$inject = ["$scope",
                                      "$filter",
                                      "$timeout",
                                      "notificationsService",
                                      "RB.Umbraco.CleanUpManager.ContentTypesService"];

    angular.module("umbraco").controller("RB.Umbraco.CleanUpManager.ContentTypesController", contentTypesController);

}());
