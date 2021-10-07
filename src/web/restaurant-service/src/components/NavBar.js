import React from 'react';
import {Link, Route, Switch} from 'react-router-dom';
import {Container, Nav, Navbar} from 'react-bootstrap';
import Home from '../pages/Home';
import Kitchen from '../pages/Kitchen';
import Hall from '../pages/Hall';
import SignIn from "../pages/SignIn";

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
                            <Nav.Link as={Link} to="/hall">Зал</Nav.Link>
                        </Nav>
                        <Nav>
                            <Nav.Link as={Link} to="/signin">Войти</Nav.Link>
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
            <Switch>
                <Route exact path='/' component={Home} />
                <Route exact path='/kitchen' component={Kitchen} />
                <Route exact path='/hall' component={Hall} />
                <Route exact path='/signin' component={SignIn} />
            </Switch>
        </>
    );
}

export default NavBar;