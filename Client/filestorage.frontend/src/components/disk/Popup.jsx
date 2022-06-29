import React, {useState} from 'react';
import Input from "../../utils/input/Input";
import "./disk.css"
import {useDispatch, useSelector} from "react-redux";
import {setPopupDisplay} from "../../reducers/fileReducer";
import {createDir} from "../../actions/file";
const Popup = () => {
    const [dirName, setDirName] = useState('')
    const popupDisplay = useSelector(state => state.files.popupDisplay)
    const currentDir = useSelector(state => state.files.currentDir)
    const dispatch = useDispatch()

    function createHandler() {
        dispatch(createDir(currentDir, dirName));
        dispatch(setPopupDisplay('none'));
        setDirName('');
    }

    function closeHandler() {

        dispatch(setPopupDisplay('none'));
        setDirName('');
    }

    return (
        <div className="popup" onClick={() => closeHandler()} style={{display: popupDisplay}}>
            <div className="popup__content" onClick={(event => event.stopPropagation())}>
                <div className="popup__header">
                    <div className="popup__title">Create new directory</div>
                    <button className="popup__close" onClick={() => closeHandler()}>X</button>
                </div>
                <Input type="text" placeholder="Name of new folder" value={dirName} setValue={setDirName}/>
                <button className="popup__create" onClick={() => createHandler()}>Create</button>
            </div>
        </div>
    );
};

export default Popup;
