(function () {
    "use strict";

    angular.module('app').controller('patientNoteModalController', function ($scope, $uibModalInstance, patientNote, patientNoteDataService) {
        var ctrl = this;
        ctrl.errorMessage = null;
        ctrl.modelState = null;
        ctrl.patientNote = patientNote;

        ctrl.add = function () {
            removeEmptyNotes();
            var newNote = { description: "", objectState: objectState.add, noteTypeId: 5 };
            ctrl.patientNote.notes.push(newNote);
        }

        ctrl.delete = function (index) {
            if (ctrl.patientNote.notes.length === 1) {
                ctrl.patientNote.notes[0].description = "";
                ctrl.patientNote.notes[0].objectState = objectState.delete;
                return;
            }
            ctrl.patientNote.notes[index].objectState = objectState.delete;
        }

        ctrl.update = function (note) {
            if ((note.objectState === objectState.add) || (note.objectState === objectState.delete))
                return;

            note.objectState = objectState.update;
        }

        ctrl.ok = function () {
            removeEmptyNotes();

            if (ctrl.patientNote.notes.length === 0) {
                alert('Sorry there are no notes to save');
                return;
            }

            if (isNewPatientNote()) {
                createPatientNote();
                return;
            }

            updatePatientNote();
        };

        ctrl.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        function isNewPatientNote() {
            return ctrl.patientNote.id === 0;
        }

        function createPatientNote() {
            ctrl.errorMessage = null;
            ctrl.modelState = null;

            patientNoteDataService.create(patientNote).then(
                function (response) {
                    $uibModalInstance.close(ctrl.patientNote);
                },
                function (response) {
                    handleCreateUpdateResponseError(ctrl, response);
                });
        }

        function updatePatientNote() {
            ctrl.errorMessage = null;
            ctrl.modelState = null;

            patientNoteDataService.update(patientNote).then(
                function (response) {
                    $uibModalInstance.close(ctrl.patientNote);
                },
                function (response) {
                    handleCreateUpdateResponseError(ctrl, response);
                });
        }

        function removeEmptyNotes() {
            for (var i = ctrl.patientNote.notes.length - 1; i > -1; i--) {
                var note = ctrl.patientNote.notes[i];
                if (!note.description) {
                    ctrl.patientNote.notes.objectState = objectState.delete;
                }
            }
        }



    });//controller

})();