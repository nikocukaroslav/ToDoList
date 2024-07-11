import {configureStore} from "@reduxjs/toolkit";
import {combineEpics, createEpicMiddleware} from "redux-observable";
import todoListReducer, {epics} from "./features/todolist/ToDoListSlice.js";

const epicMiddleware = createEpicMiddleware();

const store = configureStore({
    reducer: {
        todolist: todoListReducer,
    },
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware().concat(epicMiddleware),
});

const rootEpic = combineEpics(...epics);
epicMiddleware.run(rootEpic);

export default store;
