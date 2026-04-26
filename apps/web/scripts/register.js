import { register, saveToken } from "./api.js";

const form = document.querySelector("#register-form");
const status = document.querySelector("#register-status");

form?.addEventListener("submit", async event => {
  event.preventDefault();
  const formData = new FormData(form);
  const username = String(formData.get("username") || "");
  const displayName = String(formData.get("displayName") || "");
  const password = String(formData.get("password") || "");

  try {
    const result = await register(username, displayName, password);
    if (result.token) {
      saveToken(result.token);
      status.textContent = `Аккаунт создан: ${result.profile.displayName}`;
      return;
    }

    status.textContent = result.error || "Не удалось зарегистрироваться.";
  } catch (error) {
    status.textContent = "Локальный API недоступен.";
  }
});
