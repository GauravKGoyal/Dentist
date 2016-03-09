/// <reference path="PatientNoteController.js" />
(function () {
    "use strict";

    var angularApp = angular.module("app");
    angularApp.controller("patientNoteController", ["$scope", "$uibModal", "patientNoteDataService", patientNoteController]);

    function patientNoteController($scope, $uibModal, patientNoteDataService) {
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

            var newPatientNote = { id: 0, notes: [{ description: "", noteTypeId: 5 }], patientId: GetSelectedPatientId(), recordedDate: new Date() };
            var modalInstance = showModal(newPatientNote);
            modalInstance.result.then(function (patientNote) {
                refreshPatientNotes();
                // or ctrl.patientNotes.push(patientNote);
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
                refreshPatientNotes();
                // or call the url to refresh page but then refreshing page will be slower than just refreshing data                
            });
        }

        function deletePatient(patientNoteId) {
            var patientNote = getPatientNote(patientNoteId);
            if (!patientNote) {
                alert("Please select the patient note to delete");
                return;
            }

            patientNoteDataService.delete(patientNoteId).then(
                function (response) {
                    removePatientNote(patientNoteId);
                },
                function (response) {
                    handleDeleteResponseError(response);
                });
        }

        function getPatientNote(id) {
            for (var i = 0; i < ctrl.patientNotes.length; i++) {
                if (ctrl.patientNotes[i].id === id) {
                    return ctrl.patientNotes[i];
                }
            }
        }

        function removePatientNote(id) {
            for (var i = 0; i < ctrl.patientNotes.length; i++) {
                if (ctrl.patientNotes[i].id === id) {
                    ctrl.patientNotes.splice(i, 1);
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
            refreshPatientNotes();
        }

        function refreshPatientNotes() {
            ctrl.patientNotes = [];

            if (isPatientSelected()) {
                getNotesForSelectedPatient();
                return;
            }

            getAllPatientNotes();
        }

        function getAllPatientNotes() {
            patientNoteDataService.getAll()
            .then(
                function (response) {
                    ctrl.patientNotes = response.data;
                },
                function (response) {
                    alert('Failed to load notes for all the patients');
                }
            )
            .catch(function (data, status) {
                console.error('error', response.status, response.data);
            });
        }

        function getNotesForSelectedPatient() {
            var patientId = GetSelectedPatientId();
            patientNoteDataService.getByPatientId(patientId).success(function (data) {
                ctrl.patientNotes = data;
            }).error(function (data, status) {
                alert('Failed to load notes for selected patient');
            });
        }
    }//controller

})();

//todo
//address model state errors (400) froms the server side and show it correctly to the user
//address text errors (400) froms the server side and show it correctly to the user
//address text errors (404) froms the server side and show it correctly to the user
//address exceptions from server
//do not refresh the entire grid on deleting and updating a record
//implement paging for patient notes grid 