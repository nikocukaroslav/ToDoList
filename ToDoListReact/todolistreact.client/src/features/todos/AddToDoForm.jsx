import styles from "../../styles/AddToDoForm.module.css";
import {useEffect, useState} from "react";
import {useDispatch, useSelector} from "react-redux";
import {addToDo, fetchCategories} from "./ToDoSlice.js";
import {generateGUID} from "@/helpers.js";

export function AddToDoForm() {
    const [task, setTask] = useState("");
    const [categoryName, setCategoryName] = useState("");
    const [dateToPerform, setDateToPerform] = useState("");

    const dispatch = useDispatch();

    const categories = useSelector((store) => store.todo.categories);

    useEffect(() => {
        dispatch(fetchCategories())
    }, [dispatch]);

    function handleSubmit(e) {
        e.preventDefault();

        if (!categoryName || !task) return;

        const newToDo = {
            id: generateGUID(),
            isPerformed: false,
            task,
            categoryName,
            dateToPerform: dateToPerform || null,
        };

        dispatch(addToDo(newToDo));

        setTask("");
        setDateToPerform("");
    }

    return (
        <form className={styles.addToDoForm} onSubmit={handleSubmit}>
            <input
                type="text"
                value={task}
                onChange={(e) => setTask(e.target.value)}
                required
            />
            <select
                value={categoryName}
                onChange={(e) => {
                    const selectedCategory = categories.find(
                        (category) => category.name === e.target.value
                    );
                    setCategoryName(selectedCategory.name);
                }}
                required
            >
                {categories.map((category, index) => (
                    <option key={index}>{category.name}</option>
                ))}
            </select>
            <input
                value={dateToPerform}
                type="date"
                className={styles.date}
                onChange={(e) => setDateToPerform(e.target.value)}
            />
            <button>Add</button>
        </form>
    );
}
