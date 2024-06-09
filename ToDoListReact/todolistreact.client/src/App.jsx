import "./styles/App.css";
import {ToDoList} from "./features/todolist/ToDoList";
import {AddToDoForm} from "./features/todolist/AddToDoForm";
import {AddCategory} from "./features/todolist/AddCategory";
import StorageChanger from "@/features/todolist/StorageChanger.jsx";

function App() {
    return (
        <main>
            <h2>ToDo list</h2>
            <StorageChanger/>
            <AddToDoForm/>
            <AddCategory/>
            <ToDoList/>
        </main>
    );
}

export default App;
