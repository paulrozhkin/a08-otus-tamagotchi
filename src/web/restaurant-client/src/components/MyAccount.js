import React from 'react';
import {Switch, Route} from 'react-router-dom';
import {NavLink, Link} from 'react-router-dom';
import {Row, Col, Container, Image} from 'react-bootstrap';
import Orders from './myaccount/Orders';
import Payments from './myaccount/Payments';
import Addresses from './myaccount/Addresses';
import EditProfileModal from './modals/EditProfileModal';
import store from "../store";

class MyAccount extends React.Component {
    constructor(props, context) {
        super(props, context);
        const account = store.getState().account

        this.state = {
            showEditProfile: false, email: account.username
        };
    }

    hideEditProfile = () => this.setState({showEditProfile: false});

    render() {
        return (<>
                <EditProfileModal show={this.state.showEditProfile} onHide={this.hideEditProfile}/>
                <section className="section pt-4 pb-4 osahan-account-page">
                    <Container>
                        <Row>
                            <Col md={3}>
                                <div className="osahan-account-page-left shadow-sm bg-white h-100">
                                    <div className="border-bottom p-4">
                                        <div className="osahan-user text-center">
                                            <div className="osahan-user-media">
                                                <Image className="mb-3 rounded-pill shadow-sm mt-1"
                                                       src="/img/user/4.png" alt="gurdeep singh osahan"/>
                                                <div className="osahan-user-media-body">
                                                    <h6 className="mb-2">Олег</h6>
                                                    <p className="mb-1">+7-900-031-83-18</p>
                                                    <p>{this.state.email}</p>
                                                    <p className="mb-0 text-black font-weight-bold"><Link to='#'
                                                                                                          onClick={() => this.setState({showEditProfile: true})}
                                                                                                          className="text-primary mr-3"><i
                                                        className="icofont-ui-edit"></i> Изменить</Link></p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <ul className="nav flex-column border-0 pt-4 pl-4 pb-4">
                                        <li className="nav-item">
                                            <NavLink className="nav-link" activeClassName="active" exact
                                                     to="/myaccount/orders"><i className="icofont-food-cart"></i> Заказы</NavLink>
                                        </li>
                                        <li className="nav-item">
                                            <NavLink className="nav-link" activeClassName="active" exact
                                                     to="/myaccount/payments"><i
                                                className="icofont-credit-card"></i> Способы оплаты</NavLink>
                                        </li>
                                    </ul>
                                </div>
                            </Col>
                            <Col md={9}>
                                <Switch>
                                    <Route path="/myaccount/orders" exact component={Orders}/>
                                    <Route path="/myaccount/payments" exact component={Payments}/>
                                </Switch>
                            </Col>
                        </Row>
                    </Container>
                </section>
            </>);
    }
}


export default MyAccount;
