// Modern Product Management System - JavaScript

// Global variables
const isLoading = false
let currentTheme = localStorage.getItem("theme") || "light"

// Initialize application
function initializeApp() {
  hideLoadingScreen()
  initializeTheme()
  initializeScrollEffects()
  initializeToasts()
  initializeSearch()
  initializeBackToTop()
  initializeAnimations()
  initializeQuickSearch()
}

// Loading screen
function hideLoadingScreen() {
  setTimeout(() => {
    const loadingScreen = document.getElementById("loadingScreen")
    if (loadingScreen) {
      loadingScreen.classList.add("hidden")
      setTimeout(() => {
        loadingScreen.remove()
      }, 500)
    }
  }, 1000)
}

// Theme management
function initializeTheme() {
  document.documentElement.setAttribute("data-theme", currentTheme)
  updateThemeIcon()
}

function toggleTheme() {
  currentTheme = currentTheme === "light" ? "dark" : "light"
  document.documentElement.setAttribute("data-theme", currentTheme)
  localStorage.setItem("theme", currentTheme)
  updateThemeIcon()

  // Add transition effect
  document.body.style.transition = "all 0.3s ease"
  setTimeout(() => {
    document.body.style.transition = ""
  }, 300)
}

function updateThemeIcon() {
  const themeIcon = document.getElementById("themeIcon")
  if (themeIcon) {
    themeIcon.className = currentTheme === "light" ? "fas fa-moon" : "fas fa-sun"
  }
}

// Scroll effects
function initializeScrollEffects() {
  let ticking = false

  function updateScrollEffects() {
    const scrolled = window.pageYOffset
    const navbar = document.querySelector(".modern-navbar")

    if (navbar) {
      if (scrolled > 100) {
        navbar.style.background = "rgba(255, 255, 255, 0.95)"
        navbar.style.backdropFilter = "blur(20px)"
      } else {
        navbar.style.background = "rgba(255, 255, 255, 0.1)"
        navbar.style.backdropFilter = "blur(20px)"
      }
    }

    ticking = false
  }

  function requestTick() {
    if (!ticking) {
      requestAnimationFrame(updateScrollEffects)
      ticking = true
    }
  }

  window.addEventListener("scroll", requestTick)
}

// Toast notifications
function initializeToasts() {
  const toasts = document.querySelectorAll(".toast")
  toasts.forEach((toast) => {
    const bsToast = new bootstrap.Toast(toast, {
      autohide: true,
      delay: 5000,
    })
    bsToast.show()
  })
}

// Search functionality
function initializeSearch() {
  const searchInput = document.querySelector(".search-input")
  if (!searchInput) return

  let searchTimeout

  searchInput.addEventListener("input", function () {
    clearTimeout(searchTimeout)
    const query = this.value.trim()

    if (query.length > 2) {
      searchTimeout = setTimeout(() => {
        performSearch(query)
      }, 300)
    }
  })

  searchInput.addEventListener("keydown", function (e) {
    if (e.key === "Enter") {
      e.preventDefault()
      performSearch(this.value.trim())
    }
  })
}

function performSearch(query) {
  if (!query) return

  // Add loading state
  const searchInput = document.querySelector(".search-input")
  if (searchInput) {
    searchInput.style.background = "rgba(255, 255, 255, 0.2)"
    searchInput.disabled = true
  }

  // Simulate search (in real app, this would be an API call)
  setTimeout(() => {
    window.location.href = `${window.location.pathname}?searchTerm=${encodeURIComponent(query)}`
  }, 500)
}

// Back to top button
function initializeBackToTop() {
  const backToTopBtn = document.querySelector(".back-to-top")
  if (!backToTopBtn) return

  function toggleBackToTop() {
    if (window.pageYOffset > 300) {
      backToTopBtn.classList.add("visible")
    } else {
      backToTopBtn.classList.remove("visible")
    }
  }

  window.addEventListener("scroll", toggleBackToTop)
  toggleBackToTop() // Initial check
}

function scrollToTop() {
  window.scrollTo({
    top: 0,
    behavior: "smooth",
  })
}

// Animations
function initializeAnimations() {
  // Intersection Observer for fade-in animations
  const observerOptions = {
    threshold: 0.1,
    rootMargin: "0px 0px -50px 0px",
  }

  const observer = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
      if (entry.isIntersecting) {
        entry.target.style.opacity = "1"
        entry.target.style.transform = "translateY(0)"
        observer.unobserve(entry.target)
      }
    })
  }, observerOptions)

  // Observe all product cards
  document.querySelectorAll(".product-card-container").forEach((card) => {
    observer.observe(card)
  })
}

