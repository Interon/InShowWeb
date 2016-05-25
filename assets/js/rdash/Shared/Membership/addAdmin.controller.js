angular.module('RDash').controller('AddAdminController', ['ngDialog', '$scope', 'fMembership', 'fMember', function (ngDialog, $scope, fMembership, fMember) {
    //TODO Add Confirm Password block and 

    $scope.member = {};
    $scope.submit = function (member) {

        fMembership.ChangePassword(fMember.Id, angular.toJson(member));

    }

    $scope.memberfields = [

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

    ]



}]);