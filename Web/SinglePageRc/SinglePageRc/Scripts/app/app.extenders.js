ko.extenders.required = function (target, options) {
    target.hasError = ko.computed(function () {
        if (!options.enabled()) {
            return false;
        }

        return target().length === 0;
    });

    target.errorMessage = ko.computed(function () {
        if (!target.hasError()) {
            return null;
        }

        return options.message;
    });

    return target;
};
