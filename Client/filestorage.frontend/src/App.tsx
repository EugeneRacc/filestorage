import { FC, ReactElement } from 'react';
import { BrowserRouter as Router, Route } from 'react-router-dom';
import './App.css';
import userManager, { loadUser, signinRedirect } from './auth/user-service';
import AuthProvider from './auth/auth-provider';
import SignInOidc from './auth/SignInOidc';
import SignOutOidc from './auth/SignOutOidc';
//import UserList from './files/UsersList';

const App: FC<{}> = (): ReactElement => {
  loadUser();
  // @ts-ignore
    return (
      <div className="App">
        <header className="App-header">
          <button onClick={() => signinRedirect()}>Login</button>
          <AuthProvider userManager={userManager}>
            <Router>

            </Router>
          </AuthProvider>
        </header>
      </div>
  );
};

export default App;