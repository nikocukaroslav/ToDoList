import styles from "../../styles/ToDo.module.css";

import {useState} from "react";
import {useDispatch} from "react-redux";
import {deleteToDo, togglePerformed} from "./ToDoListSlice.js";
import TrashSvg from "@/svg/TrashSvg.jsx";

export function ToDo({todo}) {
    const [active, setActive] = useState(false);

    const dispatch = useDispatch();

    function handleDelete() {
        const id = todo.id;

        dispatch(deleteToDo(id));

    }


    function handlePerformed() {
        const toggledToDo = {
            id: todo.id,
            isPerformed: todo.isPerformed,
        }

        dispatch(togglePerformed(toggledToDo));
    }

    return (
        <li
            className={`${todo.isPerformed ? styles.completed : ""}`}
            onMouseOut={() => setActive(true)}
            onMouseLeave={() => setActive(false)}
        >
            <div className={styles.todo}>
                <input
                    checked={todo.isPerformed}
                    type="checkbox"
                    value={todo.isPerformed}
                    className={styles.checkbox}
                    onChange={handlePerformed}
                />
                <span>{todo.task}</span>
            </div>
            {active ? (
                <button
                    className={styles.trashButton}
                    onClick={handleDelete}
                >
                    <TrashSvg/>
                </button>
            ) : (
                <div className={styles.todoDescription}>
                    <span>{todo.categoryName}</span>
                    <span>{todo.dateToPerform && new Date(todo.dateToPerform).toLocaleDateString()}</span>
                </div>
            )}
        </li>
    );
}
