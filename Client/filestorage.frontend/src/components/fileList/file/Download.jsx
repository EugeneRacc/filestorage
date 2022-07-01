import React from 'react';
import "./download.css";
import { downloadFileByAccessLink} from "../../../actions/file";
import downloadLogo from '../../../assets/icon/download.svg'
async function  DownloadHandler(e) {
    e.stopPropagation();
    await downloadFileByAccessLink(window.location.href);
}

const Download = () => {
    return (
        <div className="download" onClick={e => DownloadHandler(e)}>
            Click to start downloading <img className={downloadLogo} alt="Download the file"/>
        </div>
    );
};

export default Download;
