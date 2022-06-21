import {useEffect, FC, ReactNode} from 'react';
import { useNavigate } from 'react-router-dom';
import { signinRedirectCallback } from './user-service';

const SigninOidc= () => {
    const history = useNavigate();
    useEffect(() => {
        async function signinAsync() {
            await signinRedirectCallback();
            history('/');
        }
        signinAsync();
    }, [history]);
    return <div>Redirecting...</div>;
};

export default SigninOidc;