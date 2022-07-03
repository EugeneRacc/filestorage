import {applyMiddleware, combineReducers, createStore} from "redux";
import {composeWithDevTools} from "redux-devtools-extension";
import userReducer from "./userReducer";
import fileReducer from "./fileReducer";
import  appReducer from "./appReducer"
import thunk from "redux-thunk";
import adminReducer from "./adminReducer";

const rootReducer = combineReducers({
    user: userReducer,
    files: fileReducer,
    app: appReducer,
    users: adminReducer
})

export const store = createStore(rootReducer, composeWithDevTools(applyMiddleware(thunk)))