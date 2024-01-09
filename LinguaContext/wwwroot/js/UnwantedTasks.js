$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblDataUnwantedTasks').DataTable({
        "ajax": { url: '/user/usersentences/getunwantedtasks/'},
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
                data: 'task.sentenceId',
                render: function (data) {
                    return `<div class="btn-group">
                              <a href="/User/UserSentences/ReDislikeTask?id=${data}" type="button" class="btn btn-primary"><i class="bi bi-trash-fill"></i></a>
                            </div>`
                },
                width: 20
            }
        ]
    });
}