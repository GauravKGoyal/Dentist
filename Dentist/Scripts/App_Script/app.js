var angularApp = angular.module('app', ['ngCookies', 'ngRoute']);
angularApp.controller('PatientNoteController', ['$cookies', '$scope', '$http', '$routeParams', '$location', PatientNoteController]);

// I configure the route provider.
angularApp.config(
    function ($routeProvider) {
        $routeProvider.when("/edit:id", {controller:'PatientNoteController'});
    }
);