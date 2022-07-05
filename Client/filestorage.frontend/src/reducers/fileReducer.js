const SET_FILES = "SET_FILES";
const SET_CURRENT_DIR = "SET_CURRENT_DIR";
const ADD_FILE = "ADD_FILE";
const SET_POPUP_DISPLAY = "SET_POPUP_DISPLAY"
const PUSH_TO_STACK = "PUSH_TO_STACK"
const DELETE_FILE = "DELETE_FILE"
const SET_SPAN_DISPLAY = "SET_SPAN_DISPLAY"
const defaultState = {
    files:[],
    currentDir: null,
    popupDisplay: 'none',
    dirStack: [],
    spanDisplay: 'none'
}

export default function fileReducer(state = defaultState, action){
    switch (action.type){
        case SET_FILES:
            return {...state, files: action.payload};
        case SET_CURRENT_DIR:
            return {...state, currentDir: action.payload};
        case ADD_FILE:
            return {...state, files: [...state.files, action.payload]};
        case SET_POPUP_DISPLAY:
            return {...state, popupDisplay: action.payload};
        case PUSH_TO_STACK:
            return {...state, dirStack: [...state.dirStack, action.payload]};
        case DELETE_FILE:
            return {...state, files: [...state.files.filter(file => file.id != action.payload)]}
        case SET_SPAN_DISPLAY:
            return {...state, spanDisplay: action.payload}
        default:
            return state;
    }
}

export const setFiles = (files) => ({type: SET_FILES, payload: files})
export const setCurrentDir = (dir) => ({type: SET_CURRENT_DIR, payload: dir})
export const addFile = (file) => ({type: ADD_FILE, payload: file})
export const setPopupDisplay = (display) => ({type: SET_POPUP_DISPLAY, payload: display})
export const pushToStack = (dir) => ({type: PUSH_TO_STACK, payload: dir})
export const deleteFileReducer = (dirId) => ({type: DELETE_FILE, payload: dirId})
export const setSpanDisplay = (display) => ({type: SET_SPAN_DISPLAY, payload: display})