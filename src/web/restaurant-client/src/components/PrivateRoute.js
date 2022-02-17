import React from 'react';
import {Route, Redirect} from 'react-router-dom';
import {connect} from "react-redux";

const PrivateRoute = ({component: Component, ...rest}) => {
    return (

        // Show the component only when the user is logged in
        // Otherwise, redirect the user to /signin page
        <Route {...rest} render={props => (
            rest.isLogin ?
                <Component {...props} />
                : <Redirect to="/login"/>
        )}/>
    );
};

function mapStateToProps(state) {
    const {account} = state
    return {isLogin: !!account}
}

export default connect(mapStateToProps)(PrivateRoute);
