(function () {
    "use strict";

    var angularApp = angular.module("app");
    angularApp.controller("patientNoteController", ["$scope", "$http", "$uibModal", patientNoteController]);

    function patientNoteController($scope, $http, $uibModal) {
        var ctrl = this;
        ctrl.value = "test value";
        ctrl.patientNotes = [];
        ctrl.inAddMode = inAddMode;
        ctrl.inEditMode = inEditMode;
        ctrl.add = add;
        ctrl.edit = edit;
        ctrl.save = save;
        ctrl.activepatientNote = {};
        // initialization
        retrieveAll();

        //  private
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

        var formModifyMode = "none;"
        function retrieveAll() {
            $http.get("../api/PatientNotesApi").success(function (data) {
                ctrl.patientNotes = data;
            }).error(function () { alert("Error on loading patientNotes") });
        }

        function add() {
            var patientId = GetSelectedPatientId();
            if (!patientId)
            {
                alert("Please specify the patient you wish to creates notes for");
                return;
            }

            formModifyMode = "add";
            ctrl.activepatientNote = {};
            var addPatientNote = { id: 0, notes: [{ description: "", noteTypeId: 5 }], patientId: GetSelectedPatientId(), recordedDate: new Date() };
            var modalInstance = showModal(addPatientNote);
            modalInstance.result.then(function (patientNote) {                
                $http.post("../api/PatientNotesApi", patientNote).success(function (data) {
                    ctrl.patientNotes.push(patientNote);
                }).error(function () { alert("Failed to add new patient note") });
            });
        }

        function edit(patientNoteId) {
            formModifyMode = "edit";
            // load single patientNote with the id and make it active
            var editPatientNote = null;
            for (var i = 0; i < ctrl.patientNotes.length; i++) {
                if (ctrl.patientNotes[i].id == patientNoteId) {
                    editPatientNote = ctrl.patientNotes[i];
                }
            }

            if (!editPatientNote)
            {
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

            
            //$http.get("../api/patientNoteApi/" + patientNoteId).success(function (data) {
            //    ctrl.activepatientNote = data;
            //}).error(function () { alert("Error loading the patientNote") });


        }

        function save() {
            if (inAddMode) {
                alert("save it to db and refresh the list to maintain sort order" + ctrl.activepatientNote.Id);
            } else if (inEditMode()) {
                alert("update db and refresh the ui to present the updated record" + ctrl.activepatientNote.Id);
            }

            ctrl.activepatientNote = {};
        }

        function inAddMode() {
            return formModifyMode === "add";
        }

        function inEditMode() {
            return formModifyMode === "edit";
        }

    }
})();