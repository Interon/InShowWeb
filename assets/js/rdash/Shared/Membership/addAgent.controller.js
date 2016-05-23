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

    $scope.getAgentMemberByEmail = function (email) {

        var agentMember = fMembership.GetMemberByEmail(email, angular.toJson(member))
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
                  uniqueUsername: {
                      expression: function ($viewValue, $modelValue, scope) {
                          scope.options.templateOptions.loading = true;
                          return $timeout(function () {
                              scope.options.templateOptions.loading = false;
                              //-----------Unpredictable Custom Code -------------------------------------------------------
                              if ($scope.getAgentMemberByEmail($viewValue) !== -1) {
                                throw new Error('taken')
                              }
                              //-----------Unpredictable Code -------------------------------------------------------
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
          }
    ];

    $scope.originalFields = angular.copy($scope.fields);

}]);