import React from 'react';
import "./filelist.css"
import {useSelector} from "react-redux";
import File from "./file/File";

const FileList = () => {
    const files = useSelector(state => state.files.files)
        .map(file => <File key={file.id} file={file}/>)
    const isAdmin = useSelector(state => state.user.currentUser.roleName) === "Admin"
    const isCurrentUserNull = useSelector(state => state.users.currentUserId) !== null
    const userFiles = useSelector(state => state.users.userFiles)
        .map(file => <File key={file.id} file={file} />)
    if (files.length === 0){
        return (
            <div className="notfound">Files not found</div>
        )
    }


    return (
        <div className="filelist">
            <div className="filelist__header">
                <div className="filelist__name">Name</div>
                <div className="filelist__date">Date</div>
                <div className="filelist__size">Size</div>
            </div>
            {(isAdmin && isCurrentUserNull) ? userFiles : files }
        </div>
    );
};

export default FileList;
