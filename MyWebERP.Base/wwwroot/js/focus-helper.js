//window.registerEnterKeyHandler = function (selector) {

//    document.querySelectorAll(selector).forEach(function (el) {
//        el.addEventListener('keydown', function (e) {
//            if (e.key === "Enter") {

//                e.preventDefault();

//                let form = el.closest('form');
//                if (!form) return;

//                // Chỉ lấy những loại cần thiết, thiếu thì thêm sau
//                let elements = Array.from(form.elements);
//                let index = elements.indexOf(el);

//                for (let i = index + 1; i < elements.length; i++) {
//                    let next = elements[i];

//                    // Skip hidden, disabled, or DevExpress internal button elements
//                    if (
//                        next.disabled ||
//                        next.type === "hidden" ||
//                        next.tabIndex === -1 ||
//                        next.offsetParent === null ||
//                        next.classList.contains('dx-button') ||         // DevExpress internal button
//                        next.closest('.dx-button')                      // or anything inside a dx-button
//                    ) {
//                        continue;
//                    }

//                    next.focus();
//                    break;
//                }
//            }
//        });
//    });
//};

// Hàm này có truyền thêm dotNetHelper để focus vào ô đầu tiên của grid
window.registerEnterKeyHandler = function (selector, dotNetHelper) {
    document.querySelectorAll(selector).forEach(function (el) {
        el.addEventListener('keydown', function (e) {
            if (e.key === "Enter") {
                e.preventDefault();

                let form = el.closest('form');
                if (!form) return;

                let focusableElements = Array.from(form.querySelectorAll(
                    'input, select, textarea, button, dxbl-grid'
                ));

                //let form = document.querySelector('form');
                let footer = document.querySelector('.dxbl-modal-footer'); // Lấy footer
                let buttons = footer ? Array.from(footer.querySelectorAll('button')) : []; // Lấy button trong footer

                if (buttons.length > 0) {
                    focusableElements = [...focusableElements, ...buttons]; // Gộp lại
                };

                let index = focusableElements.indexOf(el);

                for (let i = index + 1; i < focusableElements.length; i++) {
                    let next = focusableElements[i];

                    // Skip disabled, hidden, and DevExpress internal buttons
                    if (
                        next.disabled ||
                        next.type === "hidden" ||
                        next.tabIndex === -1 ||
                        next.offsetParent === null
                    ) {
                        continue;
                    }

                    /// ✅ Check if the next element is part of a DevExpress Grid
                    let gridTable = next.querySelector('.dxbl-grid-table') ||
                        next.closest('.dxbl-virtual-scroll-viewer')?.querySelector('.dxbl-grid-table');

                    if (gridTable) {
                        // Vẫn focus vào ô filter, nhưng cứ để đây
                        let firstRow = gridTable.querySelector('tbody tr[data-visible-index="0"]');

                        let dataCells = firstRow.querySelectorAll('td:not(.dxbl-grid-command-cell):not(.dxbl-grid-empty-cell)');

                        if (dataCells.length > 0) {
                            //const firstCell = dataCells[0];
                            //firstCell.focus();

                            dotNetHelper.invokeMethodAsync('FocusFirstEditableCell');
                            break;
                        }

                    } else {
                        next.focus();
                        break;
                    }
                }
            }
        });
    });
};

// Moves focus to the next focusable control when Enter is pressed on the DxComboBox input
// Chưa dùng, enter cho dùng chung hàm trên (registerEnterKeyHandler) để đây tham khảo
//window.registerDxComboBoxEnterKeyHandler = function (inputSelector) {
//    document.querySelectorAll(inputSelector).forEach(function (inputEl) {
//        inputEl.addEventListener('keydown', function (e) {
//            if (e.key === "Enter") {
//                e.preventDefault();
//                // Find the closest form (or use document if not in a form)
//                let container = inputEl.closest("form") || document;
//                // Find all common focusable elements
//                let focusables = Array.from(container.querySelectorAll("input, textarea, select, button"))
//                    .filter(f => f.offsetParent !== null && !f.disabled && f.tabIndex !== -1);
//                let index = focusables.indexOf(inputEl);
//                if (index !== -1 && index + 1 < focusables.length) {
//                    focusables[index + 1].focus();
//                }
//            }
//        });
//    });
//};

// Không cần nữa vì dùng addShortcutKey rồi, nhưng cứ để đây tham khảo
//window.registerDxComboBoxF4KeyHandler = function (inputSelector) {
//    document.querySelectorAll(inputSelector).forEach(function (inputEl) {
//        inputEl.addEventListener('keydown', function (e) {
//            if (e.key === "F4") {
//                e.preventDefault();
//                // Lấy cha, vì input là cái textbox đang nhập liệu
//                let parent = inputEl.parentNode;
//                // Lấy button từ thằng cha
//                let button = parent.querySelector('.dxbl-edit-btn-dropdown');

//                if (button) {
//                    button.click();
//                }
//            }
//        });
//    });
//};

window.registerDxComboBoxDownKeyHandler = function (selector, popupItemSelector) {
    document.querySelectorAll(selector).forEach(function (el) {
        el.addEventListener('keydown', function (e) {
            if (e.key === "ArrowDown") {
                // Allow the default behavior to open the popup if it isn't already open.
                // Then, after a short delay, move focus to the first popup item.
                setTimeout(function () {
                    // Find the first popup item using the provided selector.
                    let popupItem = document.querySelector(popupItemSelector);
                    if (popupItem) {
                        popupItem.focus();
                    }
                }, 50); // Adjust the delay if necessary.
            }
        });
    });
};

window.addShortcutKey = function (dotNetHelper) {
    document.addEventListener("keydown", function (e) {

        if (e.key === "F4") {
            // F4 mà là combobox thì xổ popup
            var focusedElement = document.activeElement;
            e.preventDefault();

            // Lấy cha, vì input là cái textbox đang nhập liệu
            let parent = focusedElement.parentNode;
            // Lấy button từ thằng cha
            let button = parent.querySelector('.dxbl-edit-btn-dropdown');

            if (button) {
                button.click();
            }
            return;
        }

        if (e.key === "F10" | (e.ctrlKey && e.key === "s")) {
            e.preventDefault();
            document.getElementById('btnSubmit')?.click();
            return;
        }

        if (e.key === "F2" | e.key === "F8"
            | (e.ctrlKey && e.key === "n")) {
            e.preventDefault();  // Prevent default action for F2 or Ctrl+N

            var key = e.key;

            if (e.ctrlKey && e.key === "n") {
                // Nếu là Ctrl + N thì F2
                key = "F2";
            }

            //dotNetHelper.invokeMethodAsync('FocusFirstEditableCell');
            // Call the .NET method to add a new row to the DxGrid
            dotNetHelper.invokeMethodAsync('Shortcut_KeyDown', "", key);
            return;
        }
    });
};
