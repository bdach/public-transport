(function () {
    var app = angular.module("myApp");

    app.factory("stringBuilder", function () {
        var strings = [];

        var stringBuilder = {
            append: function (value) {
                if (value) {
                    strings.push(value);
                }
            },

            toString: function (separator) {
                if (strings.length) {
                    return strings.join(separator);
                }
            },

            clear: function () {
                strings = [];
            }
        };

        return stringBuilder;
    });
})();