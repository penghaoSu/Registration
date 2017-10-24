$(function () {
    $(document).on('click', 'button.AddItem', null, function (event) {
        event.preventDefault();
        $target = $(this).parent().parent().parent();
        $.ajax({
            url: '/Home/AddDetail',
            cache: false,
            success: function (data) {
                $target.append(data);
            },
            error: function () {
                alert("error");
            }
        });
    });
    $(document).on('click', 'button.DelItem', null, function (event) {
        event.preventDefault();
        $target = $('.Detail');
        if ($target.length != 1) {
            $(this).parent().parent().remove();
        }
    });
});
