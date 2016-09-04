var stock;
(function (stock) {
    var index = (function () {
        function index() {
        }
        return index;
    }());
    stock.index = index;
    $(function () {
        var app = new stock.index();
    });
})(stock || (stock = {}));
