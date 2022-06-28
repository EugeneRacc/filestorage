import React, {useEffect, useState} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {createDir, getFiles, uploadFile} from "../../actions/file";
import FileList from "../fileList/FileList";
import "./disk.css"
import Popup from "./Popup";
import {setCurrentDir, setPopupDisplay} from "../../reducers/fileReducer";

import fileUploadLogo from "../../assets/icon/file_upload.svg";

const Disk = () => {
    const dispatch = useDispatch();
    const currentDir = useSelector(state => state.files.currentDir)
    const dirStack = useSelector(state => state.files.dirStack)
    const [dragEnter, setDragEnter] = useState(false)

    useEffect(() => {
        dispatch((getFiles(currentDir)))
    }, [currentDir]);


    function showPopupHandler() {
        dispatch(setPopupDisplay("flex"))
    }

    function stepBackHandler() {
        const backDirId = dirStack.pop()
        dispatch(setCurrentDir(backDirId))
    }

    function fileUploadHandler(event) {
        const files = [...event.target.files]
        files.forEach(file => dispatch(uploadFile(file, currentDir)))
    }

    function dragEnterHandler(event){
        event.preventDefault();
        event.stopPropagation()
        setDragEnter(true)
    }

    function dragLeaveHandler(event){
        event.preventDefault();
        event.stopPropagation()
        setDragEnter(false)
    }


    function dropHandler(event) {
        event.preventDefault();
        event.stopPropagation()
        let files = [...event.dataTransfer.files]
        files.forEach(file => dispatch(uploadFile(file, currentDir)))
        setDragEnter(false)
    }

    return ( !dragEnter ?
        <div className="disk" onDragEnter={dragEnterHandler} onDragLeave={dragLeaveHandler} onDragOver={dragEnterHandler}>
            <div className="disk__btn">
                <button className="disk__back" onClick={() => stepBackHandler()}>Back</button>
                <button className="disk__create" onClick={() => showPopupHandler()}>Create Directory</button>
                <div className="disk__upload">
                    <label htmlFor="disk__upload-input" className="disk__upload-label">
                        <img src={fileUploadLogo} alt="upload_file" className="disk__upload-img"/>
                        <p>File Upload</p></label>
                    <input multiple={true} onChange={(event) => fileUploadHandler(event)}
                        type="file" id="disk__upload-input" className="disk__upload-input" />
                </div>
            </div>
            <FileList />
            <Popup />
        </div>
            :
            <div className="drop-area" onDrop={dropHandler} onDragEnter={dragEnterHandler} onDragLeave={dragLeaveHandler} onDragOver={dragEnterHandler}>
                Drag files
            </div>
    );
};

export default Disk;
