(function () {
    "use strict";

    angular.module("app").service("patientNoteDataService", function ($http) {
        var ctrlUrl = "../api/PatientNotesApi";

        this.getAll = function () {
            return getPatientsQueryable(ctrlUrl);
        }

        this.getByPatientId = function (patientId) {
            var url = ctrlUrl + "?$filter=PatientId eq " + patientId;
            return getPatientsQueryable(url);
        }

        this.update = function (patientNote) {
            return $http.put(ctrlUrl, patientNote);
        }
        
        this.add = function (patientNote) {
           return $http.post("../api/PatientNotesApi", patientNote);
        }

        function getPatientsQueryable(url) {
            return $http.get(url);
        }

    });//Service

})();