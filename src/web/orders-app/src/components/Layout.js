import React, { Component } from 'react';
import { Container } from 'react-bootstrap';
import { Link, Route, Switch } from 'react-router-dom';
import { Navbar, Nav } from 'react-bootstrap';
import Restaurants from "./Restaurants"
import Sample from "./Sample"
import Map from "./Map"

export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <div>
        <Navbar bg="light" expand="lg">
          <Container>
            <Navbar.Brand href="#home">Tamagotchi orders</Navbar.Brand>
            <Navbar.Toggle aria-controls="basic-navbar-nav" />
            <Navbar.Collapse id="basic-navbar-nav">
              <Nav className="me-auto">
                <Nav.Link as={Link} to="/restaurants">Restaurants</Nav.Link>
                <Nav.Link as={Link} to="/map">Map</Nav.Link>
                <Nav.Link as={Link} to="/about">About</Nav.Link>
              </Nav>
              <Container>
                {this.props.children}
              </Container>
            </Navbar.Collapse>
          </Container>
        </Navbar>
        <Container>
          <Switch>
            <Route exact path='/' component={Restaurants}></Route>
            <Route exact path='/restaurants' component={Restaurants}></Route>
            <Route exact path='/map' component={Map}></Route> 
            <Route exact path='/about' component={Sample}></Route>
          </Switch>
        </Container>
      </div>
    );
  }
}

export default Layout;