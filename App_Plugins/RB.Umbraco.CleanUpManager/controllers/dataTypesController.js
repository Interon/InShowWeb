(function () {


    var dataTypesController = function ($scope,
                                        $filter,
                                        $timeout,
                                        notificationsService,
                                        dataTypesService) {

        $scope.totalDataTypeRecords = 0;
        $scope.totalDataTypePages = 0;
        $scope.dataTypePageSize = 10;
        $scope.dataTypeCurrentPage = 1;
        $scope.dataTypes = [];
        $scope.orderby = "PropertyEditorAlias";
        $scope.reverse = false;
        $scope.dataTypeSearchText = "";
        $scope.cardAnimationClass = "card-animation";

        var getDataTypes = function () {
            dataTypesService.getDataTypes($scope.dataTypeCurrentPage - 1, $scope.dataTypePageSize, $scope.dataTypeSearchText)
            .then(function (data) {
                $scope.totalDataTypeRecords = data.totalRecords;
                $scope.totalDataTypePages = Math.ceil(data.totalRecords / $scope.dataTypePageSize);
                $scope.dataTypes = data.results;
                if ($scope.dataTypes.length === 0) {
                    $scope.dataTypeCurrentPage = 0;
                }
            }, function (e) {
                console.log(e);
                notificationsService.success("CleanUp Manager", "Error:  " + e);
            });
        }

        $scope.firstPage = function () {
            if (($scope.dataTypeCurrentPage - 1) >= 1) {
                $scope.dataTypeCurrentPage = 1;
                getDataTypes();
            }
        };

        $scope.previousPage = function () {
            if (($scope.dataTypeCurrentPage - 1) >= 1) {
                $scope.dataTypeCurrentPage--;
                getDataTypes();
            }
        };

        $scope.nextPage = function () {
            if (($scope.dataTypeCurrentPage + 1) <= $scope.totalDataTypePages) {
                $scope.dataTypeCurrentPage++;
                getDataTypes();
            }
        };

        $scope.lastPage = function () {
            if (($scope.dataTypeCurrentPage + 1) <= $scope.totalDataTypePages) {
                $scope.dataTypeCurrentPage = $scope.totalDataTypePages;
                getDataTypes();
            }
        };

        $scope.deleteDataTypes = function () {
            if (confirm("Are you sure you want to delete all data types?")) {
                dataTypesService.deleteDataTypes().then(function () {
                    getDataTypes();
                }, function (e) {
                    console.log(e);
                    notificationsService.success("CleanUp Manager", "Error:  " + e);
                });
            }
        };

        $scope.deleteDataType = function (id, alias) {
            if (confirm("Are you sure you want to delete " + alias + " data type?")) {
                dataTypesService.deleteDataType(id).then(function (response) {
                    console.log(response);
                    getDataTypes();
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
            $scope.dataTypesSearchForm.$setPristine();
            $scope.dataTypeCurrentPage = 1;
            getDataTypes();
        };

        $scope.clearSearch = function () {
            $scope.dataTypeSearchText = "";
            $scope.dataTypesSearchForm.$setPristine();
            $scope.dataTypeCurrentPage = 1;
            getDataTypes();
        };



        $scope.refreshDataTypes = function () {
            $scope.dataTypeCurrentPage = 1;
            getDataTypes();
        };


        $scope.showNode = function (nodeId) {
            window.location = "/umbraco#/developer/datatype/edit/" + nodeId;
        };

        function init() {
            getDataTypes();
        }

        init();

        var toggleChevron = function (e) {
            $(e.target).prev('.panel-heading')
                       .find("i.indicator")
                       .toggleClass('icon-navigation-bottom icon-navigation-top');
        }

        $('#accordion').on('hidden.bs.collapse', toggleChevron);
        $('#accordion').on('shown.bs.collapse', toggleChevron);


    };

    dataTypesController.$inject = ["$scope",
                                   "$filter",
                                   "$timeout",
                                   "notificationsService",
                                   "RB.Umbraco.CleanUpManager.DataTypesService"];

    angular.module("umbraco").controller("RB.Umbraco.CleanUpManager.DataTypesController", dataTypesController);

}());
