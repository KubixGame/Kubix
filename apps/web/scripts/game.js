import { fetchGame } from "./api.js";

const container = document.querySelector("#game-detail");
const params = new URLSearchParams(window.location.search);
const gameId = params.get("gameId");

async function main() {
  if (!container || !gameId) {
    return;
  }

  try {
    const game = await fetchGame(gameId);
    container.innerHTML = `
      <p class="eyebrow">${game.genre}</p>
      <h1>${game.title}</h1>
      <p>${game.description}</p>
      <p><strong>Автор:</strong> ${game.authorName}</p>
      <div class="hero-actions">
        <a class="button primary" href="./download.html">Играть в приложении</a>
        <a class="button secondary" href="./games.html">Назад в каталог</a>
      </div>
    `;
  } catch (error) {
    container.innerHTML = "<p>Не удалось загрузить игру.</p>";
  }
}

main();
