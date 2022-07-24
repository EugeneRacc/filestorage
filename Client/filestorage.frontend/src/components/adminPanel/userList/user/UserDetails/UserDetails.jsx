import React, {useEffect} from 'react';
import "./userDetails.css"
import {useDispatch, useSelector} from "react-redux";
import {setCurrentUser} from "../../../../../reducers/adminReducer";
import {useNavigate} from "react-router-dom";
import avatarIcon from "../../../../../assets/icon/person.svg"
import {getUserFiles} from "../../../../../actions/admin";
import {setPopupDisplay} from "../../../../../reducers/fileReducer";
import AdminPopup from "../../../AdminPopup";
import sizeFormat from "../../../../../utils/sizeFormat";

const UserDetails = () => {
    const dispatch = useDispatch()
    const currentUser = useSelector(state => state.users.currentUserId)
    const navigate = useNavigate()
    useEffect(() => {

        if(currentUser === null){
            navigate("/admin-panel")
        }
        else{
            navigate("/user-details")
        }
    }, [currentUser]);

    function stepBackToUsersHandler() {
        dispatch(setCurrentUser(null))
    }

    function getUserFilesHandler() {
        dispatch(getUserFiles(null, null, currentUser.id))
        navigate("/user-files")
    }

    function changeInfoHandler() {
        dispatch(setPopupDisplay("flex"))
    }

    return (

            <div className="user-details">
                <div className="user-details__btn">
                    <button className="user-details__back" onClick={() => stepBackToUsersHandler()}>Back</button>
                </div>
                {currentUser !== null ?
                <div className="user-details__profile">
                    <div className="user-details__avatar">
                        <img src={avatarIcon} alt="avatar" className="user-details__img"/>
                    </div>
                    <div className="user-details__card">
                        <div className="user-details__block"><span className="user-details__email">Email: </span>{currentUser.email}</div>
                        <div className="user-details__block"><span className="user-details__role">Role: </span>{currentUser.roleName}</div>
                        <div className="user-details__block"><span className="user-details__uds">Used disk space: </span>{sizeFormat(currentUser.usedDiskSpace)}</div>
                        <div className="user-details__block"><span className="user-details__disk">Disk space: </span>
                        {sizeFormat(currentUser.availableDiskSpace) === null
                            ? 0
                            : sizeFormat(currentUser.availableDiskSpace)}</div>
                        {currentUser.filesIds !== undefined ?
                            <div className="user-details__block"><span className="user-details__amount">Number of files:
                                <span className="user-details__inner"> {currentUser.filesIds.length}
                                </span>
                            </span></div>
                            :
                            <div className="user-details__block"><span className="user-details__amount">Number of files: </span></div>
                        }
                    </div>
                </div>
                    :
                    <div></div>}

                <div className="user-details__btn">
                    <button className="user-details__btn-details" onClick={() => getUserFilesHandler()}>Get Files</button>
                    <button className="user-details__btn-details" onClick={() => changeInfoHandler()}>Change info</button>
                </div>
                <div className="user-details__admin-popup">
                    {currentUser && <AdminPopup />}
                </div>
            </div>



    );
};

export default UserDetails;
