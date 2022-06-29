import React from 'react';
import "./filelist.css"
import {useSelector} from "react-redux";
import File from "./file/File";

const FileList = () => {
    const files = useSelector(state => state.files.files)
        .map(file => <File key={file.id} file={file}/>)

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
            {files}
        </div>
    );
};

export default FileList;
