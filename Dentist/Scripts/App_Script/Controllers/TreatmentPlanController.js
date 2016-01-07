(function () {
    "use strict";

    var angularApp = angular.module("app");
    angularApp.controller("treatmentPlanController", ["$scope", "$http", "$uibModal", treatmentPlanController]);

    function treatmentPlanController($scope, $http, $uibModal, treatmentPlanDataService) {
        var ctrl = this;
        ctrl.treatmentPlans = [];
        initialization();

        ctrl.add = function add() {
            var patientId = GetSelectedPatientId();
            if (!patientId) {
                alert("Please specify the patient you wish to create treatment for");
                return;
            }

            var addTreatmentPlan = { id: 0, treatments:[], patientId: GetSelectedPatientId(), recordedDate: new Date() };
            var modalInstance = showModal(addTreatmentPlan);
            modalInstance.result.then(function (treatmentPlan) {
                $http.post("../api/TreatmentPlansApi", treatmentPlan).success(function (data) {
                    ctrl.treatmentPlans.push(treatmentPlan);
                }).error(function () {
                    alert("Failed to add new treatment plan")
                });
            });
        }

        ctrl.edit = function edit(treatmentPlanId) {

            var treatmentPlan = getTreatmentPlan(treatmentPlanId);
            if (!treatmentPlan) {
                alert("Please select the treatment plan to edit");
                return;
            }

            var editTreatmentPlan = null;
            editTreatmentPlan = angular.copy(treatmentPlan);

            var modalInstance = showModal(editTreatmentPlan);
            modalInstance.result.then(function (treatmentPlan) {
                $http.put("../api/TreatmentPlansApi", treatmentPlan).success(function (data) {
                    retrieveAll();
                }).error(function () {
                    alert("Failed to update treatment plan")
                });
            });
        }

        ctrl.delete = function deletePatient(treatmentPlanId) {
            var treatmentPlan = getTreatmentPlan(treatmentPlanId);
            if (!treatmentPlan) {
                alert("Please select the treatment plan to delete");
                return;
            }

            var url = "../api/TreatmentPlansApi/" + treatmentPlanId;
            $http.delete(url).success(function (data) {
                retrieveAll();
            }).error(function () {
                alert("Failed to delete tratment plan")
            });
        }

        function getTreatmentPlan(id) {
            for (var i = 0; i < ctrl.treatmentPlans.length; i++) {
                if (ctrl.treatmentPlans[i].id === id) {
                    return ctrl.treatmentPlans[i];
                }
            }
        }

        function showModal(treatmentPlan) {
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: "treatmentPlanModal.html",
                controller: "treatmentPlanModalController as ctrl",
                size: "lg",
                resolve: {
                    treatmentPlan: function () {
                        return treatmentPlan;
                    }
                }
            });

            return modalInstance;
        }

        function initialization() {
            retrieveAll();
        }

        function retrieveAll() {
            var url = "../api/TreatmentPlansApi";
            var patientId = GetSelectedPatientId();
            if (patientId)
            {
                var url = url + "?$filter=PatientId eq " + patientId;
            }

            $http.get(url).success(function (data) {
                ctrl.treatmentPlans = data;
            }).error(function () {
                throw "Error on loading all treatmentPlans";
            });
        }
    }//controller
})();

//todo
//seperate data layer factory
//address errors froms the server side and show it correctly to the user
//do not refresh the entire grid on deleting and updating a record
//implement paging for patient notes grid