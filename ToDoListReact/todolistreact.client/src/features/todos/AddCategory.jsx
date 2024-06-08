import styles from "../../styles/AddCategoty.module.css";
import { useState } from "react";
import { useDispatch } from "react-redux";
import { addCategory } from "@/features/todos/ToDoSlice.js";
import { generateGUID } from "@/helpers.js";

export function AddCategory() {
   const [active, setActive] = useState(false);
   const [categoryName, setCategoryName] = useState("");

   const dispatch = useDispatch();

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
               onClick={() => setActive((active) => !active)}
            >
               <span>Add category</span>
               <svg
                  xmlns="http://www.w3.org/2000/svg"
                  fill="currentColor"
                  className={styles.plus}
                  viewBox="0 0 16 16"
               >
                  <path d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4" />
               </svg>
            </div>
         ) : (
            <form className={styles.addCategoryForm} onSubmit={handleSubmit}>
               <span
                  className={styles.backButton}
                  onClick={() => setActive((active) => !active)}
               >
                  <svg
                     xmlns="http://www.w3.org/2000/svg"
                     width="16"
                     height="16"
                     fill="currentColor"
                     className="bi bi-arrow-left"
                     viewBox="0 0 16 16"
                  >
                     <path
                        fillRule="evenodd"
                        d="M15 8a.5.5 0 0 0-.5-.5H2.707l3.147-3.146a.5.5 0 1 0-.708-.708l-4 4a.5.5 0 0 0 0 .708l4 4a.5.5 0 0 0 .708-.708L2.707 8.5H14.5A.5.5 0 0 0 15 8"
                     />
                  </svg>
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
