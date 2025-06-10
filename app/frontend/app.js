const apiUrl = "https://localhost:5001/todos"; // Update port if needed

const todoList = document.getElementById("todo-list");
const todoForm = document.getElementById("todo-form");
const titleInput = document.getElementById("title");

// Load todos on page load
window.onload = loadTodos;

function loadTodos() {
  fetch(apiUrl)
    .then(res => res.json())
    .then(data => {
      todoList.innerHTML = "";
      data.forEach(todo => {
        addTodoToUI(todo);
      });
    });
}

todoForm.addEventListener("submit", function (e) {
  e.preventDefault();
  const title = titleInput.value.trim();
  if (!title) return;

  fetch(apiUrl, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ title, isCompleted: false })
  })
    .then(res => res.json())
    .then(newTodo => {
      addTodoToUI(newTodo);
      titleInput.value = "";
    });
});

function addTodoToUI(todo) {
  const li = document.createElement("li");
  li.className = todo.isCompleted ? "completed" : "";
  li.setAttribute("data-id", todo.id);
  li.innerHTML = `
    <span>${todo.title}</span>
    <div class="actions">
      <button onclick="toggleComplete(${todo.id}, ${!todo.isCompleted})">
        ${todo.isCompleted ? "Undo" : "Done"}
      </button>
      <button onclick="deleteTodo(${todo.id})">Delete</button>
    </div>
  `;
  todoList.appendChild(li);
}

function toggleComplete(id, isCompleted) {
  fetch(`${apiUrl}/${id}`)
    .then(res => res.json())
    .then(todo => {
      todo.isCompleted = isCompleted;
      return fetch(`${apiUrl}/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(todo)
      });
    })
    .then(() => loadTodos());
}

function deleteTodo(id) {
  fetch(`${apiUrl}/${id}`, { method: "DELETE" })
    .then(() => loadTodos());
}
