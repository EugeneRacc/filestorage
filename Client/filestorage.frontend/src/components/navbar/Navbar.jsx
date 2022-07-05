import React, {useState} from 'react';
import './navbar.css'
import Storage from '../../assets/icon/hard-drive.png'
import {NavLink, useNavigate} from "react-router-dom";
import {useDispatch, useSelector} from "react-redux";
import {logout} from "../../reducers/userReducer";
import {getFiles, searchFiles} from "../../actions/file";
import {showLoader} from "../../reducers/appReducer";
const Navbar = () => {
    const navigate = useNavigate()
    const isAuth = useSelector(state => state.user.isAuth);
    const dispatch = useDispatch()
    const [searchName, setSearchName] = useState('')
    const [searchTimeout, setSearchTimeout] = useState(false)
    const currentDir = useSelector(state => state.files.currentDir)
    const isAdmin = useSelector(state => state.user.currentUser.roleName);
    function searchHandler(e) {
        setSearchName(e.target.value)
        if(searchTimeout !== false){
            clearTimeout(searchTimeout)
        }
        dispatch(showLoader())
        if(e.target.value !== "") {
            setSearchTimeout(setTimeout((value) => {
                dispatch(searchFiles(value))
            }, 1000, e.target.value))
        }
        else{
            dispatch(getFiles(currentDir))
        }
    }

    return (
        <div className="navbar">
            <div className="container">
                <img src={Storage} alt="FileStorage" className="navbar__logo"/>
                <div className="navbar__header" onClick={() => navigate("/")}>Best File Storage</div>
                {isAuth && <input
                    value={searchName}
                    onChange={e => searchHandler(e)}
                    type="text"
                    placeholder="Search by Name"
                    className="navbar__search"
                />}
                {!isAuth && <div className="navbar__login"><NavLink to="/login">Sign In </NavLink></div> }
                {!isAuth && <div className="navbar__registration"><NavLink to="/registration">Sign Up</NavLink></div>}
                {(isAuth && isAdmin === "Admin")&& <div className="navbar__admin-panel"><NavLink to="/admin-panel">Admin Panel</NavLink></div> }
                {isAuth && <div className="navbar__login" onClick={() => dispatch(logout())}>Sign Out</div> }
            </div>
        </div>
    );
};

export default Navbar;
