import React, {useState} from 'react';
import "./file.css"
import directoryLogo from "../../../assets/icon/folder.svg"
import fileLogo from "../../../assets/icon/file.svg"
import shareLogo from "../../../assets/icon/share.svg"
import {useDispatch, useSelector} from "react-redux";
import {pushToStack, setCurrentDir, setSpanDisplay} from "../../../reducers/fileReducer";
import {deleteFile, downloadFile} from "../../../actions/file";
import { CopyToClipboard } from "react-copy-to-clipboard";
import {CSSTransition} from "react-transition-group";

const File = ({file}) => {

    const dispatch = useDispatch()
    const currentDir = useSelector(state => state.files.currentDir)
    const [showSpan, setShowSpan] = useState(false)
    const spanDisplay = useSelector(state => state.files.spanDisplay)
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

    function deleteFileHandler(event) {
        event.stopPropagation();
        dispatch(deleteFile(file))
    }

    function setSpanHandler(){
        setShowSpan(true)
        const timer = setTimeout(() => {
            setShowSpan(false)
        }, 3000);
        return () => clearTimeout(timer);
    }
    return (
        <div className="file" onClick={() => openDirHandler(file)}>
            <img src={file.type === "dir" ? directoryLogo : fileLogo} alt="" className="file__img"/>
            <div className="file__name">{file.name}</div>
            {file.type !== "dir" && <CopyToClipboard text={`http://localhost:3000/share/${file.accessLink}`}>
                <div onClick={() => setSpanHandler()} className="file__share">
                    <CSSTransition in={showSpan} classNames="alert" timeout={300} unmountOnExit>
                        <span className="file__span">Copied to clipboard</span>
                    </CSSTransition>
                    <img src={shareLogo} className="file__share-logo" alt="share"/></div>
            </CopyToClipboard>}
            <div className="file__date">{file.date.split('T')[0]}</div>
            <div className="file__size">{file.size}</div>
            {file.type !== "dir" &&
                <button onClick={(e) => downloadHandler(e)}
                    className="file__btn file__download">Download</button>}
            <button onClick={(e) => deleteFileHandler(e)} className="file__btn file__delete">Delete</button>
        </div>

    );
};

export default File;
