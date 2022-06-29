import axios from "axios";
import {setUser} from "../reducers/userReducer";

export const registration = async (email, password, confirmPassword) => {
        if(confirmPassword !== password) {
            alert("Passwords aren't same");
            return;
        }

    try{
        const response = await axios.post('https://localhost:44368/api/1.0/register' ,
            {
                email,
                password
            })
        alert(response.data + "Correctly registered");
    }
    catch (e){
        alert(e.response.data + " smh went wrong");
    }
}

export const login = (email, password) => {
    return async dispatch => {
        try {
            const response = await axios.post('https://localhost:44368/api/1.0/login',
                {
                    email,
                    password
                })
            //console.log(typeof response + " " + response)
                dispatch(setUser(response.data.user));
                localStorage.setItem('token', response.data.token);
                console.log(response.data);
        } catch (e) {
            alert(e.response.data + " smh went wrong");
        }
    }
}

export const auth = () => {
    return async dispatch => {
        try {
            const response = await axios.get('https://localhost:44368/api/1.0/auth',
                {headers:{Authorization:`Bearer ${localStorage.getItem('token')}`}})
            //console.log(typeof response + " " + response)
            dispatch(setUser(response.data.user));
            localStorage.setItem('token', response.data.token);
            console.log(response.data);
        } catch (e) {
            console.log(e.response)
            localStorage.removeItem('token')
        }
    }
}