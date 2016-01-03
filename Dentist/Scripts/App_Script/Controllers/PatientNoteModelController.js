(function () {
    "use strict";

    angular.module('app').controller('patientNoteModalController', function ($scope, $uibModalInstance, patientNote) {
        var ctrl = this;
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
            }
            else {
                $uibModalInstance.close(ctrl.patientNote);
            }

        };

        ctrl.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

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