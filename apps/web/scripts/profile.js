import { fetchProfile, renderGameCard } from "./api.js";

const container = document.querySelector("#profile-detail");
const params = new URLSearchParams(window.location.search);
const profileId = params.get("profileId");

async function main() {
  if (!container || !profileId) {
    return;
  }

  try {
    const profile = await fetchProfile(profileId);
    container.innerHTML = `
      <p class="eyebrow">Профиль автора</p>
      <h1>${profile.displayName}</h1>
      <p>@${profile.username}</p>
      <div class="section">
        <div class="section-header">
          <h2>Опубликованные игры</h2>
        </div>
        <div class="card-grid">
          ${profile.publishedGames.map(renderGameCard).join("")}
        </div>
      </div>
    `;
  } catch (error) {
    container.innerHTML = "<p>Не удалось загрузить профиль.</p>";
  }
}

main();
