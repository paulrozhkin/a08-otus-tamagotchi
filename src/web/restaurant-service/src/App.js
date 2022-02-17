import React from "react";
import 'bootstrap/dist/css/bootstrap.min.css';
import NavBar from './components/NavBar';
import {BrowserRouter as Router} from "react-router-dom";
import store from "./store";
import {Provider} from "react-redux";
import "./axios-interceptors";

function App() {
    return (
        <Provider store={store}>
            <Router>
                <NavBar/>
            </Router>
        </Provider>
    );
}

export default App;
