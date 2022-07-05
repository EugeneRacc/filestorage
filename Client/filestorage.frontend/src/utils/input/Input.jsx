import React from 'react';
import './input.css'


const Input = (props) => {

    return (
            <input onChange={props.onChange}
                   value={props.value}
                   type={props.type}
                   placeholder={props.placeholder}
                   name={props.name}
                   onBlur={props.onBlur}
            />
    );
};

export default Input;
