import {applyMiddleware, combineReducers, createStore} from "redux";
import {composeWithDevTools} from "redux-devtools-extension";
import userReducer from "./userReducer";
import fileReducer from "./fileReducer";
import  appReducer from "./appReducer"
import thunk from "redux-thunk";

const rootReducer = combineReducers({
    user: userReducer,
    files: fileReducer,
    app: appReducer
})

export const store = createStore(rootReducer, composeWithDevTools(applyMiddleware(thunk)))