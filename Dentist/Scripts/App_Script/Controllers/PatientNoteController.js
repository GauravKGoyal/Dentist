(function () {
    "use strict";

    var angularApp = angular.module("app");
    angularApp.controller("patientNoteController", ["$scope", "$http", "$uibModal", patientNoteController]);

    function patientNoteController($scope, $http, $uibModal) {
        var ctrl = this;
        ctrl.patientNotes = [];        
        initialization();

        ctrl.add = function add() {
            var patientId = GetSelectedPatientId();
            if (!patientId) {
                alert("Please specify the patient you wish to creates notes for");
                return;
            }

            var addPatientNote = { id: 0, notes: [{ description: "", noteTypeId: 5 }], patientId: GetSelectedPatientId(), recordedDate: new Date() };
            var modalInstance = showModal(addPatientNote);
            modalInstance.result.then(function (patientNote) {
                $http.post("../api/PatientNotesApi", patientNote).success(function (data) {
                    ctrl.patientNotes.push(patientNote);
                }).error(function () { alert("Failed to add new patient note") });
            });
        }

        ctrl.edit = function edit(patientNoteId) {
            // load single patientNote with the id and make it active
            var editPatientNote = null;
            for (var i = 0; i < ctrl.patientNotes.length; i++) {
                if (ctrl.patientNotes[i].id == patientNoteId) {
                    editPatientNote = ctrl.patientNotes[i];
                }
            }

            if (!editPatientNote) {
                alert("Please select the patient note to edit");
                return;
            }

            var modalInstance = showModal(editPatientNote);
            modalInstance.result.then(function (patientNote) {
                $http.put("../api/PatientNotesApi", patientNote).success(function (data) {
                }).error(
                    //rollback the changes
                    function () { alert("Failed to update patient note") }
                );
            });

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
            $http.get("../api/PatientNotesApi").success(function (data) {
                ctrl.patientNotes = data;
            }).error(function () { alert("Error on loading patientNotes") });
        }


    }//controller
})();