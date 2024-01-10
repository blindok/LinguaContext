$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblDataUserSentences').DataTable({
        "ajax": { url: '/sentences/getusertasks/' },
        "columns": [
            { data: 'sentence.phrase', width: 40 },
            { data: 'sentence.translation', width: 40 },
            { data: 'sentence.answer', width: 30 },
            { data: 'sentence.answerTranslation', width: 30 },
            { data: 'info.createdAt', width: 20, render: DataTable.render.date() },
            { data: 'info.lastEditedAt', width: 20, render: DataTable.render.date() },
            {
                data: 'sentence.sentenceId',
                render: function (data) {
                    return `<div class="btn-group">
                              <a href="/User/UserSentences/EditSentence?id=${data}" type="button" class="btn btn-primary"><i class="bi bi-pencil-square"></i></a>
                              <a href="/User/UserSentences/DeleteSentence?id=${data}" type="button" class="btn btn-primary"><i class="bi bi-trash-fill"></i></a>
                            </div>`
                },
                width: 20
            }
        ]
    });
}