/// <reference path="PatientNoteController.js" />
(function () {
    "use strict";

    var angularApp = angular.module("app");
    angularApp.controller("patientNoteController", ["$scope", "$http", "$uibModal", "patientNoteDataService", patientNoteController]);

    function patientNoteController($scope, $http, $uibModal, patientNoteDataService) {
        var ctrl = this;
        ctrl.patientNotes = [];
        ctrl.add = add;
        ctrl.edit = edit;
        ctrl.delete = deletePatient;

        initialization();

        function add() {
            var patientId = GetSelectedPatientId();
            if (!patientId) {
                alert("Please select the patient you want to creates the snotes for");
                return;
            }

            var addPatientNote = { id: 0, notes: [{ description: "", noteTypeId: 5 }], patientId: GetSelectedPatientId(), recordedDate: new Date() };
            var modalInstance = showModal(addPatientNote);
            modalInstance.result.then(function (patientNote) {
                patientNoteDataService.add(patientNote).success(function (data, status, header, config) {
                    ctrl.patientNotes.push(patientNote);
                })
                .error(function () {
                    alert("Failed to add patient's note");
                });
            });
        }

        function edit(patientNoteId) {
            var patientNote = getPatientNote(patientNoteId);
            if (!patientNote) {
                alert("Please select the patient note to edit");
                return;
            }

            var editPatientNote = null;
            editPatientNote = angular.copy(patientNote);

            var modalInstance = showModal(editPatientNote);
            modalInstance.result.then(function (patientNote) {
                patientNoteDataService.update(patientNote).success(function () {
                    retrieveAll();
                }).error(function () {
                    alert("Failed to update patient's note");
                });
            });
        }

        function deletePatient(patientNoteId) {
            var patientNote = getPatientNote(patientNoteId);
            if (!patientNote) {
                alert("Please select the patient note to delete");
                return;
            }

            var url = "../api/PatientNotesApi/" + patientNoteId;
            $http.delete(url).success(function (data) {
                retrieveAll();
            }).error(function () {
                alert("Failed to delete patient's note")
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
            ctrl.patientNotes = [];

            var patientId = GetSelectedPatientId();
            if (patientId) {
                patientNoteDataService.getByPatientId(patientId).success(function (data) {
                    ctrl.patientNotes = data;
                }).error(function (data, status) {
                    alert('Failed to load notes for selected patient');
                });
            }
            else {
                patientNoteDataService.getAll().then(function (response) {
                    if (response.status === 200) {
                        ctrl.patientNotes = response.data;
                    }
                    else {
                        alert('Failed to load notes for all the patients');
                    }
                });
            }
        }
    }//controller
})();

//todo
//address validation errors froms the server side and show it correctly to the user
//do not refresh the entire grid on deleting and updating a record
//implement paging for patient notes grid 