(function () {
    "use strict";

    angular.module('app').controller('treatmentPlanModalController', function ($scope, $uibModalInstance, treatmentPlan) {
        var ctrl = this;
        ctrl.treatmentPlan = treatmentPlan;

        ctrl.add = function () {
           // removeEmptyNotes();
            var newTreatment = {objectState: objectState.add};
            ctrl.treatmentPlan.treatments.push(newTreatment);
        }

        ctrl.delete = function (index) {            
            ctrl.treatmentPlan.treatments[index].objectState = objectState.delete;
        }

        ctrl.update = function (treatment) {
            if ((treatment.objectState === objectState.add) || (treatment.objectState === objectState.delete))
                return;

            treatment.objectState = objectState.update;
        }

        ctrl.ok = function () {
           // removeEmptyNotes();
            if (ctrl.treatmentPlan.treatments.length === 0) {
                alert('Sorry there are no treatments to save');
            }
            else {
                $uibModalInstance.close(ctrl.treatmentPlan);
            }

        };

        ctrl.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        //function removeEmptyNotes() {
        //    for (var i = ctrl.treatmentPlan.treatments.length - 1; i > -1; i--) {
        //        var treatment = ctrl.treatmentPlan.treatments[i];
        //        if (!treatment.description) {
        //            ctrl.treatmentPlan.treatments.objectState = objectState.delete;
        //        }
        //    }
        //}

    });//controller

})();