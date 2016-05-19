angular.module('RDash').controller('AddAgentController', ['ngDialog', '$scope', 'fMembership', 'fMember', function (ngDialog, $scope, fMembership, fMember) {
    //TODO close modal after save 
    //Check email API 
    $scope.member = {};
    $scope.submit = function (member) {

        debugger;
        fMembership.AddAgent(fMember.Id, angular.toJson(member));

    }

    $scope.memberfields = [

      {
          key: 'emailAddress',
          type: 'input',
          templateOptions: {
              label: 'Email Address',
              placeholder: 'Email Address'
          }
      },

      {
          key: 'username',
          type: 'input',
          templateOptions: {
              label: 'User Name',
              placeholder: 'User Name'
          }
      },

      {
          key: 'firstName',
          type: 'input',
          templateOptions: {
              label: 'First Name',
              placeholder: 'First Name'
          }
      },

      {
          key: 'lastName',
          type: 'input',
          templateOptions: {
              label: 'Last Name',
              placeholder: 'Last Name'
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
          key: 'password',
          type: 'input',
          templateOptions: {
              type: 'password',
              label: 'Password',
              placeholder: 'Password'
          }
      },

      {
          key: 'confirmPassword',
          type: 'input',
          templateOptions: {
              type: 'password',
              label: 'Confirm Password',
              placeholder: 'Confirm Password'
          }
      },



    ]



}]);