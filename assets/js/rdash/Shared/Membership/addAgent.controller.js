angular.module('RDash').controller('AddAgentController', ['ngDialog', '$scope', 'fMembership', 'fMember', function (ngDialog, $scope, fMembership, fMember) {
    //TODO AJ Check if agent exist via Restfull call to API


    $scope.onSubmit = onSubmit;

    $scope.existingUsers = [
        'john',
        'tyrion',
        'arya'
    ];


    $scope.member = {};

    //$scope.submit = function (member) {

    //debugger;

    //fMembership.GetMemberByEmail($scope.memberfields.key, angular.toJson(member));
    //}

    //Form Validation START -------------------------------------------------------------------------------------

    $scope.options = {};

    //Form Validation END ---------------------------------------------------------------------------------------

    $scope.memberfields = [

        {
            key: 'text',
            type: 'input-loader',
            templateOptions: {
                label: 'Username',
                placeholder: 'Username',
                required: true,
                onKeydown: function (value, options) {
                    options.validation.show = false;
                },
                onBlur: function (value, options) {
                    options.validation.show = null;
                }
            },
            asyncValidators: {
                uniqueUsername: {
                    expression: function ($viewValue, $modelValue, scope) {
                        scope.options.templateOptions.loading = true;
                        return $timeout(function () {
                            scope.options.templateOptions.loading = false;
                            if (vm.existingUsers.indexOf($viewValue) !== -1) {
                                throw new Error('taken');
                            }
                        }, 1000);
                    },
                    message: '"This username is already taken."'
                }
            },
            modelOptions: {
                updateOn: 'blur'
            }
        },

      {
          key: 'emailAddress',
          type: 'input',
          templateOptions: {
              required: true,
              label: 'Email Address',
              placeholder: 'Email Address',
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

    ];

    $scope.originalFields = angular.copy(vm.fields);

    // function definition
    function onSubmit() {
        //fMembership.GetMemberByEmail($scope.memberfields.key, angular.toJson(member));

        if (vm.form.$valid) {
            vm.options.updateInitialValue();
            alert(JSON.stringify(vm.model), null, 2);
            ngDialog.close();
        }
    }

}]);