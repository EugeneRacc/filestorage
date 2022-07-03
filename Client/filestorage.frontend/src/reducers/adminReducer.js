const SET_USERS = "SET_USERS"

const defaultState = {
    users:[],
    currentUserId: null,
    userFiles: []
}

export default function adminReducer(state = defaultState, action){
    switch (action.type){
        case SET_USERS:
            return {...state, users: action.payload};
        default:
            return state;
    }
}

export const setUsers = (users) => ({type: SET_USERS, payload: users})
