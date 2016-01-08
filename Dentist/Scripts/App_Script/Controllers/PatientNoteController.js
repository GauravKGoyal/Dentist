/// <reference path="PatientNoteController.js" />
(function () {
    "use strict";

    var angularApp = angular.module("app");
    angularApp.controller("patientNoteController", ["$scope", "$http", "$uibModal", "patientNoteDataService", patientNoteController]);

    function patientNoteController($scope, $http, $uibModal, patientNoteDataService) {
        var ctrl = this;
        ctrl.patientNotes = [];
        initialization();

        ctrl.add = function () {
            var patientId = GetSelectedPatientId();
            if (!patientId) {
                alert("Please specify the patient you wish to creates notes for");
                return;
            }

            var addPatientNote = { id: 0, notes: [{ description: "", noteTypeId: 5 }], patientId: GetSelectedPatientId(), recordedDate: new Date() };
            var modalInstance = showModal(addPatientNote);
            modalInstance.result.then(function (patientNote) {
                $http.post("../api/PatientNotesApi", patientNote).success(function (data, status, header, config) {
                    ctrl.patientNotes.push(patientNote);
                }).error(function (data, status, header, config) {
                    alert("Failed to add new patient note")
                });
            });
        }

        ctrl.edit = function (patientNoteId) {
            var patientNote = getPatientNote(patientNoteId);
            if (!patientNote) {
                alert("Please select the patient note to edit");
                return;
            }

            var editPatientNote = null;
            editPatientNote = angular.copy(patientNote);

            var modalInstance = showModal(editPatientNote);
            modalInstance.result.then(function (patientNote) {
                $http.put("../api/PatientNotesApi", patientNote).success(function (data, status, header, config) {
                    retrieveAll();
                });
            });
        }

        ctrl.delete = function (patientNoteId) {
            var patientNote = getPatientNote(patientNoteId);
            if (!patientNote) {
                alert("Please select the patient note to delete");
                return;
            }

            var url = "../api/PatientNotesApi/" + patientNoteId;
            $http.delete(url).success(function (data) {
                retrieveAll();
            }).error(function () {
                alert("Failed to delete patient note")
            });
        }

        function getPatientNote(id) {
            for (var i = 0; i < ctrl.patientNotes.length; i++) {
                if (ctrl.patientNotes[i].id === id) {
                    return ctrl.patientNotes[i];
                }
            }
        }

        function showModal(patientNote) {
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: "patientNoteModal.html",
                controller: "patientNoteModalController as ctrl",
                size: "lg",
                resolve: {
                    patientNote: function () {
                        return patientNote;
                    }
                }
            });

            return modalInstance;
        }

        function initialization() {
            retrieveAll();
        }

        function retrieveAll() {
            var patientId = GetSelectedPatientId();
            if (patientId) {
                patientNoteDataService.getByPatientId(patientId).success(function (data) {
                    ctrl.patientNotes = patientNoteDataService.patientNotes;
                });
                return;
            }

            patientNoteDataService.getAll.success(function (data) {
                ctrl.patientNotes = patientNoteDataService.patientNotes;
            });
        }
    }//controller
})();

//todo
//seperate data layer factory
//address errors froms the server side and show it correctly to the user
//do not refresh the entire grid on deleting and updating a record
//implement paging for patient notes grid