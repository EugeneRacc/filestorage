import React, {useState} from 'react';
import Input from "../../utils/input/Input";
import "./adminPopup.css"
import {useDispatch, useSelector} from "react-redux";
import {setPopupDisplay} from "../../reducers/fileReducer";
import {updateUserInfo} from "../../actions/admin";

const Popup = () => {
    const currentUser = useSelector(state => state.users.currentUserId)
    const [role, setRole] = useState(currentUser.roleName)
    const [email, setEmail] = useState(currentUser.email)
    const popupDisplay = useSelector(state => state.files.popupDisplay)
    const dispatch = useDispatch()
    function createHandler() {
        updateUserInfo(currentUser, email, role)
        dispatch(setPopupDisplay('none'));
    }

    function closeHandler() {

        dispatch(setPopupDisplay('none'));
    }

    return (
        <div className="popup" onClick={() => closeHandler()} style={{display: popupDisplay}}>
            <div className="popup__content" onClick={(event => event.stopPropagation())}>
                <div className="popup__header">
                    <div className="popup__title">Change information</div>
                    <button className="popup__close" onClick={() => closeHandler()}>X</button>
                </div>
                <p>New Email</p>
                <Input onChange={e => setEmail(e.target.value)} type="text" placeholder="New email" value={email} setValue={setEmail}/>
                <div className="popup__select">
                <p>Change Role</p>
                    <select value={role} onChange={(e) => setRole(e.target.value)} className="disk__select">
                        <option value="Admin">Admin</option>
                        <option value="User">User</option>
                    </select>
                </div>
                <button className="popup__create" onClick={() => createHandler()}>Update</button>
            </div>
        </div>
    );
};

export default Popup;
