$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblDataFavoriteSentences').DataTable({
        "ajax": { url: '/user/usersentences/getfavoritesentences/' },
        "columns": [
            { data: 'sentence.phrase', width: 40 },
            { data: 'sentence.translation', width: 40 },
            { data: 'sentence.answer', width: 30 },
            { data: 'sentence.answerTranslation', width: 30 },
            { data: 'date', width: 20, render: DataTable.render.date() },
            {
                data: 'sentence.sentenceId',
                render: function (data) {
                    return `<div class="btn-group">
                              <a href="/User/UserSentences/DislikeSentence?id=${data}" type="button" class="btn btn-primary"><i class="bi bi-trash-fill"></i></a>
                            </div>`
                },
                width: 20
            }
        ]
    });
}