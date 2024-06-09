import {useDispatch, useSelector} from "react-redux";
import {ToDo} from "./ToDo";
import styles from "../../styles/ToDoList.module.css";
import {useEffect} from "react";
import {fetchToDos} from "@/features/todolist/ToDoListSlice.js";

export function ToDoList() {
    const todos = useSelector((state) => state.todolist.todos);
    const dispatch = useDispatch();

    useEffect(() => {
        dispatch(fetchToDos())
    }, [dispatch]);

    const sortedToDos = [...todos].sort((a, b) => a.isPerformed - b.isPerformed);

    return (
        <ul className={styles.todoList}>
            {sortedToDos.map((todo, index) => (
                <ToDo todo={todo} key={index}/>
            ))}
        </ul>
    );
}
