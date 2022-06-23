import axios from "axios";

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
        alert(response.data.message + "Correctly registered");
    }
    catch (e){
        alert(e.response.data.message + " smh went wrong");
    }

}