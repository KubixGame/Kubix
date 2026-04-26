import { fetchGames, renderGameCard } from "./api.js";

const container = document.querySelector("#all-games");
const searchInput = document.querySelector("#search-input");

let allGames = [];

function render(games) {
  if (!container) {
    return;
  }

  container.innerHTML = games.map(renderGameCard).join("");
}

async function main() {
  try {
    allGames = await fetchGames();
    render(allGames);
  } catch (error) {
    if (container) {
      container.innerHTML = "<p>Не удалось загрузить каталог. Поднимите локальный API.</p>";
    }
  }
}

searchInput?.addEventListener("input", () => {
  const query = searchInput.value.trim().toLowerCase();
  const filtered = allGames.filter(game => game.title.toLowerCase().includes(query));
  render(filtered);
});

main();
