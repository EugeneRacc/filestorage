import React, {useEffect, useState} from 'react';
import './registration.css'
import Input from "../../utils/input/Input";
import {registration} from "../../actions/user";
import {useNavigate} from "react-router-dom";
let passwd = ""
const Registration = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [emailDirty, setEmailDirty] = useState(false);
    const [passwordDirty, setPasswordDirty] = useState(false);
    const [confirmDirty, setConfirmDirty] = useState(false);
    const [emailError, setEmailError] = useState("Email can't be empty")
    const [passwordError, setPasswordError] = useState("Password can't be empty")
    const [confirmError, setConfirmError] = useState("Password can't be empty");
    const [formValid, setFormValid] = useState(false)
    const navigate = useNavigate();

    useEffect(() => {
       if(emailError || passwordError || confirmError){
            setFormValid(false)
       }else
       {
            setFormValid(true)
       }
    }, [emailError, passwordError, confirmError]);
    
    const blurHandler = (e) => {
        switch (e.target.name){
            case "email":
                setEmailDirty(true)
                break
            case "password":
                setPasswordDirty(true)
                break
            case "confirm":
                setConfirmDirty(true)
                break
        }
    }

    const emailHandler = (e) => {
        setEmail(e.target.value)
        const re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
        if(!re.test(String(e.target.value).toLowerCase())){
            setEmailError("Email is incorrect")
        }
        else{
            setEmailError("")
        }
    }

    const passwordHandler = (e) => {
        setPassword(e.target.value)
        if(e.target.value.length < 8){
            setPasswordError("Password should be longer than 8 characters")
            if(!e.target.value){
                setPasswordError("Password can't be empty")
            }
        }
        else{
            setPasswordError("")
        }
        passwd = e.target.value.toString()
    }

    const confirmHandler = (e) => {
        setConfirmPassword(e.target.value)
        if( e.target.value !== passwd){
            setConfirmError("Passwords aren't same")
            if(!e.target.value){
                setConfirmError("Password can't be empty")
            }
        }
        else{
            setConfirmError("")
        }
    }

    return (
        <div className="registration">
            <div className="registration__header">Registration</div>
            {(emailDirty && emailError) && <p style={{color: 'red'}}>{emailError}</p>}
            <Input onChange={e => emailHandler(e)}
                onBlur={e => blurHandler(e)} name="email" value={email} setValue={setEmail} type="text" placeholder="Email"/>
            {(passwordDirty && passwordError) && <span style={{color: 'red'}}>{passwordError}</span>}
            <Input onChange={e => passwordHandler(e)}
                onBlur={e => blurHandler(e)} name="password" value={password} setValue={setPassword} type="password" placeholder="Password"/>
            {(confirmDirty && confirmError) && <span style={{color: 'red'}}>{confirmError}</span>}
            <Input onChange={e => confirmHandler(e)}
                onBlur={e => blurHandler(e)} name="confirm" value={confirmPassword} setValue={setConfirmPassword} type="password" placeholder="Confirm Password"/>
            <button disabled={!formValid} className="registration__btn" onClick={() => registration(email, password, confirmPassword, navigate)}>
                Sign Up
            </button>
        </div>
    );
};

export default Registration;
