import { fetchGames, renderGameCard } from "./api.js";

const container = document.querySelector("#featured-games");

async function main() {
  if (!container) {
    return;
  }

  try {
    const games = await fetchGames();
    container.innerHTML = games.slice(0, 6).map(renderGameCard).join("");
  } catch (error) {
    container.innerHTML = "<p>Каталог пока недоступен. Проверьте локальный API.</p>";
  }
}

main();
