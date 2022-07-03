import React from 'react';
import personLogo from "../../../../assets/icon/person.svg";
import "./user.css"
import {useDispatch} from "react-redux";
import {setCurrentUser} from "../../../../reducers/adminReducer";

const User = ({user}) => {
    const dispatch = useDispatch()
    function openUserHandler(user) {
        dispatch(setCurrentUser(user.id))
    }

    return (
        <div className="user" onClick={() => openUserHandler(user)}>
            <img src={personLogo} alt="personLogo" className="user__img"/>
            <div className="user__name">{user.email}</div>
            <div className="user__role">{user.roleName}</div>
            <div className="user__uds">{user.usedDiskSpace}</div>
        </div>
    );
};

export default User;
