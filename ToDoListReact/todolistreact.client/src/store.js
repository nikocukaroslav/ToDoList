import {configureStore} from "@reduxjs/toolkit";

import todoListReducer from "./features/todolist/ToDoListSlice.js";

const store = configureStore({
    reducer: {
        todolist: todoListReducer,
    },
});

export default store;
