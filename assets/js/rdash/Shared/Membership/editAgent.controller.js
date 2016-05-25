angular.module('RDash').controller('EditAgentController', ['ngDialog', '$scope', 'fMembership', 'fMember', function (ngDialog, $scope, fMembership, fMember) {
    //TODO Add Confirm Password block and 

    $scope.member = {};
    $scope.submit = function (member) {

        fMembership.ChangePassword(fMember.Id, angular.toJson(member));

    }

    $scope.memberfields = [

      {
          key: 'agencyName',
          type: 'input',
          templateOptions: {
              label: 'Agency Name',
              placeholder: 'Agency Name'
          }
      },

      {
          key: 'agencyPin',
          type: 'input',
          templateOptions: {
              label: 'Agency Pin',
              placeholder: 'Agency Pin'
          }
      },

      {
          key: 'cellNumber',
          type: 'input',
          templateOptions: {
              label: 'Cell Number',
              placeholder: 'Cell Number'
          }
      },

      {
          key: 'address',
          type: 'input',
          templateOptions: {
              label: 'Address Users',
              placeholder: 'Adress Users'
          }
      },

    ]



}]);