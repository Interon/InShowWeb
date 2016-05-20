angular.module('RDash').controller('AddAgentController', ['ngDialog', '$scope', 'fMembership', 'fMember', function (ngDialog, $scope, fMembership, fMember) {
    //TODO AJ Check if agent exist via Restfull call to API

    $scope.member = {};
    var vm = this;

    $scope.submit = function (member) {

        //debugger;

        fMembership.AddAgent(fMember.Id, angular.toJson(member));
    }

    //Form Validation START -------------------------------------------------------------------------------------
    
    
    vm.onSubmit = onSubmit;

    vm.options = {};



    function onSubmit() {
        if (vm.form.$valid) {
            vm.options.updateInitialValue();
            alert(JSON.stringify(vm.model), null, 2);
            ngDialog.close();
        }
    }
    
    
    //Form Validation END ---------------------------------------------------------------------------------------


    $scope.memberfields = [

      {
          key: 'emailAddress',
          type: 'input',
          templateOptions: {
              required: true,
              label: 'Email Address',
              placeholder: 'Email Address'
          }
      },

      {
          key: 'username',
          type: 'input',
          templateOptions: {
              required: true,
              label: 'User Name',
              placeholder: 'User Name'
          }
      },

      {
          key: 'firstName',
          type: 'input',
          templateOptions: {
              required: true,
              label: 'First Name',
              placeholder: 'First Name'
          }
      },

      {
          key: 'lastName',
          type: 'input',
          templateOptions: {
              required: true,
              label: 'Last Name',
              placeholder: 'Last Name'
          }
      },
   
      {
          key: 'cellNumber',
          type: 'input',
          templateOptions: {
              required: true,
              label: 'Cell Number',
              placeholder: 'Cell Number'
          }
      },

      {
          key: 'password',
          type: 'input',
          templateOptions: {
              required: true,
              type: 'password',
              label: 'Password',
              placeholder: 'Password'
          }
      },

      {
          key: 'confirmPassword',
          type: 'input',
          templateOptions: {
              requires: true,
              type: 'password',
              label: 'Confirm Password',
              placeholder: 'Confirm Password'
          }
      },

    ]

}]);