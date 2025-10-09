//let tooltipTimeout;

//window.showTooltipAt = (selector, x, y) => {
//    const el = document.querySelector(selector);
//    if (!el) return;
//    clearTimeout(tooltipTimeout); // hủy timeout ẩn nếu đang chạy
//    el.style.left = x + 'px';
//    el.style.top = y + 'px';
//    el.style.display = 'block';
//    requestAnimationFrame(() => el.style.opacity = 1);
//};

//window.hideTooltip = (selector, delay = 200) => {
//    const el = document.querySelector(selector);
//    if (!el) return;
//    clearTimeout(tooltipTimeout);
//    tooltipTimeout = setTimeout(() => {
//        el.style.opacity = 0;
//        setTimeout(() => { el.style.display = 'none'; }, 200);
//    }, delay);
//};

//window.getTooltipSize = (selector) => {
//    const el = document.querySelector(selector);
//    if (!el) return { width: 250, height: 120 };
//    const rect = el.getBoundingClientRect();
//    return { width: rect.width, height: rect.height };
//};

//window.getWindowSize = () => {
//    return { width: window.innerWidth, height: window.innerHeight };
//};

let tooltipTimeout;
let showDelayTimeout;
let isTooltipVisible = false;

window.showTooltipWithDelay = (selector, x, y, delay = 500) => {
    clearTimeout(showDelayTimeout);

    if (isTooltipVisible) {
        // Nếu đã hiển thị thì update ngay
        window.showTooltipAt(selector, x, y);
    } else {
        // Nếu chưa hiển thị thì delay
        showDelayTimeout = setTimeout(() => {
            window.showTooltipAt(selector, x, y);
        }, delay);
    }
};

window.showTooltipAt = (selector, x, y) => {
    const el = document.querySelector(selector);
    if (!el) return;
    clearTimeout(tooltipTimeout);
    el.style.left = x + 'px';
    el.style.top = y + 'px';
    el.style.display = 'block';
    requestAnimationFrame(() => {
        el.style.opacity = 1;
        isTooltipVisible = true;
    });
};

window.hideTooltip = (selector, delay = 200) => {
    const el = document.querySelector(selector);
    if (!el) return;
    clearTimeout(showDelayTimeout);
    clearTimeout(tooltipTimeout);
    tooltipTimeout = setTimeout(() => {
        el.style.opacity = 0;
        setTimeout(() => {
            el.style.display = 'none';
            isTooltipVisible = false;
        }, 200);
    }, delay);
};

window.getTooltipSize = (selector) => {
    const el = document.querySelector(selector);
    if (!el) return { width: 250, height: 120 };
    const rect = el.getBoundingClientRect();
    return { width: rect.width, height: rect.height };
};

window.getWindowSize = () => {
    return { width: window.innerWidth, height: window.innerHeight };
};

