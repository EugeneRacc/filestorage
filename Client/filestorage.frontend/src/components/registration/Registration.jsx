import React, {useState} from 'react';
import './registration.css'
import Input from "../../utils/input/Input";
import {registration} from "../../actions/user";

const Registration = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");

    return (
        <div className="registration">
            <div className="registration__header">Registration</div>
            <Input value={email} setValue={setEmail} type="text" placeholder="Email"/>
            <Input value={password} setValue={setPassword} type="password" placeholder="Password"/>
            <Input value={confirmPassword} setValue={setConfirmPassword} type="password" placeholder="Confirm Password"/>
            <button className="registration__btn" onClick={() => registration(email, password, confirmPassword)}>
                Sign Up
            </button>
        </div>
    );
};

export default Registration;
