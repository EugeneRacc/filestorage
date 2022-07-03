import React, {useEffect} from 'react';
import "./userDetails.css"
import {useDispatch, useSelector} from "react-redux";
import {setCurrentUser} from "../../../../../reducers/adminReducer";
import {getUserById} from "../../../../../actions/admin";
import {useNavigate} from "react-router-dom";
const UserDetails = () => {
    const dispatch = useDispatch()
    const currentUser = useSelector(state => state.users.currentUserId)
    const navigate = useNavigate()
    useEffect(() => {
        dispatch((getUserById(currentUser)))
        if(currentUser !== null) {
            navigate("/user-details")
        }
        if(currentUser === null){
            navigate("/admin-panel")
        }
    }, [currentUser]);
    function stepBackHandler() {
        dispatch(setCurrentUser(null))
    }
    return (
        <div className="user-details">
            <div className="user-details__btn">
                <button className="user-details__back" onClick={() => stepBackHandler()}>Back</button>
            </div>
        </div>
    );
};

export default UserDetails;
