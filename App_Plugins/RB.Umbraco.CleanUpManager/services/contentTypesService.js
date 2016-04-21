(function () {

    var contentTypesService = function ($http) {

        var serviceBase = "backoffice/RBCleanUpManager/CleanUpManagerApi/",
            factory = {};

        factory.getContentTypes = function (pageIndex, pageSize, filter) {
            return getPagedResource("GetOrphanContentTypes", pageIndex, pageSize, filter);
        };

        factory.deleteContentType = function (id) {
            return $http.post(serviceBase + "DeleteOrphanContentType",
                              JSON.stringify(id),
                              { headers: { 'Content-Type': 'application/json' } }).then(function (data) {
                                  console.log(data);
                              });
        };

        factory.deleteContentTypes = function () {
            return $http.post(serviceBase + "DeleteOrphanContentTypes").then(function (status) {
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

    contentTypesService.$inject = ["$http"];

    angular.module("umbraco").factory("RB.Umbraco.CleanUpManager.ContentTypesService", contentTypesService);

}());