import {createAsyncThunk, createSlice} from "@reduxjs/toolkit";

export const BASE_URL = "https://localhost:7208/graphql";

export const initialState = {
    todos: [],
    categories: [],
    storage: "XmlStorage",
    addCategoryFormActive: true,
};


export const fetchCategories = createAsyncThunk(
    "todolist/fetchCategories",
    async (_, {getState}) => {
        const state = getState();
        const response = await fetch(BASE_URL, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "StorageName": state.todolist.storage
            },
            body: JSON.stringify({
                query: `
                    {
                         categories {
                            name
                        }
                    }
                `
            }),
        });
        const data = await response.json();
        return data.data.categories;
    }
);


export const fetchToDos = createAsyncThunk(
    "todolist/fetchToDos",
    async (_, {getState}) => {
        const state = getState();
        const response = await fetch(BASE_URL, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "StorageName": state.todolist.storage
            },
            body: JSON.stringify({
                query: `
                    {
                         todos {
                             id
                             task
                             isPerformed
                             categoryName
                             dateToPerform
                        }
                    }
                `
            }),
        });
        const data = await response.json();
        return data.data.todos;
    }
);


export const addToDo = createAsyncThunk(
    "todolist/addToDo",
    async (newToDo, {getState}) => {
        const state = getState();
        await fetch(BASE_URL, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "StorageName": state.todolist.storage
            },
            body: JSON.stringify({
                query: `
                     mutation AddToDo($todo: ToDoInputType!) {
                         addToDo(todo: $todo) {
                                id
                                task
                                isPerformed
                                categoryName
                                dateToPerform
                        }
                     }
                     `,
                variables: {
                    todo: newToDo
                }
            }),
        });
        return newToDo;
    }
);

export const togglePerformed = createAsyncThunk(
    "todolist/togglePerformed",
    async (toggledToDo, {getState}) => {
        const state = getState();
        const response = await fetch(BASE_URL, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "StorageName": state.todolist.storage
            },
            body: JSON.stringify({
                query: `
                    mutation TogglePerformed($todo: ToDoInputType!){
                         handlePerformed( handlePerformed: $todo ){
                           id
                           isPerformed
                         }
                        }
                     `,
                variables: {
                    todo: toggledToDo
                }
            }),
        });
        const data = await response.json();

        return data.data.handlePerformed;

    }
);


export const deleteToDo = createAsyncThunk(
    "todolist/deleteToDo",
    async (id, {getState}) => {
        const state = getState();
        await fetch(BASE_URL, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "StorageName": state.todolist.storage
            },
            body: JSON.stringify({
                query: `
                    mutation DeleteToDo($id: ID!){
                        deleteToDo(id: $id)
                        }
                     `,
                variables: {
                    id: id
                }
            }),
        });
        return id;
    }
);

export const addCategory = createAsyncThunk(
    "todolist/addCategory",
    async (newCategory, {getState}) => {
        const state = getState();
        await fetch(BASE_URL, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "StorageName": state.todolist.storage
            },
            body: JSON.stringify({
                query: `
                         mutation AddCategory($category: CategoryInputType!) {
                             addCategory(category: $category) {
                                 id
                                 name
                            }
                         }
                     `,
                variables: {
                    category: newCategory
                }
            }),
        });
        return newCategory;
    }
)

const toDoListSlice = createSlice({
    name: "todolist",
    initialState,
    reducers: {
        changeStorage(state, action) {
            state.storage = action.payload;
        },
        handleAddCategoryFormActive(state) {
            state.addCategoryFormActive = !state.addCategoryFormActive
        },
    },
    extraReducers: (builder) => {
        builder.addCase(fetchToDos.fulfilled, (state, action) => {
            state.todos = action.payload;
        });
        builder.addCase(fetchCategories.fulfilled, (state, action) => {
            state.categories = action.payload;
        });
        builder.addCase(addToDo.fulfilled, (state, action) => {
            state.todos.push(action.payload);
        });
        builder.addCase(addCategory.fulfilled, (state, action) => {
            state.categories.push(action.payload);
        });
        builder.addCase(togglePerformed.fulfilled, (state, action) => {
            state.todos = state.todos.map((todo) =>
                todo.id === action.payload.id
                    ? {...todo, isPerformed: action.payload.isPerformed}
                    : todo,
            );
        });
        builder.addCase(deleteToDo.fulfilled, (state, action) => {
            state.todos = state.todos.filter((todo) => todo.id !== action.payload);
        });
    },
});

export const {changeStorage, handleAddCategoryFormActive} = toDoListSlice.actions;

export default toDoListSlice.reducer;
