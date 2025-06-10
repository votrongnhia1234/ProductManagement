// Customer JavaScript

// Load categories for navigation
document.addEventListener("DOMContentLoaded", () => {
    loadCategories()
    updateCartCount()
})

async function loadCategories() {
    try {
        const response = await fetch("/api/categories")
        const categories = await response.json()

        const dropdown = document.getElementById("categoryDropdown")
        if (dropdown) {
            dropdown.innerHTML =
                '<li><a class="dropdown-item" href="/Customer/Home">Tất cả sản phẩm</a></li><li><hr class="dropdown-divider"></li>'

            categories.forEach((category) => {
                const li = document.createElement("li")
                li.innerHTML = `<a class="dropdown-item" href="/Customer/Home?categoryFilter=${category.id}">${category.name}</a>`
                dropdown.appendChild(li)
            })
        }
    } catch (error) {
        console.error("Error loading categories:", error)
    }
}

function updateCartCount() {
    // Get cart count from session storage or API
    const cartCount = sessionStorage.getItem("cartCount") || "0"
    const cartBadge = document.getElementById("cartCount")
    if (cartBadge) {
        cartBadge.textContent = cartCount
        cartBadge.style.display = cartCount === "0" ? "none" : "inline"
    }
}

function addToCart(productId, quantity = 1) {
    fetch("/Customer/Cart/AddToCart", {
        method: "POST",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded",
            RequestVerificationToken: document.querySelector('input[name="__RequestVerificationToken"]').value,
        },
        body: `productId=${productId}&quantity=${quantity}`,
    })
        .then((response) => {
            if (response.ok) {
                // Update cart count
                const currentCount = Number.parseInt(sessionStorage.getItem("cartCount") || "0")
                sessionStorage.setItem("cartCount", (currentCount + quantity).toString())
                updateCartCount()

                // Show success message
                showToast("Đã thêm sản phẩm vào giỏ hàng", "success")
            }
        })
        .catch((error) => {
            console.error("Error adding to cart:", error)
            showToast("Có lỗi xảy ra khi thêm vào giỏ hàng", "error")
        })
}

function showToast(message, type = "info") {
    // Create toast container if it doesn't exist
    let toastContainer = document.querySelector(".toast-container")
    if (!toastContainer) {
        toastContainer = document.createElement("div")
        toastContainer.className = "toast-container position-fixed top-0 end-0 p-3"
        toastContainer.style.zIndex = "9999"
        document.body.appendChild(toastContainer)
    }

    // Create toast
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
