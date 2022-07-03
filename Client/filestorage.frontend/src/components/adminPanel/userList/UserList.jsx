import React from 'react';
import {useSelector} from "react-redux";
import User from "./user/User";
import "./userList.css"


const UserList = () => {
    const users = useSelector(state => state.users.users)
       .map(user => <User key={user.id} user={user}/>)

    if (users.length === 0){
        return (
            <div className="notfound">Files not found</div>
        )
    }
    return (
        <div className="user-list">
            <div className="user-list__header">
                <div className="user-list__name">Email</div>
                <div className="user-list__role">Role</div>
                <div className="user-list__uds">Used disk space</div>
            </div>
            {users}
        </div>
    );
};

export default UserList;
