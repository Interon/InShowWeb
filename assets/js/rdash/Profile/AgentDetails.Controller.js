angular.module('RDash').controller('agentdetailscontroller', ['$scope', 'fMember', 'appService', 'fMembership','ngDialog', function ($scope, fMember, appService, fMembership,ngDialog) {


    $scope.multifile = true;
    $scope.ChangePassword = function () {
       
        ngDialog.open({
            template: '/assets/js/rdash/Shared/Membership/PasswordChange.view.html',
            className: 'ngdialog-theme-plain'
        });
        
    }


    $scope.AddAgent = function () {
        //$rootScope.theme = 'ngdialog-theme-plain custom-width';

        ngDialog.open({
            template: '/assets/js/rdash/Shared/Membership/addAgent.view.html',
            className: 'ngdialog-theme-plain custom-width',
            closeByDocument: false
        });
    };


    $scope.ImageUploadClick = function () {
       
        ngDialog.open({
            template: '/assets/js/rdash/Shared/FileUpload/fileUpload.view.html',
            className: 'ngdialog-theme-plain',
            scope: $scope,
            
        });
        
    }




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