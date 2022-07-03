import React from 'react';
import "./download.css";
import { downloadFileByAccessLink} from "../../../actions/file";
import downloadLogo from '../../../assets/icon/download.svg'
import {useNavigate} from "react-router-dom"

async function  DownloadHandler(e, nav) {
    e.stopPropagation();
    await downloadFileByAccessLink(window.location.href);
    const timer = setTimeout(() => {
        nav("/");
    }, 3000);
    return () => clearTimeout(timer);
}

const Download = () => {
    const navigate = useNavigate();
    return (
        <div className="download" onClick={e => DownloadHandler(e, navigate)}>
            Click to start downloading <img src={downloadLogo} className="download__icon" alt="Download the file"/>
        </div>
    );
};

export default Download;
