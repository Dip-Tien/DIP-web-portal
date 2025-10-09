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

window.getElementWidthBySelector = (selector) => {
    const el = document.querySelector(selector);
    return el ? el.getBoundingClientRect().width : 0;
};
