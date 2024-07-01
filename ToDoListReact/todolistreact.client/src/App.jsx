import "./styles/App.css";
import {ToDoList} from "./features/todolist/ToDoList";
import {AddToDoForm} from "./features/todolist/AddToDoForm";
import {AddCategory} from "./features/todolist/AddCategory";
import {useSelector} from "react-redux";
import StorageChanger from "@/features/todolist/StorageChanger.jsx";

function App() {
    const isActive = useSelector((state) => state.todolist.addCategoryFormActive);

    return (
        <>
            <StorageChanger/>
            <main>
                <div className="div">
                    {isActive && <h2>ToDo list</h2>}
                    <AddCategory/>
                </div>
                <AddToDoForm/>
                <ToDoList/>
            </main>
        </>
    );
}

export default App;
