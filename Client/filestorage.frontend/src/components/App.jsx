import Navbar from "./navbar/Navbar";
import './app.css'
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Registration from "./registration/Registration";
import Login from "./login/Login";


function App() {
  return (
        <BrowserRouter>
            <div className="App">
                <Navbar/>
                <div className="wrap">
                    <Routes>
                        <Route path="/registration" element={<Registration />} />
                        <Route path="/login" element={<Login />} />
                    </Routes>
                </div>
            </div>
        </BrowserRouter>
  );
}

export default App;
