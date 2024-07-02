import {createSlice} from "@reduxjs/toolkit";
import {from} from "rxjs";
import {map, switchMap} from "rxjs/operators";

export const BASE_URL = "https://localhost:7208/graphql";

export const initialState = {
    todos: [],
    categories: [],
    storage: "XmlStorage",
    addCategoryFormActive: true,
};

export const fetchCategories = () => (dispatch, getState) => {
    const state = getState();
    from(fetch(BASE_URL, {
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
    }))
        .pipe(
            switchMap(response => from(response.json())),
            map(data => data.data.categories)
        ).subscribe(categories => dispatch(setCategories(categories))
    )
}

export const fetchToDos = () => (dispatch, getState) => {
    const state = getState();
    from(fetch(BASE_URL, {
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
        })
    ).pipe(
        switchMap(response => from(response.json())),
        map(data => data.data.todos),
    ).subscribe(todos => dispatch(setToDos(todos)))
}

export const createToDo = (newToDo) => (dispatch, getState) => {
    const state = getState();
    from(
        fetch(BASE_URL, {
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
        })).pipe(
        switchMap(response => from(response.json())),
        map(data => data.data.addToDo)
    ).subscribe(createdToDo => dispatch(addToDo(createdToDo)))
}

export const togglePerformed = (toggledToDo) => (dispatch, getState) => {
    const state = getState();
    from(
        fetch(BASE_URL, {
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
        })).pipe(
        switchMap(response => from(response.json())),
        map(data => data.data.handlePerformed)
    ).subscribe(toggledToDo => dispatch(toggleToDo(toggledToDo)))
}


export const deleteToDo = (id) => (dispatch, getState) => {
    const state = getState();
    from(
        fetch(BASE_URL, {
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
        })
    ).subscribe(_ => dispatch(removeToDo(id)))
}

export const createCategory = (newCategory) => (dispatch, getState) => {
    const state = getState();
    from(
        fetch(BASE_URL, {
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
        })
    ).pipe(
        switchMap(response => from(response.json())),
        map(data => data.data.addCategory)
    ).subscribe(newCategory => dispatch(addCategory(newCategory)))
}

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
        setCategories(state, action) {
            state.categories = action.payload;
        },
        setToDos(state, action) {
            state.todos = action.payload;
        },
        addToDo(state, action) {
            state.todos.push(action.payload);
        },
        toggleToDo(state, action) {
            state.todos = state.todos.map((todo) =>
                todo.id === action.payload.id
                    ? {...todo, isPerformed: action.payload.isPerformed}
                    : todo,
            );
        },
        removeToDo(state, action) {
            state.todos = state.todos.filter((todo) => todo.id !== action.payload);
        },
        addCategory(state, action) {
            state.categories.push(action.payload);
        }
    }
});

export const {
    changeStorage,
    handleAddCategoryFormActive,
    setCategories,
    setToDos,
    addToDo,
    toggleToDo,
    removeToDo,
    addCategory,
} = toDoListSlice.actions;

export default toDoListSlice.reducer;
