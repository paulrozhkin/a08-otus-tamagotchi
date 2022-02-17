import React from 'react';
import {NavLink,Link} from 'react-router-dom';
import {Navbar,Nav,Container,NavDropdown,Image} from 'react-bootstrap';
import DropDownTitle from '../common/DropDownTitle';
import CartDropdownHeader from '../cart/CartDropdownHeader';
import CartDropdownItem from '../cart/CartDropdownItem';
import Icofont from 'react-icofont';
import {connect} from "react-redux";
import {logout as logoutAction} from "../../actions/logout";


class Header extends React.Component {
	constructor(props) {
	    super(props);
	    this.state = {
	      isNavExpanded: false
	    };

		this.handleLogout = this.handleLogout.bind(this);
	}
    setIsNavExpanded = (isNavExpanded) => {
      this.setState({ isNavExpanded: isNavExpanded });
    }
    closeMenu = () => {
      this.setState({ isNavExpanded: false });
    }

    handleClick = (e) => {
      if (this.node.contains(e.target)) {
        // if clicked inside menu do something
      } else {
        // If clicked outside menu, close the navbar.
        this.setState({ isNavExpanded: false });
      }
    }

	componentDidMount() {
	    document.addEventListener('click', this.handleClick, false);
	}

	componentWillUnmount() {
	    document.removeEventListener('click', this.handleClick, false);
	}

	handleLogout(event) {
		event.preventDefault()
		this.props.logout();
	}

	render() {
    	return (
    		<div ref={node => this.node = node}>
			<Navbar onToggle={this.setIsNavExpanded}
           expanded={this.state.isNavExpanded} color="light" expand='lg' className="navbar-light osahan-nav shadow-sm">
			   <Container>
			      <Navbar.Brand to="/"><Image src="/img/logo.png" alt='' /></Navbar.Brand>
			      <Navbar.Toggle/>
			      <Navbar.Collapse id="navbarNavDropdown">
			         <Nav activeKey={0} className="ml-auto" onSelect={this.closeMenu}>
						<Nav.Link eventKey={0} as={NavLink} activeclassname="active" exact to="/">
			               На главную <span className="sr-only">(current)</span>
			            </Nav.Link>
			            <NavDropdown alignRight
			            	title={
			            		<DropDownTitle
			            			className='d-inline-block'
			            			image="img/user/4.png"
			            			imageAlt='user'
			            			imageClass="nav-osahan-pic rounded-pill"
			            			title='Мой аккаунт'
			            		/>
			            	}
			            >
							<NavDropdown.Item eventKey={4.1} as={NavLink} activeclassname="active" to="/myaccount/orders"><Icofont icon='food-cart'/> Заказы</NavDropdown.Item>
							<NavDropdown.Item eventKey={4.2} activeclassname="active" onClick={this.handleLogout}><Icofont icon='icofont-logout'/> Выйти</NavDropdown.Item>
			            </NavDropdown>
			         </Nav>
			      </Navbar.Collapse>
			   </Container>
			</Navbar>
			</div>
		);
	}
}


function mapStateToProps(state) {
	return {
	}
}

function mapDispatchToProps(dispatch) {
	return {
		logout: () => dispatch(logoutAction())
	}
}

export default connect(mapStateToProps, mapDispatchToProps)(Header);
