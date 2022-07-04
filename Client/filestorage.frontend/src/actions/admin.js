import {hideLoader, showLoader} from "../reducers/appReducer";
import axios from "axios";
import {setCurrentUser, setUserFiles, setUsers} from "../reducers/adminReducer";

export function getUsers(sort, name){
    return async dispatch => {
        try{
            dispatch(showLoader())
            let url = `https://localhost:44368/api/1.0/admin/users`;
            if(sort){
                url = `https://localhost:44368/api/1.0/admin/users?sort=${sort}`
            }
            if(name){
                url = `https://localhost:44368/api/1.0/admin/users?name=${name}`
            }
            if(name && sort){
                url = `https://localhost:44368/api/1.0/admin/users?name=${name}`
            }
            const response = await axios.get(url, {
                headers: {Authorization: `Bearer ${localStorage.getItem('token')}`}
            });
            console.log(response.data)
            dispatch(setUsers(response.data))
        }
        catch (e){
            console.log(e)
        }
        finally {
            dispatch(hideLoader())
        }
    }
}

export function getUserById(currentUser){
    return async dispatch => {
        try{
            const response = await axios.get(`https://localhost:44368/api/1.0/admin/users/${currentUser}`,
                {headers: {
                        Authorization: `Bearer ${localStorage.getItem('token')}`
                    }});
            dispatch(setCurrentUser(response.data))
        }
        catch (e){
            console.log("Something went wrong")
        }
        finally {
            dispatch(hideLoader())
        }
    }
}

export function getUserFiles(sort, name, id){
    return async dispatch => {
        try{
            dispatch(showLoader())
            let url = `https://localhost:44368/api/1.0/admin/users/${id}/files`;
            if(sort){
                url = `https://localhost:44368/api/1.0/admin/users/${id}/files?sort=${sort}`
            }
            if(name){
                url = `https://localhost:44368/api/1.0/admin/users/${id}/files?name=${name}`
            }
            if(sort && name){
                url = `https://localhost:44368/api/1.0/admin/users/${id}/files?name=${name}&sort=${sort}`
            }
            const response = await axios.get(url, {
                headers: {Authorization: `Bearer ${localStorage.getItem('token')}`}
            });
            console.log(response.data)
            dispatch(setUserFiles(response.data))
        }
        catch (e){

        }
        finally {
            dispatch(hideLoader())
        }
    }
}
export async function updateUserInfo(userToUpdate,newEmail, newRole){
        try{
            const response = await axios.put(
                `https://localhost:44368/api/1.0/admin/users`, {
                    Id: userToUpdate.id,
                    Email: newEmail,
                    RoleName: newRole.toString()
                },{
                    headers: {Authorization: `Bearer ${localStorage.getItem('token')}`}
                });

            console.log(response.data)
        }
        catch (e){
            alert(e.response.data.message)
        }
}