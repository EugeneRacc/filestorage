import React, {useEffect, useState} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {getFiles, uploadFile} from "../../actions/file";
import FileList from "../fileList/FileList";
import "./disk.css"
import Popup from "./Popup";
import {setCurrentDir, setPopupDisplay} from "../../reducers/fileReducer";

import fileUploadLogo from "../../assets/icon/file_upload.svg";
import {useNavigate} from "react-router-dom";
const Disk = () => {
    const dispatch = useDispatch();
    const currentDir = useSelector(state => state.files.currentDir)
    const loader = useSelector(state => state.app.loader)
    const dirStack = useSelector(state => state.files.dirStack)
    const [dragEnter, setDragEnter] = useState(false)
    const [sort, setSort] = useState('type')
    const isAdmin = useSelector(state => state.user.currentUser.roleName) === "Admin"
    const isCurrentUserNull = useSelector(state => state.users.currentUserId) !== null
    const navigate = useNavigate()
    useEffect(() => {
        dispatch((getFiles(currentDir, sort)))
    }, [currentDir, sort]);


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

    if(loader){
        return (
            <div className="wrapper__loader">
                <span className="loader"></span>
            </div>
        )
    }

    function goBackToUserHandler() {

        navigate("/user-details")
    }

    return ( !dragEnter ?
        <div className="disk" onDragEnter={dragEnterHandler} onDragLeave={dragLeaveHandler} onDragOver={dragEnterHandler}>
            <div className="disk__btn">
                {(isCurrentUserNull && isAdmin)?
                    <button className="disk__back" onClick={() => goBackToUserHandler()}>Back to user</button>:
                    <button className="disk__back" onClick={() => stepBackHandler()}>Back</button>}
                {(!isCurrentUserNull)?
                    <button className="disk__create" onClick={() => showPopupHandler()}>Create Directory</button>:
                    <div></div>}
                {(!isCurrentUserNull)?
                    <div className="disk__upload">
                    <label htmlFor="disk__upload-input" className="disk__upload-label">
                        <img src={fileUploadLogo} alt="upload_file" className="disk__upload-img"/>
                        <p>File Upload</p></label>
                    <input multiple={true} onChange={(event) => fileUploadHandler(event)}
                        type="file" id="disk__upload-input" className="disk__upload-input" />
                </div>:
                    <div></div>}
                {(!isCurrentUserNull)?
                <select value={sort} onChange={(e) => setSort(e.target.value)} className="disk__select">
                    <option value="name">Sort by name</option>
                    <option value="type">Sort by type</option>
                    <option value="date">Sort by date</option>
                </select>:
                    <div></div>}
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
