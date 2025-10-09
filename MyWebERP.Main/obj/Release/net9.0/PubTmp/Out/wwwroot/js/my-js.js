// Lấy giá trị 1 biến css: áp dụng lấy màu chủ đạo, tham khảo CircleIconButton.razor
window.getCssVariable = function (variableName) {
    const root = getComputedStyle(document.documentElement);
    return root.getPropertyValue(variableName).trim();
};

