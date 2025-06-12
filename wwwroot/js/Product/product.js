const unitPrice = 185000;
function updateAmount(quantity) {
  const total = parseInt(quantity) * unitPrice;
  document.getElementById("amountInput").value = total;
}

// Gán mặc định giá trị ban đầu
document.addEventListener("DOMContentLoaded", function () {
  updateAmount(1);
});
