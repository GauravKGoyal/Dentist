/// Patient Note is envelop of notes for a patient
var PatientNoteControllerObsolete = function ($cookies, $scope, $http, $routeParams, $location) {
    // Interface
    var vm = this;
    var baseLocal = "http://localhost/api";
    vm.patientId = 0; // person who holds envelop
    vm.id = 0; //patientNoteId or envelop Id    
    vm.newNote = {};
    vm.notes = [];
    vm.addNote = addNote;
    vm.deleteNote = deleteNote;
    vm.resetNewNote = resetNewNote;
    vm.noteChanged = noteChanged;
    vm.save = saveToApi;

    // Initialization
    resetNewNote();
    //mockNotes();

    if ($routeParams.id) {
        editPatientNote($routeParams.id);
    } else {
        addPatientNote();
    }

    // Implementation
    function addPatientNote() {
        loadFromCookies();
        $('#myModal').modal('show');
    }

    function editPatientNote(id) {
        loadFromApi(id);
        $('#myModal').modal('show');
    }

    function resetNewNote() {
        vm.newNote = { id: 0, description: "", noteTypeId: 5, objectState: "add" };
    }

    function addNote(note) {
        if (note) {
            vm.notes.push(note);
            vm.resetNewNote();
        }
    }

    function deleteNote(note) {
        if (note) {
            var index = vm.notes.indexOf(note);
            if (note.id == 0) {
                vm.notes.splice(index, 1);
            } else {
                vm.notes[index].objectState = "delete";
            }
        }
    }

    function noteChanged(note) {
        if (note.objectState != "add") {
            note.objectState = "update";
        }
    }

    function mockNotes() {
        vm.notes = [
            { id: 1, description: "Stop talking to honey", noteTypeId: 5, objectState:"" },
            { id: 2, description: "Hello", noteTypeId: 5, objectState:"" },
            { id: 3, description: "Side", noteTypeId: 5, objectState:"" }
        ];
    }

    function loadFromCookies() {
        var selectedPatient = GetSelectedPatient();//$cookies.get('D_SelectedPatient');
        if (selectedPatient) {
            vm.patientId = selectedPatient.Id;
        }
    }

    function loadFromApi(id) {
        $http.get('/api/PatientNotesApi', { "$filter": "Id eq " + id })
            .then(function(data) {
                vm.patientId = data.patientId;
                vm.id = data.id;
                vm.notes = data.notes;
            },
            function (data, status, statusText) { }
            );
    }

    function getPatientNoteData() {
        return {
            patientId : vm.patientId, 
            id: vm.id,
            notes : vm.notes
        };
    }

    function saveToApi() {
        $http.post('/api/PatientNotesApi', getPatientNoteData())
        .then(function(data) {
                
            },
        function(data, status, statusText) {
            
        });
    }
}