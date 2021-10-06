import React from "react";
import 'bootstrap/dist/css/bootstrap.min.css';
import NavBar from './components/NavBar';
import {BrowserRouter as Router} from "react-router-dom";
import {Container} from "react-bootstrap";

function App() {
    return (
        <Router>
            <NavBar/>
        </Router>
    );
}

export default App;
