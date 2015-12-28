/// <reference path="C:\Users\Gaurav\Source\Repos\Dentist\Dentist\HtmlViews/Test.html" />
var angularApp = angular.module('app', ['ngCookies', 'ngRoute', 'ui.bootstrap']);
//angularApp.controller('PatientNoteController', ['$cookies', '$scope', '$http', '$routeParams', '$location', PatientNoteController]);

// I configure the route provider.
//angularApp.config(
//    function ($routeProvider, $locationProvider) {
//        $locationProvider.html5Mode(true);
//        $routeProvider.when("/PatientNote/edit/:id", { templateUrl: 'Views/PatientNote/EditorTemplates/PatientNoteViewModel.html', controller: 'PatientNoteController' });
//        $routeProvider.when("/PatientNote/create", { templateUrl: 'HtmlViews/Test.html', controller: 'PatientNoteController' });
       
//    }
//);  