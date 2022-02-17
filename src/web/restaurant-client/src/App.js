import React from 'react';
import {Switch, Route} from 'react-router-dom';
import Header from './components/common/Header';
import Footer from './components/common/Footer';
import Index from './components/Index';
import MyAccount from './components/MyAccount';
import NotFound from './components/NotFound';
import Thanks from './components/Thanks';
import Login from './components/Login';
import Register from './components/Register';
import Detail from './components/Detail';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'font-awesome/css/font-awesome.min.css';
import 'react-select2-wrapper/css/select2.css';
import './App.css';
import PrivateRoute from "./components/PrivateRoute";
import PublicRoute from "./components/PublicRoute";

class App extends React.Component  {
  render() {
    return (
      <>
          {
            (this.props.location.pathname!=='/login' && this.props.location.pathname!=='/register') ? <Header/>:''
          }
          <Switch>
            <PrivateRoute path="/" exact component={Index} />
            <PrivateRoute path="/myaccount" component={MyAccount} />
            <PrivateRoute path="/404" exact component={NotFound} />
            <PublicRoute path="/login" exact component={Login} />
            <PublicRoute path="/register" exact component={Register} />
            <PrivateRoute path="/thanks" exact component={Thanks} />
            <PrivateRoute path="/detail/:id" exact component={Detail} />
            <Route exact component={NotFound} />
          </Switch>
          {
            (this.props.location.pathname!=='/login' && this.props.location.pathname!=='/register') ? <Footer/>:''
          }
      </>
    );
  }
}

export default App;
