import { login, saveToken } from "./api.js";

const form = document.querySelector("#login-form");
const status = document.querySelector("#login-status");

form?.addEventListener("submit", async event => {
  event.preventDefault();
  const formData = new FormData(form);
  const username = String(formData.get("username") || "");
  const password = String(formData.get("password") || "");

  try {
    const result = await login(username, password);
    if (result.token) {
      saveToken(result.token);
      status.textContent = `Вход выполнен: ${result.profile.displayName}`;
      return;
    }

    status.textContent = result.error || "Не удалось войти.";
  } catch (error) {
    status.textContent = "Локальный API недоступен.";
  }
});
