import {createSlice} from "@reduxjs/toolkit";
import {from} from "rxjs";
import {map, switchMap} from "rxjs/operators";
import {ofType} from "redux-observable";

export const BASE_URL = "https://localhost:7208/graphql";

const graphQlQuery = (query, variables, state) => {
    return from(fetch(BASE_URL, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "StorageName": state.todolist.storage
        },
        body: JSON.stringify({
            query,
            variables
        }),
    })).pipe(
        switchMap(response => from(response.json()))
    );
};

export const initialState = {
    todos: [],
    categories: [],
    storage: "XmlStorage",
    addCategoryFormActive: true,
};

const FETCH_CATEGORIES = "FETCH_CATEGORIES"
const FETCH_TODOS = "FETCH_TODOS"
const CREATE_CATEGORY = "CREATE_CATEGORY"
const CREATE_TODO = "CREATE_TODO"
const TOGGLE_PERFORMED = "TOGGLE_PERFORMED"
const DELETE_TODO = "DELETE_TODO"

export const fetchCategories = () => ({type: FETCH_CATEGORIES});
export const fetchToDos = () => ({type: FETCH_TODOS});
export const createCategory = (category) => ({type: CREATE_CATEGORY, payload: category});
export const createToDo = (todo) => ({type: CREATE_TODO, payload: todo});
export const togglePerformed = (todo) => ({type: TOGGLE_PERFORMED, payload: todo});
export const deleteToDo = (id) => ({type: DELETE_TODO, payload: id});

export const fetchCategoriesEpic = (action$, state$) =>
    action$.pipe(
        ofType(FETCH_CATEGORIES),
        switchMap(() =>
            graphQlQuery(`
        {
            categories {
                name
            }
        }`, {}, state$.value)
                .pipe(
                    map(response => setCategories(response.data.categories))
                )
        )
    );

export const fetchToDosEpic = (action$, state$) =>
    action$.pipe(
        ofType(FETCH_TODOS),
        switchMap(() =>
            graphQlQuery(`
        {
             todos {
                 id
                 task
                 isPerformed
                 categoryName
                 dateToPerform
            }
        }
    `, {}, state$.value)
                .pipe(
                    map(response => setToDos(response.data.todos))
                )
        )
    );

export const createToDoEpic = (action$, state$) =>
    action$.pipe(
        ofType(CREATE_TODO),
        switchMap(action =>
            graphQlQuery(`
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
                {todo: action.payload}, state$.value)
                .pipe(
                    map(response => addToDo(response.data.addToDo))
                )
        )
    );

export const togglePerformedEpic = (action$, state$) =>
    action$.pipe(
        ofType(TOGGLE_PERFORMED),
        switchMap(action =>
            graphQlQuery(`
        mutation TogglePerformed($todo: ToDoInputType!){
             handlePerformed( handlePerformed: $todo ){
               id
               isPerformed
             }
            }
         `,
                {todo: action.payload}, state$.value)
                .pipe(
                    map(response => toggleToDo(response.data.handlePerformed))
                )
        )
    );

export const deleteToDoEpic = (action$, state$) =>
    action$.pipe(
        ofType(DELETE_TODO),
        switchMap(action =>
            graphQlQuery(`
         mutation DeleteToDo($id: ID!){
             deleteToDo(id: $id)
             }
          `,
                {id: action.payload}, state$.value)
                .pipe(
                    map(() => removeToDo(action.payload))
                )
        )
    );

export const createCategoryEpic = (action$, state$) =>
    action$.pipe(
        ofType(CREATE_CATEGORY),
        switchMap(action =>
            graphQlQuery(`
        mutation AddCategory($category: CategoryInputType!) {
            addCategory(category: $category) {
                id
                name
           }
        }
    `,
                {category: action.payload}, state$.value)
                .pipe(
                    map(response => addCategory(response.data.addCategory))
                )
        )
    );

const toDoListSlice = createSlice({
    name: "todolist",
    initialState,
    reducers: {
        changeStorage(state, action) {
            state.storage = action.payload;
        },
        handleAddCategoryFormActive(state) {
            state.addCategoryFormActive = !state.addCategoryFormActive;
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
                    : todo
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

export const epics = [
    fetchCategoriesEpic,
    fetchToDosEpic,
    createCategoryEpic,
    createToDoEpic,
    togglePerformedEpic,
    deleteToDoEpic
];
