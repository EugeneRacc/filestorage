import React, { FC, ReactElement, useRef, useEffect, useState } from 'react';
import {  Client } from '../api/api';
import { FormControl } from 'react-bootstrap';

const apiClient = new Client('https://localhost:44353');


const NoteList: FC<{}> = (): ReactElement => {

    return (
        <div>

        </div>
    );
};
export default NoteList;