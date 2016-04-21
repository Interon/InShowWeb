(function () {

    var contentTypesFilter = function () {

        return function (contentTypes, filterValue) {
            if (!filterValue) return contentTypes;

            var matches = [];
            filterValue = filterValue.toLowerCase();
            for (var i = 0; i < contentTypes.length; i++) {
                var contentType = contentTypes[i];
                if (contentType.Alias.toLowerCase().indexOf(filterValue) > -1 ||
                    contentType.Description.toLowerCase().indexOf(filterValue) > -1) {

                    matches.push(contentType);
                }
            }
            return matches;
        };
    };

    angular.module("umbraco").filter("contentTypesFilter", contentTypesFilter);

}());