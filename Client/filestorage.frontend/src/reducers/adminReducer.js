const SET_USERS = "SET_USERS"
const SET_CURRENT_USER_ID = "SET_CURRENT_USER"
const SET_USER_FILES = "SET_USER_FILES"
const defaultState = {
    users:[],
    currentUserId: null,
    userFiles: [],
}

export default function adminReducer(state = defaultState, action){
    switch (action.type){
        case SET_USERS:
            return {...state, users: action.payload};
        case SET_CURRENT_USER_ID:
            return {...state, currentUserId: action.payload};
        case SET_USER_FILES:
            return {...state, userFiles: action.payload}
        default:
            return state;
    }
}

export const setUsers = (users) => ({type: SET_USERS, payload: users})
export const setCurrentUser = (user) => ({type: SET_CURRENT_USER_ID, payload: user})
export const setUserFiles = (files) => ({type: SET_USER_FILES, payload: files})