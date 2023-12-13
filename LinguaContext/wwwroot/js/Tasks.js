$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblDataTasks').DataTable({
        "ajax": { url: '/user/usersentences/gettasks/'},
        "columns": [
            { data: 'sentence.phrase', width: "30%" },
            { data: 'sentence.translation', width: "30%" },
            { data: 'sentence.answer', width: 30 },
            { data: 'task.firstReview', width: 20, render: DataTable.render.date() },
            { data: 'task.lastReview', width: 20, render: DataTable.render.date() },
            { data: 'task.nextReview', width: 20, render: DataTable.render.date() },
            { data: 'task.currentInterval', width: "15%" },
            { data: 'task.repetitionNumber', width: "15%" },
            {
                data: 'task.isPersonalTask',
                render: function (data) {
                    if (data == true) {
                        return 'личное'
                    } else {
                        return 'базовое'
                    }
                },
                width: 15
            }
        ]
    });
}