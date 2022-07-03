import React from 'react';
import personLogo from "../../../../assets/icon/person.svg";
import "./user.css"

const User = ({user}) => {
    return (
        <div className="user">
            <img src={personLogo} alt="personLogo" className="user__img"/>
            <div className="user__name">{user.email}</div>
            <div className="user__role">{user.roleName}</div>
            <div className="user__uds">{user.usedDiskSpace}</div>
        </div>
    );
};

export default User;
