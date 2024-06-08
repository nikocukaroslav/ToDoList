import {configureStore} from "@reduxjs/toolkit";

import toDoReducer from "./features/todos/ToDoSlice.js";

const store = configureStore({
    reducer: {
        todo: toDoReducer,
    },
});

export default store;
