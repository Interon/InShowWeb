angular.module('RDash').controller('PasswordChangeController', ['ngDialog', '$scope', 'fMembership', 'fMember', function (ngDialog, $scope, fMembership, fMember) {

    $scope.member = {};
    $scope.submit = function (member) {

        fMembership.ChangePassword(fMember.Id, angular.toJson(member));

    }

    $scope.memberfields = [

      {
          key: 'password',
          type: 'input',
          templateOptions: {
              type: 'password',
              label: 'New Password',
              placeholder: 'Password'
          }
      },

    ]



}]);