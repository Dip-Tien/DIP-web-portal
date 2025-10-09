// Xử lý màu mè giao diện.

    window.setCssVar = function (varName, color) {
        console.log("setCssVar:", varName, color);
        document.documentElement.style.setProperty(varName, color);
        localStorage.setItem("theme-" + varName, color);
    };

    window.getColorVarFromStorage = function (varName) {
        const value = localStorage.getItem("theme-" + varName);
        console.log("getColorVarFromStorage:", varName, value);
        return value;
    };

window.changePrimaryColor = function (color) {
    document.documentElement.style.setProperty('--bs-primary', color);
    localStorage.setItem('primary-color', color);
};

window.getPrimaryColorFromStorage = function () {
    return localStorage.getItem('primary-color');
};

window.getCssVarDefault = function (varName) {
    return getComputedStyle(document.documentElement).getPropertyValue(varName).trim();
};