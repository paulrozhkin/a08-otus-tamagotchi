import React from 'react';
import {Route, Redirect} from 'react-router-dom';
import {connect} from "react-redux";

const PublicRoute = ({component: Component, ...rest}) => {
    return (
        <Route {...rest} render={props => (
            !rest.isLogin ?
                <Component {...props} />
                : <Redirect to="/"/>
        )}/>
    );
};

function mapStateToProps(state) {
    const {account} = state
    return {isLogin: !!account}
}

export default connect(mapStateToProps)(PublicRoute);