// Product actions
function showDeleteConfirmation(id, name) {
  const modal = document.createElement("div")
  modal.className = "modal fade"
  modal.innerHTML = `
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content modern-modal">
                <div class="modal-header border-0 bg-danger text-white">
                    <h5 class="modal-title">
                        <i class="fas fa-exclamation-triangle me-2"></i>Xác nhận xóa
                    </h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body text-center p-4">
                    <div class="mb-3">
                        <i class="fas fa-trash-alt fa-3x text-danger"></i>
                    </div>
                    <h6 class="mb-3">Bạn có chắc chắn muốn xóa sản phẩm này?</h6>
                    <p class="text-muted mb-0"><strong>${name}</strong></p>
                </div>
                <div class="modal-footer border-0 justify-content-center">
                    <button type="button" class="btn btn-secondary modern-btn" data-bs-dismiss="modal">
                        <i class="fas fa-times me-2"></i>Hủy
                    </button>
                    <button type="button" class="btn btn-danger modern-btn" onclick="confirmDelete(${id})">
                        <i class="fas fa-trash me-2"></i>Xóa
                    </button>
                </div>
            </div>
        </div>
    `

  document.body.appendChild(modal)
  const bsModal = new bootstrap.Modal(modal)
  bsModal.show()

  modal.addEventListener("hidden.bs.modal", () => {
    modal.remove()
  })
}

function confirmDelete(id) {
  // Show loading state
  const deleteBtn = event.target
  const originalText = deleteBtn.innerHTML
  deleteBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Đang xóa...'
  deleteBtn.disabled = true

  // Simulate delete operation
  setTimeout(() => {
    window.location.href = `/Product/Delete/${id}`
  }, 1000)
}

// Utility functions
function debounce(func, wait) {
  let timeout
  return function executedFunction(...args) {
    const later = () => {
      clearTimeout(timeout)
      func(...args)
    }
    clearTimeout(timeout)
    timeout = setTimeout(later, wait)
  }
}

function throttle(func, limit) {
  let inThrottle
  return function () {
    const args = arguments

    if (!inThrottle) {
      func.apply(this, args)
      inThrottle = true
      setTimeout(() => (inThrottle = false), limit)
    }
  }
}

// Error handling
window.addEventListener("error", (e) => {
  console.error("JavaScript Error:", e.error)
  showToast("Đã xảy ra lỗi. Vui lòng thử lại.", "error")
})

// Performance monitoring
if ("performance" in window) {
  window.addEventListener("load", () => {
    setTimeout(() => {
      const perfData = performance.getEntriesByType("navigation")[0]
      console.log("Page Load Time:", perfData.loadEventEnd - perfData.loadEventStart, "ms")
    }, 0)
  })
}

// Export functions for global use
window.toggleTheme = toggleTheme
window.scrollToTop = scrollToTop
window.showToast = showToast
window.showDeleteConfirmation = showDeleteConfirmation

