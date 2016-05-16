angular.module('RDash').controller('agentdetailscontroller', ['$scope', 'fMember', 'appService', 'fMembership', function ($scope, fMember, appService, fMembership) {

  
        $scope.member = fMember;
        //$scope.updateUser = function (data) {
        //    debugger;
        //    return $http.post('/updateUser', { id: $scope.user.id, name: data });
        //};
        $scope.$watch(
                    "member",
                    function handleMemberChange(newValue, oldValue) {
                        if (newValue.Id != 0 && oldValue.Id != 0) {
                            if (!angular.equals(newValue, oldValue)) {
                                var newJson = angular.toJson(newValue);
                                var oldJson = angular.toJson(oldValue);
                               
                                var difference = appService.compareJsonObjects(oldValue, newValue)
                                fMembership.UpdateMember(oldValue.Id, difference);
                            }
                        }
                        
                       
                       
                    }
                ,true);
      
}]);