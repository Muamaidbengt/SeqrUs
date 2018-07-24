$(document).ready(function () {
    var body = $(document.body),
        window = $(window),
        bar = $("#seq-risks-sidebar");

    body.scrollspy({ target: "#seq-risks-sidebar" });
    window.on("load", function () {
        body.scrollspy("refresh");
    });

    bar.affix({
        offset: {
            top: function () {
                var c = bar.offset().top,
                    d = parseInt(bar.children(0).css("margin-top"), 10),
                    e = $(".navbar").height();
                console.log(e);
                return this.top = c - e - d;
            },
            bottom: function () {
                return this.bottom = 0;
            }
        }
    });
});