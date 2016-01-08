(function () {
    "use strict";

    angular.module("app").service("patientNoteDataService", function ($http) {
        var ctrlUrl = "../api/PatientNotesApi";
        var service = this;
        service.patientNotes = [];

        this.getAll = function () {
            return getPatientsQueryable(url);
        }

        this.getByPatientId = function (patientId) {
            var url = ctrlUrl + "?$filter=PatientId eq " + patientId;
            return getPatientsQueryable(url);
        }
       

        function getPatientsQueryable(url) {
            return $http.get(url).success(function (data) {
                service.patientNotes = data;
            }).error(function (data) {
                throw data;
            });
        }

    });//Service

})();