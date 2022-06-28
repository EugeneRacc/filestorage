import React from 'react';
import "./file.css"
import directoryLogo from "../../../assets/icon/folder.svg"
import fileLogo from "../../../assets/icon/file.svg"
import {useDispatch, useSelector} from "react-redux";
import {pushToStack, setCurrentDir} from "../../../reducers/fileReducer";
import {downloadFile} from "../../../actions/file";


const File = ({file}) => {

    const dispatch = useDispatch()
    const currentDir = useSelector(state => state.files.currentDir)
    function openDirHandler() {
        if(file.type === "dir") {
            dispatch(pushToStack(currentDir))
            dispatch(setCurrentDir(file.id))
        }
    }

    function downloadHandler(e) {
        e.stopPropagation();
        downloadFile(file);
    }

    return (
        <div className="file" onClick={() => openDirHandler(file)}>
            <img src={file.type === "dir" ? directoryLogo : fileLogo} alt="" className="file__img"/>
            <div className="file__name">{file.name}</div>
            <div className="file__date">{file.date.split('T')[0]}</div>
            <div className="file__size">{file.size}</div>
            {file.type !== "dir" &&
                <button onClick={(e) => downloadHandler(e)}
                    className="file__btn file__download">Download</button>}
            <button className="file__btn file__delete">Delete</button>
        </div>
    );
};

export default File;
