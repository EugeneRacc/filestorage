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

export function uploadFile(file, dirId){
    return async dispatch => {
        try{
            const formData = new FormData()
            formData.append("uploadedFile", file)
            if(dirId){
                formData.append("parentId", dirId)
            }
            const response = await axios.post(
                `https://localhost:44368/api/1.0/file/upload`, formData,{
                    headers: {Authorization: `Bearer ${localStorage.getItem('token')}`},
                    onUploadProgress: progressEvent => {
                        const totalLength = progressEvent.lengthComputable ? progressEvent.total : progressEvent.target.getResponseHeader('content-length') || progressEvent.target.getResponseHeader('x-decompressed-content-length');
                        console.log('total', totalLength)
                        if(totalLength){
                            let progress = Math.round((progressEvent.loaded * 100)/totalLength);
                            console.log(progress);
                        }
                    }
                });

            console.log(response.data)
            dispatch(addFile(response.data))
        }
        catch (e){
            alert(e.response.data.message)
        }
    }
}

export async function downloadFile(file){
    const response = await fetch(`https://localhost:44368/api/1.0/file/download?id=${file.id}`,
        {headers: {
                Authorization: `Bearer ${localStorage.getItem('token')}`
            }
        })
    if(response.status === 200){
        const blob = await response.blob()
        const downloadUrl = window.URL.createObjectURL(blob)
        const link = document.createElement('a')
        link.href = downloadUrl
        link.download = file.name
        document.body.appendChild(link)
        link.click()
        link.remove()
    }
}