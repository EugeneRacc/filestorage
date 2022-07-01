import Navbar from "./navbar/Navbar";
import './app.css'
import {BrowserRouter, Routes, Route, Navigate} from "react-router-dom";
import Registration from "./registration/Registration";
import Login from "./login/Login";
import {useDispatch, useSelector} from "react-redux";
import {auth} from "../actions/user";
import {useEffect} from "react";
import Disk from "./disk/Disk";
import Download from "./fileList/file/Download";



function App() {
    const isAuth = useSelector(state => state.user.isAuth);
    const dispatch = useDispatch();

    useEffect(() => {
        dispatch(auth())
    }, [])

  return (
        <BrowserRouter>
            <div className="App">
                <Navbar/>

                <div className="wrap">
                    {!isAuth ?
                        <Routes>
                            <Route path="/registration" element={<Registration />} />
                            <Route path="/login" element={<Login />} />
                            <Route path="/share/*" element={<Download />}
                                   />
                            <Route path="*" element={<Navigate replace to="/login"/>} />
                        </Routes>
                        :
                        <Routes>
                            <Route exact path="/" element={<Disk />} />
                            <Route path="*" element={<Navigate replace to="/"/>} />
                        </Routes>
                    }
                </div>
            </div>
        </BrowserRouter>
  );
}

export default App;
