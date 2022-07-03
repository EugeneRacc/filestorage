import {hideLoader, showLoader} from "../reducers/appReducer";
import axios from "axios";
import {setUsers} from "../reducers/adminReducer";


export function getUsers(sort){
    return async dispatch => {
        try{
            dispatch(showLoader())
            let url = `https://localhost:44368/api/1.0/admin/users`;
            if(sort){
                url = `https://localhost:44368/api/1.0/admin/users?sort=${sort}`
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