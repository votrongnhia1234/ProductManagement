// Admin Panel JavaScript

// Sidebar toggle
function toggleSidebar() {
    const sidebar = document.querySelector(".admin-sidebar")
    sidebar.classList.toggle("show")
}

// Initialize admin features
document.addEventListener("DOMContentLoaded", () => {
    initializeAdminFeatures()
})

function initializeAdminFeatures() {
    // Initialize tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map((tooltipTriggerEl) => new bootstrap.Tooltip(tooltipTriggerEl))

    // Initialize popovers
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map((popoverTriggerEl) => new bootstrap.Popover(popoverTriggerEl))

    // Auto-hide alerts
    setTimeout(() => {
        const alerts = document.querySelectorAll(".alert")
        alerts.forEach((alert) => {
            const bsAlert = new bootstrap.Alert(alert)
            bsAlert.close()
        })
    }, 5000)
}

// Confirm delete actions
function confirmDelete(message = "Bạn có chắc chắn muốn xóa?") {
    return confirm(message)
}

// Show loading state
function showLoading(element) {
    const originalText = element.innerHTML
    element.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Đang xử lý...'
    element.disabled = true

    return () => {
        element.innerHTML = originalText
        element.disabled = false
    }
}

// Toast notifications
function showToast(message, type = "success") {
    const toastContainer = document.querySelector(".toast-container") || createToastContainer()
    const toastId = "toast-" + Date.now()

    const toastHTML = `
        <div class="toast" id="${toastId}" role="alert">
            <div class="toast-header">
                <i class="fas fa-${getToastIcon(type)} text-${type} me-2"></i>
                <strong class="me-auto">${getToastTitle(type)}</strong>
                <button type="button" class="btn-close" data-bs-dismiss="toast"></button>
            </div>
            <div class="toast-body">
                ${message}
            </div>
        </div>
    `

    toastContainer.insertAdjacentHTML("beforeend", toastHTML)

    const toast = document.getElementById(toastId)
    const bsToast = new bootstrap.Toast(toast)
    bsToast.show()

    toast.addEventListener("hidden.bs.toast", () => {
        toast.remove()
    })
}

function createToastContainer() {
    const container = document.createElement("div")
    container.className = "toast-container position-fixed top-0 end-0 p-3"
    container.style.zIndex = "9999"
    document.body.appendChild(container)
    return container
}

function getToastIcon(type) {
    const icons = {
        success: "check-circle",
        error: "exclamation-circle",
        warning: "exclamation-triangle",
        info: "info-circle",
    }
    return icons[type] || "info-circle"
}

function getToastTitle(type) {
    const titles = {
        success: "Thành công",
        error: "Lỗi",
        warning: "Cảnh báo",
        info: "Thông tin",
    }
    return titles[type] || "Thông báo"
}
