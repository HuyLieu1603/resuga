document.querySelector("form").addEventListener("submit", async function (e) {
  e.preventDefault();
  const email = e.target.Email.value;
  const password = e.target.Password.value;

  const res = await fetch("/Auth/Login", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, password }),
  });

  const result = await res.json();
  if (res.ok) {
    // Lưu JWT vào cookie hoặc localStorage
    localStorage.setItem("token", result.token);
    window.location.href = "/dashboard"; // hoặc trang chính
  } else {
    alert("Sai tài khoản hoặc mật khẩu!");
  }
});
