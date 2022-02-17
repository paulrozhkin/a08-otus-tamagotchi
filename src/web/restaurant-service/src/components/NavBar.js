import React from 'react';
import {Link, Route, Switch} from 'react-router-dom';
import {Container, Nav, Navbar} from 'react-bootstrap';
import Home from '../pages/Home';
import Kitchen from '../pages/Kitchen';
import Hall from '../pages/Hall';
import SignIn from "../pages/SignIn";
import PrivateRoute from "./PrivateRoute";
import PublicRoute from "./PublicRoute";

const NavBar = () => {
    return (
        <>
            <Navbar bg="light" expand="lg">
                <Container>
                    <Navbar.Brand as={Link} to="/">Restaurant Service</Navbar.Brand>
                    <Navbar.Toggle aria-controls="basic-navbar-nav"/>
                    <Navbar.Collapse id="responsive-navbar-nav">
                        <Nav className="me-auto">
                            <Nav.Link as={Link} to="/kitchen">Кухня</Nav.Link>
                        </Nav>
                        <Nav>
                            <Nav.Link as={Link} to="/signin">Войти</Nav.Link>
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
            <Switch>
                <PrivateRoute exact path='/' component={Home}/>
                <PrivateRoute exact path='/kitchen' component={Kitchen}/>
                <PublicRoute exact path='/signin' component={SignIn}/>
            </Switch>
        </>
    );
}

export default NavBar;