// Quick Search functionality
function initializeQuickSearch() {
  const quickSearchInput = document.getElementById("quickSearchInput")
  const quickSearchResults = document.getElementById("quickSearchResults")
  const searchResultsList = document.getElementById("searchResultsList")
  const searchNoResults = document.getElementById("searchNoResults")
  const searchClearBtn = document.querySelector(".search-clear-btn")

  if (!quickSearchInput) return

  let quickSearchTimeout

  quickSearchInput.addEventListener("input", function () {
    const query = this.value.trim()

    if (query.length === 0) {
      hideQuickSearchResults()
      searchClearBtn.style.display = "none"
      return
    }

    searchClearBtn.style.display = "block"

    if (query.length < 2) {
      return
    }

    clearTimeout(quickSearchTimeout)
    quickSearchTimeout = setTimeout(() => {
      performQuickSearch(query)
    }, 300)
  })

  quickSearchInput.addEventListener("focus", function () {
    if (this.value.trim().length >= 2) {
      quickSearchResults.style.display = "block"
    }
  })

  // Hide results when clicking outside
  document.addEventListener("click", (e) => {
    if (!e.target.closest(".search-container")) {
      hideQuickSearchResults()
    }
  })

  function performQuickSearch(query) {
    // Show loading state
    searchResultsList.innerHTML = `
      <div class="search-result-item">
        <div class="spinner-border spinner-border-sm me-2"></div>
        <span>Đang tìm kiếm...</span>
      </div>
    `
    quickSearchResults.style.display = "block"
    searchNoResults.style.display = "none"

    // Perform AJAX search
    fetch(`/Product/QuickSearch?query=${encodeURIComponent(query)}`, {
      method: "GET",
      headers: {
        "X-Requested-With": "XMLHttpRequest",
      },
    })
      .then((response) => response.json())
      .then((data) => {
        displayQuickSearchResults(data, query)
      })
      .catch((error) => {
        console.error("Quick search error:", error)
        searchResultsList.innerHTML = `
        <div class="search-result-item">
          <i class="fas fa-exclamation-triangle text-warning me-2"></i>
          <span>Có lỗi xảy ra khi tìm kiếm</span>
        </div>
      `
      })
  }

  function displayQuickSearchResults(results, query) {
    if (results.length === 0) {
      searchResultsList.innerHTML = ""
      searchNoResults.style.display = "block"
      return
    }

    searchNoResults.style.display = "none"

    const resultsHTML = results
      .slice(0, 5)
      .map(
        (product) => `
      <div class="search-result-item" onclick="navigateToProduct(${product.id})">
        <img src="${product.imgUrl || "https://images.unsplash.com/photo-1560472354-b33ff0c44a43?w=100"}" 
             class="search-result-image" alt="${product.productName}"
             onerror="this.src='https://images.unsplash.com/photo-1560472354-b33ff0c44a43?w=100'">
        <div class="search-result-info">
          <div class="search-result-name">${highlightSearchTerm(product.productName, query)}</div>
          <div class="search-result-price">${formatCurrency(product.price)}</div>
        </div>
      </div>
    `,
      )
      .join("")

    searchResultsList.innerHTML = resultsHTML
  }

  function highlightSearchTerm(text, term) {
    const regex = new RegExp(`(${term})`, "gi")
    return text.replace(regex, '<span class="search-highlight">$1</span>')
  }

  function hideQuickSearchResults() {
    quickSearchResults.style.display = "none"
  }
}

function clearQuickSearch() {
  const quickSearchInput = document.getElementById("quickSearchInput")
  const searchClearBtn = document.querySelector(".search-clear-btn")

  quickSearchInput.value = ""
  searchClearBtn.style.display = "none"
  document.getElementById("quickSearchResults").style.display = "none"
  quickSearchInput.focus()
}

function navigateToProduct(id) {
  window.location.href = `/Product/Details/${id}`
}

function viewAllSearchResults() {
  const quickSearchInput = document.getElementById("quickSearchInput")
  const query = quickSearchInput.value.trim()

  if (query) {
    window.location.href = `/Product?searchTerm=${encodeURIComponent(query)}`
  }
}

// Enhanced toast function with better error handling
function showToast(message, type = "success") {
  // Only show success messages
  if (type !== "success") {
    console.log(`${type.toUpperCase()}: ${message}`)
    return
  }

  const toastContainer = document.getElementById("toastContainer")
  if (!toastContainer) {
    console.warn("Toast container not found")
    return
  }

  const toastId = "toast-" + Date.now()
  const iconClass = "fa-check-circle"
  const typeLabel = "Thành công"

  const toastHTML = `
    <div class="toast modern-toast success-toast show" id="${toastId}" role="alert">
      <div class="toast-header border-0">
        <i class="fas ${iconClass} text-success me-2"></i>
        <strong class="me-auto">${typeLabel}</strong>
        <button type="button" class="btn-close" data-bs-dismiss="toast"></button>
      </div>
      <div class="toast-body">
        ${message}
      </div>
    </div>
  `

  toastContainer.insertAdjacentHTML("beforeend", toastHTML)

  const toast = document.getElementById(toastId)
  const bsToast =
    bootstrap.Toast.getInstance(toast) ||
    new bootstrap.Toast(toast, {
      autohide: true,
      delay: 5000,
    })

  bsToast.show()

  toast.addEventListener("hidden.bs.modal", () => {
    toast.remove()
  })
}

// Fixed formatCurrency function
function formatCurrency(number) {
  // Convert to number if it's a string
  const num = typeof number === "string" ? Number.parseFloat(number) : number

  // Check if it's a valid number
  if (isNaN(num)) {
    return "0₫"
  }

  // Format with Vietnamese locale
  return num.toLocaleString("vi-VN") + "₫"
}

// Initialize Bootstrap's toast
var toastElList = [].slice.call(document.querySelectorAll(".toast"))
var toastList = toastElList.map((toastEl) => new bootstrap.Toast(toastEl))
