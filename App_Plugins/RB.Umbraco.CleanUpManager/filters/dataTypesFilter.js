(function () {

    var dataTypesFilter = function () {

        return function (dataTypes, filterValue) {
            if (!filterValue) return dataTypes;

            var matches = [];
            filterValue = filterValue.toLowerCase();
            for (var i = 0; i < dataTypes.length; i++) {
                var dataType = dataTypes[i];
                if (dataType.PropertyEditorAlias.toLowerCase().indexOf(filterValue) > -1 ||
                    dataType.DbType.toLowerCase().indexOf(filterValue) > -1) {

                    matches.push(dataType);
                }
            }
            return matches;
        };
    };

    angular.module("umbraco").filter("dataTypesFilter", dataTypesFilter);

}());