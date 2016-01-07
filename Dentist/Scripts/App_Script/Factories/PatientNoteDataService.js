(function () {
    "use strict";

    angular.module('app').service('patientNoteDataService', function ($http) {
        this.all = [];
        this.getAll = function () {
            $http.get("../api/PatientNotesApi").success(function (data) {
                this.all = data;
            }).error(function () {
                throw "patientNoteDataService: Error on loading all patientNotes";
            });
        }


    });//Service

})();