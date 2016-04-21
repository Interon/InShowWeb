(function () {

    var dataTypesService = function ($http) {

        var serviceBase = "backoffice/RBCleanUpManager/CleanUpManagerApi/",
            factory = {};

        factory.getDataTypes = function (pageIndex, pageSize, filter) {
            return getPagedResource("GetOrphanDataTypes", pageIndex, pageSize, filter);
        };

        factory.deleteDataType = function (id) {
            return $http.post(serviceBase + "DeleteOrphanDataType",
                              JSON.stringify(id),
                              { headers: { 'Content-Type': 'application/json' } }).then(function (data) {
                                  console.log(data);
                              });
        };

        factory.deleteDataTypes = function () {
            return $http.post(serviceBase + "DeleteOrphanDataTypes").then(function (status) {
                return status.data;
            });
        };

        function getPagedResource(baseResource, pageIndex, pageSize, filter) {
            var resource = baseResource;
            resource += (arguments.length === 4) ? buildPagingUri(pageIndex, pageSize, filter) : "";
            return $http.get(serviceBase + resource).then(function (response) {
                var results = response.data;
                return {
                    totalRecords: results.TotalCount,
                    results: results.List
                };
            });
        }

        function buildPagingUri(pageIndex, pageSize, filter) {
            var uri = "?pageSize=" + pageSize + "&pageIndex=" + pageIndex;
            if (filter !== undefined && filter !== "") {
                uri = uri + "&filter=" + filter;
            }
            return uri;
        }
        return factory;
    };

    dataTypesService.$inject = ["$http"];

    angular.module("umbraco").factory("RB.Umbraco.CleanUpManager.DataTypesService", dataTypesService);

}());