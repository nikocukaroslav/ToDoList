import {createAsyncThunk, createSlice} from "@reduxjs/toolkit";

export const BASE_URL = "https://localhost:7208/graphql";

const initialState = {
    todos: [],
    categories: [],
};

export const fetchCategories = createAsyncThunk(
    "todo/fetchCategories",
    async () => {
        const response = await fetch(BASE_URL, {
            method: "POST",
            headers: {"Content-Type": "application/json"},
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
    "todo/fetchToDos",
    async () => {
        const response = await fetch(BASE_URL, {
            method: "POST",
            headers: {"Content-Type": "application/json"},
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
    "todo/addToDo",
    async (newToDo) => {
        await fetch(BASE_URL, {
            method: "POST",
            headers: {"Content-Type": "application/json"},
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
    "todo/togglePerformed",
    async (toggledToDo) => {
        const response = await fetch(BASE_URL, {
            method: "POST",
            headers: {"Content-Type": "application/json"},
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
    "todo/deleteToDo",
    async (id) => {
        await fetch(BASE_URL, {
            method: "POST",
            headers: {"Content-Type": "application/json"},
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
    "todo/addCategory",
    async (newCategory) => {
        await fetch(BASE_URL, {
            method: "POST",
            headers: {"Content-Type": "application/json"},
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

const toDoSlice = createSlice({
    name: "todo",
    initialState,
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

export default toDoSlice.reducer;
