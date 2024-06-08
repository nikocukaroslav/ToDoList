import "./styles/App.css";
import { ToDoList } from "./features/todos/ToDoList";
import { AddToDoForm } from "./features/todos/AddToDoForm";
import { AddCategory } from "./features/todos/AddCategory";

function App() {
  return (
    <main>
      <h2>ToDo list</h2>
      <AddToDoForm />
      <AddCategory />
      <ToDoList />
    </main>
  );
}

export default App;
