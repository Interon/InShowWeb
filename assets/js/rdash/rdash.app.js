/// <reference path="Profile/AgentDetailsView.html" />
//Rdash


angular.module('RDash', ['ui.bootstrap', 'ui.router', 'ngCookies', 'angularFileUpload',
                         'ngResource', 'ngRoute', 'angular-loading-bar', 'ngAnimate', 'xeditable',
                         'ngFileUpload', 'ngImgCrop', 'toaster', 'formly', 'formlyBootstrap', 'ngDialog']);

/**
 * Route configuration for the RDash module.
 */
angular.module('RDash').config(['$stateProvider', '$urlRouterProvider',  'ngDialogProvider','cfpLoadingBarProvider',
    function ($stateProvider, $urlRouterProvider, ngDialogProvider) {
        ngDialogProvider.setDefaults({
            className: 'ngdialog-theme-default',
            plain: false,
            showClose: true,
            closeByDocument: true,
            closeByEscape: true,
            appendTo: false,
            preCloseCallback: function () {
                console.log('default pre-close callback');
            }
        });
        // For unmatched routes
        $urlRouterProvider.otherwise('/');

        // Application routes
        $stateProvider
            .state('index', {
                url: '/',
                templateUrl: '/templates/template.html'
            })
            .state('dashboard', {
                url: '/dashboard',
                templateUrl: '/templates/dashboard.html'
            })
             .state('profile', {
                 url: '/profile',
                 templateUrl: '/assets/js/rdash/Profile/AgentDetailsView.html'
             })
             .state('listings', {
                 url: '/listings',
                 templateUrl: '/templates/listings.html'
             })
        .state('inshow', {
            url: '/inshow',
            templateUrl: '/templates/template.html'
        });
    }
]);


angular.module('RDash').run(function ($rootScope, $state, editableOptions) {

    editableOptions.theme = 'bs3';
});



