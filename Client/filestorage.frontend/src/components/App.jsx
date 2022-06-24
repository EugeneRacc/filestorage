import Navbar from "./navbar/Navbar";
import './app.css'
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Registration from "./registration/Registration";
import Login from "./login/Login";
import {useDispatch, useSelector} from "react-redux";
import {auth} from "../actions/user";
import {useEffect} from "react";


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
                    {!isAuth &&
                        <Routes>
                            <Route path="/registration" element={<Registration />} />
                            <Route path="/login" element={<Login />} />
                        </Routes>
                    }
                </div>
            </div>
        </BrowserRouter>
  );
}

export default App;
