import React, {useState, useEffect} from 'react';
import {useDispatch, useSelector} from "react-redux";
import "./adminPanel.css"
import {getUserById, getUsers} from "../../actions/admin";
import UserList from "./userList/UserList";
import {showLoader} from "../../reducers/appReducer";
import {useNavigate} from "react-router-dom";


const AdminPanel = () => {
    const dispatch = useDispatch();
    const currentUser = useSelector(state => state.users.currentUserId)
    const [sort, setSort] = useState('name')
    const isAdmin = useSelector(state => state.user.currentUser.roleName) === "Admin"
    const [searchName, setSearchName] = useState('')
    const [searchTimeout, setSearchTimeout] = useState(false)
    const navigate = useNavigate()
    useEffect(() => {
        dispatch((getUsers(sort)))
    }, [sort]);

    useEffect(() => {
        dispatch((getUserById(currentUser)))
        if(currentUser !== null) {
            navigate("/user-details")
        }
        if(currentUser === null){
            navigate("/admin-panel")
        }
    }, [currentUser]);

    function searchUserHandler(e) {
        setSearchName(e.target.value)
        if(searchTimeout !== false){
            clearTimeout(searchTimeout)
        }
        dispatch(showLoader())
        if(e.target.value !== "") {
            setSearchTimeout(setTimeout((value) => {
                dispatch(getUsers(sort, value))
            }, 500, e.target.value))
        }
        else{
            dispatch(getUsers(sort))
        }
    }

    return (
        <div className="admin-panel">
            <div className="admin-panel__btn">
                 <select value={sort} onChange={(e) => setSort(e.target.value)} className="admin-panel__select">
                    <option value="name">Sort by name</option>
                    <option value="role">Sort by role</option>
                </select>
                {isAdmin && <input
                    value={searchName}
                    onChange={e => searchUserHandler(e)}
                    type="text"
                    placeholder="Search by Name"
                    className="admin-panel__search"
                />}
            </div>
            <UserList />
        </div>
    );
};

export default AdminPanel;
