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
        ctrl.open = open;
        // initialization
        retrieveAll();

        //  private
        function open() {
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: "patientNoteModal.html",
                controller: "patientNoteModalInstance as ctrl",
                size: "lg",
                resolve: {
                    patientNotes: function () {
                        return ctrl.patientNotes;
                    }
                }
            });

            modalInstance.result.then(function (selectedItem) {
                ctrl.activepatientNote = selectedItem;
            }, function () {
                alert("Modal dismissed at: " + new Date());
            });
        }

        var formModifyMode = "none;"
        function retrieveAll() {
            $http.get("../api/PatientNotesApi").success(function (data) {
                ctrl.patientNotes = data;
            }).error(function () { alert("Error on loading patientNotes") });
        }

        function add() {
            formModifyMode = "add";
            ctrl.activepatientNote = {};
        }

        function edit(patientNoteId) {
            formModifyMode = "edit";
            // load single patientNote with the id and make it active
            $http.get("../api/patientNotes/" + patientNoteId).success(function (data) {
                ctrl.activepatientNote = data;
            }).error(function () { alert("Error loading the patientNote") });
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