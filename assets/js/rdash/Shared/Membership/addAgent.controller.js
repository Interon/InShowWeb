angular.module('RDash').controller('AddAgentController', ['ngDialog', '$scope', 'fMembership', 'fMember', '$timeout', function (ngDialog, $scope, fMembership, fMember, $timeout) {
    //TODO Add Confirm Password block and 

    $scope.model = {};
    

    $scope.onSubmit = function (member) {

        if (vm.form.$valid) {
            vm.options.updateInitialValue();
            alert(JSON.stringify(vm.model), null, 2);
            ngDialog.close();
        }

        fMembership.AddAgent(fMember.email, angular.toJson(member));

    }

    



    $scope.existingUsers = [
        'john',
        'tyrion',
        'arya'
    ];

    $scope.getAgentMemberByEmail = function (email, memberType) {
        agentMember = fMembership.GetMemberByEmail(email, memberType);
        debugger;
        return agentMember;
    }


    $scope.fields = [
          {
              key: 'text',
              type: 'input-loader',
              templateOptions: {
                  label: 'Email',
                  placeholder: 'Email',
                  required: true,
                  onKeydown: function (value, options) {
                      options.validation.show = false;
                  },
                  onBlur: function (value, options) {
                      options.validation.show = null;
                  }
              },
              asyncValidators: {
                  uniqueEmail: {
                      expression: function ($viewValue, $modelValue, scope) {
                          scope.options.templateOptions.loading = true;
                          return $timeout(function () {
                              scope.options.templateOptions.loading = false;
                              //-----------Custom Code -------------------------------------------------------
                              var memberByEmail = $scope.getAgentMemberByEmail($viewValue, 'agent')
                              debugger;
                              if (memberByEmail !== -1) {


                                  throw new Error('taken');
                              }
                              //------------------------------------------------------------------------------
                              if ($scope.existingUsers.indexOf($viewValue) !== -1) {
                                  throw new Error('taken');
                              }
                          }, 1000);
                      },
                      message: '"This email is already taken."'
                  }
              },
              modelOptions: {
                  updateOn: 'blur'
              }
          },

            {
                key: 'userName',
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
                key: 'emailAddress',
                type: 'input',
                templateOptions: {
                    label: 'Email Address',
                    placeholder: 'Email Address'
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



    ];

    $scope.originalFields = angular.copy($scope.fields);

}]);