window.menuPopup = {
    lockBody: function (lock) {
        if (lock) {
            document.body.style.overflow = 'hidden';
        } else {
            document.body.style.overflow = '';
        }
    }
};

// Đóng mở từng nhóm control - Tạm đóng vì thấy không cần - hình như là đóng/mở tất cả.
//function collapseAllAccordions() {
//    document.querySelectorAll('.accordion-collapse').forEach(el => {
//        let bsCollapse = bootstrap.Collapse.getOrCreateInstance(el);
//        bsCollapse.hide();
//    });
//}
//function expandAllAccordions() {
//    document.querySelectorAll('.accordion-collapse').forEach(el => {
//        let bsCollapse = bootstrap.Collapse.getOrCreateInstance(el);
//        bsCollapse.show();
//    });
//}

// 🟦 Đặt vị trí dropdown ngay bên dưới input, tự canh tránh tràn
window.positionDropdownNear = (dropdownSelector, inputSelector) => {
    const dropdown = document.querySelector(dropdownSelector);
    const input = document.querySelector(inputSelector);
    if (!dropdown || !input) return;

    // Lấy kích thước khung hiển thị và input
    const rect = input.getBoundingClientRect();
    const dropdownSize = window.getTooltipSize(dropdownSelector);
    const windowSize = window.getWindowSize();

    let x = rect.left;
    let y = rect.bottom + 5;

    // Nếu dropdown bị tràn sang phải thì lùi lại
    if (x + dropdownSize.width > windowSize.width - 10)
        x = windowSize.width - dropdownSize.width - 10;

    // Nếu dropdown bị tràn xuống dưới thì hiển thị lên trên
    if (y + dropdownSize.height > windowSize.height - 10)
        y = rect.top - dropdownSize.height - 5;

    dropdown.style.position = "fixed"; // cố định theo viewport
    dropdown.style.left = `${x}px`;
    dropdown.style.top = `${y}px`;
    dropdown.style.width = rect.width + "px";
    dropdown.style.zIndex = 1050; // trên modal hoặc form
};
