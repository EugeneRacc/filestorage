'use strict'
import axios from "axios";
import {addFile, deleteFileReducer, setFiles} from "../reducers/fileReducer";
import {hideLoader, showLoader} from "../reducers/appReducer";


export function getFiles(dirId, sort){
    return async dispatch => {
        try{
            dispatch(showLoader())
            let url = `https://localhost:44368/api/1.0/file`;
            if(dirId){
                 url = `https://localhost:44368/api/1.0/file/${dirId}`
            }
            if(sort){
                 url = `https://localhost:44368/api/1.0/file?sortType=${sort}`
            }
            if(sort && dirId){
                 url = `https://localhost:44368/api/1.0/file/${dirId}?sortType=${sort}`
            }
            const response = await axios.get(url, {
                    headers: {Authorization: `Bearer ${localStorage.getItem('token')}`}
                });
            console.log(response.data)
            dispatch(setFiles(response.data))
        }
        catch (e){

        }
        finally {
            dispatch(hideLoader())
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

export function deleteFile(file){

    return async dispatch => {
        try{
            const response = await axios.delete(`https://localhost:44368/api/1.0/file?modelId=${file.id}`,
                {headers: {
                    Authorization: `Bearer ${localStorage.getItem('token')}`
        }});
            dispatch(deleteFileReducer(file.id))
            alert(response.data)
        }
        catch (e){
            console.log("Something went wrong")
        }
    }
}


export function searchFiles(search){

    return async dispatch => {
        try{
            const response = await axios.get(`https://localhost:44368/api/1.0/file/filter?fileName=${search}`,
                {headers: {
                        Authorization: `Bearer ${localStorage.getItem('token')}`
                    }});
            dispatch(setFiles(response.data))
        }
        catch (e){
            console.log("Something went wrong")
        }
        finally {
            dispatch(hideLoader())
        }
    }
}