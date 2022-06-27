import React, {useEffect} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {createDir, getFiles} from "../../actions/file";
import FileList from "../fileList/FileList";
import "./disk.css"
import Popup from "./Popup";
import {setCurrentDir, setPopupDisplay} from "../../reducers/fileReducer";

const Disk = () => {
    const dispatch = useDispatch();
    const currentDir = useSelector(state => state.files.currentDir)
    const dirStack = useSelector(state => state.files.dirStack)

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

    return (
        <div className="disk">
            <div className="disk__btn">
                <button className="disk__back" onClick={() => stepBackHandler()}>Back</button>
                <button className="disk__create" onClick={() => showPopupHandler()}>Create Directory</button>
            </div>
            <FileList />
            <Popup />
        </div>
    );
};

export default Disk;
