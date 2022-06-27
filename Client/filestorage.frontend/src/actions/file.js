import axios from "axios";
import {addFile, setFiles} from "../reducers/fileReducer";

export function getFiles(dirId){
    return async dispatch => {
        try{
            const response = await axios.get(
                `https://localhost:44368/api/1.0/file${dirId ? '/'+dirId : ''}`, {
                    headers: {Authorization: `Bearer ${localStorage.getItem('token')}`}
                });
            console.log(response.data)
            dispatch(setFiles(response.data))
        }
        catch (e){
            alert(e.response.data.message)
        }
    }
}

export function createDir(dirId, name){
    return async dispatch => {
        try{
            const response = await axios.post(
                `https://localhost:44368/api/1.0/file`, {
                    Name: name,
                    ParentId: dirId,
                    Type: 'dir'
                },{
                    headers: {Authorization: `Bearer ${localStorage.getItem('token')}`}
                });

            console.log(response.data)
            dispatch(addFile(response.data))
        }
        catch (e){
            alert(e.response.data.message)
        }
    }
}