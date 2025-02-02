import Navbar from "./navbar/Navbar";
import './app.css'
import {BrowserRouter, Routes, Route} from "react-router-dom";
import Registration from "./registration/Registration";
import Login from "./login/Login";
import {useDispatch, useSelector} from "react-redux";
import {auth} from "../actions/user";
import {useEffect} from "react";
import Disk from "./disk/Disk";
import Download from "./fileList/file/Download";
import AdminPanel from "./adminPanel/AdminPanel";
import UserDetails from "./adminPanel/userList/user/UserDetails/UserDetails";


function App() {
    const dispatch = useDispatch();
    useEffect(() => {
        dispatch(auth())

    }, [])
    const isAuth = useSelector(state => state.user.isAuth);
    let isAdmin = useSelector(state => state.user.currentUser.roleName) === "Admin"
return (
        <BrowserRouter>
            <div className="App">
                <Navbar/>

                <div className="wrap">
                    {isAuth === false?
                        <Routes>
                            <Route path="/registration" element={<Registration />} />
                            <Route path="/login" element={<Login />} />
                            <Route path="/share/*" element={<Download />}/>
                            <Route path="*" element={<Login />} />
                        </Routes>
                        :
                        <Routes>
                            <Route path="/" element={<Disk />} />
                            {isAdmin &&<Route path="/admin-panel" element={<AdminPanel />} />}
                            {isAdmin &&<Route path="/user-files" element={<Disk />} />}
                            {isAdmin &&<Route path="/user-details" element={<UserDetails />}/>}
                            <Route path="/share/*" element={<Download />}/>
                            <Route path="*" element={<Disk />} />
                        </Routes>
                    }
                </div>
            </div>
        </BrowserRouter>
  );
}

export default App;
