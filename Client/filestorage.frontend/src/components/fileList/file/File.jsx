import React from 'react';
import "./file.css"
import directoryLogo from "../../../assets/icon/folder.svg"
import fileLogo from "../../../assets/icon/file.svg"
import {useDispatch, useSelector} from "react-redux";
import {pushToStack, setCurrentDir} from "../../../reducers/fileReducer";


const File = ({file}) => {

    const dispatch = useDispatch()
    const currentDir = useSelector(state => state.files.currentDir)
    function openDirHandler() {
        dispatch(pushToStack(currentDir))
        dispatch(setCurrentDir(file.id))
    }

    return (
        <div className="file" onClick={file.type === "dir" ? () => openDirHandler() : ""}>
            <img src={file.type === "dir" ? directoryLogo : fileLogo} alt="" className="file__img"/>
            <div className="file__name">{file.name}</div>
            <div className="file__date">{file.date.split('T')[0]}</div>
            <div className="file__size">{file.size}</div>
        </div>
    );
};

export default File;
