// Fixed price formatting functions

function formatPrice(input) {
  // Get the raw value and remove all non-digit characters except dots
  let value = input.value.replace(/[^\d.]/g, "")

  // Handle multiple dots - keep only the first one
  const parts = value.split(".")
  if (parts.length > 2) {
    value = parts[0] + "." + parts.slice(1).join("")
  }

  // If there's a decimal part, limit it to 2 digits
  if (parts.length === 2) {
    const integerPart = parts[0]
    const decimalPart = parts[1].substring(0, 2) // Limit to 2 decimal places

    // Format the integer part with thousand separators
    const formattedInteger = integerPart ? Number.parseInt(integerPart).toLocaleString("vi-VN") : ""

    // Combine integer and decimal parts
    input.value = formattedInteger + (decimalPart ? "." + decimalPart : "")
  } else {
    // No decimal part, just format the integer
    if (value) {
      input.value = Number.parseInt(value).toLocaleString("vi-VN")
    }
  }
}

// Alternative: More robust price formatting
function formatPriceAdvanced(input) {
  let value = input.value

  // Remove all characters except digits and dots
  value = value.replace(/[^\d.]/g, "")

  // Handle empty input
  if (!value) {
    input.value = ""
    return
  }

  // Split by dot to handle decimal places
  let parts = value.split(".")

  // Keep only first dot, remove others
  if (parts.length > 2) {
    parts = [parts[0], parts.slice(1).join("")]
  }

  // Format integer part with thousand separators
  if (parts[0]) {
    parts[0] = Number.parseInt(parts[0]).toLocaleString("vi-VN")
  }

  // Limit decimal part to 2 digits
  if (parts[1] !== undefined) {
    parts[1] = parts[1].substring(0, 2)
    input.value = parts[0] + "." + parts[1]
  } else {
    input.value = parts[0]
  }
}

// Function to get numeric value from formatted string
function getNumericValue(formattedValue) {
  return Number.parseFloat(formattedValue.replace(/[^\d.]/g, "")) || 0
}

// Function to format display price (for showing)
function formatDisplayPrice(value) {
  if (!value) return "0"

  const numericValue = typeof value === "string" ? getNumericValue(value) : value
  return numericValue.toLocaleString("vi-VN")
}

// Function to format currency with VND symbol
function formatCurrency(value) {
  const numericValue = typeof value === "string" ? getNumericValue(value) : value
  return new Intl.NumberFormat("vi-VN", {
    style: "currency",
    currency: "VND",
  }).format(numericValue)
}
