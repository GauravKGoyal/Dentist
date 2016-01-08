/// <reference path="~/Scripts/App_Script/Controllers/PatientNoteController.js" />
(function () {
    "use strict";

    var angularApp = angular.module("app");

    angularApp.factory('myInterceptor', ['$log', '$q', function ($log, $q) {
        //$log.debug('$log is here to show you that this is a regular factory with injection');

        var myInterceptor = {
            responseError: function (rejection) {
                alert("An error has occoured, Please check console to see it");
                return $q.reject(rejection);
            },

            requestError: function (rejection) {
                alert("An error has occoured, Please check console to see it");
                return $q.reject(rejection);
            }
        };

        return myInterceptor;
    }]);
    angularApp.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push('myInterceptor');
    }]);

})();

