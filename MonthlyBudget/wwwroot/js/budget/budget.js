$(document).ready(function () {
    function updateLink() {
        var m = $('#BudgetMonth').val();
        var y = $('#BudgetYear').val();
        $('#changemonth').attr('href', '/Budget/Index?m=' + m + '&y=' + y);
    }

    $('#BudgetYear').change(updateLink);
    $('#BudgetMonth').change(updateLink);

});