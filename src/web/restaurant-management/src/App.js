import React, {Component} from 'react'
import {HashRouter, Redirect, Route, Switch} from 'react-router-dom'
import './scss/style.scss'
import RestaurantCreate from "./views/components/management/restaurant-create/RestaurantCreate";
import {connect} from "react-redux";

const loading = (
  <div className="pt-3 text-center">
    <div className="sk-spinner sk-spinner-pulse"/>
  </div>
)

// Containers
const DefaultLayout = React.lazy(() => import('./layout/DefaultLayout'))

// Pages
const Login = React.lazy(() => import('./views/pages/login/Login'))
const Register = React.lazy(() => import('./views/pages/register/Register'))
const Page404 = React.lazy(() => import('./views/pages/page404/Page404'))
const Page500 = React.lazy(() => import('./views/pages/page500/Page500'))

function App(props) {
    const isLogin = props.jwt.token != null;
    console.log(props.jwt)

    return (
      <HashRouter>
        <React.Suspense fallback={loading}>
          <Switch>
            <Route exact path="/login" name="Login Page" render={(props) =>  !isLogin ? <Login {...props} /> : <Redirect to="/" />}/>
            <Route exact path="/404" name="Page 404" render={(props) => <Page404 {...props} />}/>
            <Route exact path="/500" name="Page 500" render={(props) => <Page500 {...props} />}/>
            <Route path="/" name="Home" render={(props) => isLogin ? <DefaultLayout {...props} /> : <Redirect to="/login" />}/>
          </Switch>
        </React.Suspense>
      </HashRouter>
    )
}

function mapStateToProps(state) {
  return {
    jwt: state.jwt
  }
}

export default connect(mapStateToProps)(App)
