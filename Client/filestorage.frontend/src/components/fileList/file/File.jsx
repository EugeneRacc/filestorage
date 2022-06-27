import React from 'react';
import "./file.css"
import directoryLogo from "../../../assets/icon/folder.svg"
import fileLogo from "../../../assets/icon/file.svg"

const File = ({file}) => {
    return (
        <div className="file">
            <img src={file.type === "dir" ? directoryLogo : fileLogo} alt="" className="file__img"/>
            <div className="file__name">{file.name}</div>
            <div className="file__date">{file.date.split('T')[0]}</div>
            <div className="file__size">{file.size}</div>
        </div>
    );
};

export default File;
