angular.module('RDash').factory("appService", function () {
    var isEmpty = function (obj) {
        var name;
        for (name in obj) {
            return false;
        }
        return true;
    };
    function ccc(obj1, obj2) {
      
        if (obj1 === null) {
            return obj2;
        }
        if (obj2 === null) {
            return obj1;
        }
        var ret = {}, rett;
        for (var i in obj2) {
            rett = {};
            if (typeof obj2[i] === 'object') {
                rett = ccc(obj1[i], obj2[i]);
                if (!isEmpty(rett)) {
                    ret[i] = rett
                }
            } else {
                if (!obj1 || !obj1.hasOwnProperty(i) || obj2[i] !== obj1[i]) {
                    ret[i] = obj2[i];
                }
            }
        }
        return ret;
    }
    return {
        compareJsonObjects: function (obj1, obj2) {

            var ret = {}, rett;
            for (var i in obj2) {
                rett = {};
                if (typeof obj2[i] === 'object') {
                   
                    rett = ccc(obj1[i], obj2[i]);
                    if (!isEmpty(rett)) {
                        ret[i] = rett
                    }
                } else {
                    if (!obj1 || !obj1.hasOwnProperty(i) || obj2[i] !== obj1[i]) {
                        ret[i] = obj2[i];
                    }
                }
            }
            return ret;
        }
    }
});