const API_BASE_URL = "http://127.0.0.1:5000";
const TOKEN_KEY = "kubix_token";

export async function fetchGames() {
  const response = await fetch(`${API_BASE_URL}/games`);
  return response.json();
}

export async function fetchGame(gameId) {
  const response = await fetch(`${API_BASE_URL}/games/${gameId}`);
  return response.json();
}

export async function fetchProfile(profileId) {
  const response = await fetch(`${API_BASE_URL}/profiles/${profileId}`);
  return response.json();
}

export async function login(username, password) {
  const response = await fetch(`${API_BASE_URL}/auth/login`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({ username, password })
  });

  return response.json();
}

export async function register(username, displayName, password) {
  const response = await fetch(`${API_BASE_URL}/auth/register`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({ username, displayName, password })
  });

  return response.json();
}

export function saveToken(token) {
  localStorage.setItem(TOKEN_KEY, token);
}

export function getToken() {
  return localStorage.getItem(TOKEN_KEY);
}

export function renderGameCard(game) {
  return `
    <article class="game-card">
      <div class="thumb"></div>
      <div class="game-card-body">
        <h3>${game.title}</h3>
        <p>${game.genre} • <a href="./profile.html?profileId=${encodeURIComponent(game.authorId)}">${game.authorName}</a></p>
        <a class="button secondary" href="./game.html?gameId=${encodeURIComponent(game.gameId)}">Открыть</a>
      </div>
    </article>
  `;
}
