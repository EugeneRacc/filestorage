import React, {useState, useEffect} from 'react';
import {useDispatch} from "react-redux";
import "./adminPanel.css"
import {getUsers} from "../../actions/admin";
import UserList from "./userList/UserList";



const AdminPanel = () => {
    const dispatch = useDispatch();
    const [sort, setSort] = useState('name')
    useEffect(() => {
        dispatch((getUsers(sort)))
    }, [sort]);

    return (
        <div className="admin-panel">
            <div className="admin-panel__btn">
                 <select value={sort} onChange={(e) => setSort(e.target.value)} className="admin-panel__select">
                    <option value="name">Sort by name</option>
                    <option value="role">Sort by role</option>
                </select>
            </div>
            <UserList />
        </div>
    );
};

export default AdminPanel;
