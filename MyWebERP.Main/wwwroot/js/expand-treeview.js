/*Lỗi chưa chạy được*/
window.getCheckedTreeItems = function () {
    const checkedValues = [];
    const checkboxes = document.querySelectorAll('.dxbl-treeview-container input[type="checkbox"]:checked');

    checkboxes.forEach(cb => {
        checkedValues.push(cb.getAttribute('parent-id')); // hoặc id, value tùy cách DevExpress bind
    });

    return checkedValues;
};
