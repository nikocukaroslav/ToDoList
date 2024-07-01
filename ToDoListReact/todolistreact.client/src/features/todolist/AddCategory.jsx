import styles from "../../styles/AddCategoty.module.css";
import {useState} from "react";
import {useDispatch} from "react-redux";
import {addCategory, handleAddCategoryFormActive} from "@/features/todolist/ToDoListSlice.js";
import {generateGUID} from "@/helpers.js";
import LeftArrowSvg from "@/svg/LeftArrowSvg.jsx";
import PlusSvg from "@/svg/PlusSvg.jsx";

export function AddCategory() {
    const [active, setActive] = useState(false);
    const [categoryName, setCategoryName] = useState("");

    const dispatch = useDispatch();

    function handleActive() {
        setActive((active) => !active);
        dispatch(handleAddCategoryFormActive());
    }

    function handleSubmit(e) {
        e.preventDefault();

        const newCategory = {
            id: generateGUID(),
            name: categoryName,
        };

        dispatch(addCategory(newCategory));

        setCategoryName("");
        setActive(false);
    }

    return (
        <>
            {active === false ? (
                <div
                    className={styles.addCategoryButton}
                    onClick={handleActive}
                >
                    <span>Add category</span>
                    <PlusSvg/>
                </div>
            ) : (
                <form className={styles.addCategoryForm} onSubmit={handleSubmit}>
               <span
                   className={styles.backButton}
                   onClick={handleActive}
               >
                    <LeftArrowSvg/>
               </span>
                    <input
                        type="text"
                        value={categoryName}
                        onChange={(e) => setCategoryName(e.target.value)}
                    />
                    <button>Add</button>
                </form>
            )}
        </>
    );
}
