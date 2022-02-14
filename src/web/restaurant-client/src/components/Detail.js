import React, {useEffect, useState} from 'react';
import {Link, useParams} from 'react-router-dom';
import {Row, Col, Container, Form, Button, Tab, Nav, Image, Badge} from 'react-bootstrap';
import GalleryCarousel from './common/GalleryCarousel';
import CheckoutItem from './common/CheckoutItem';
import StarRating from './common/StarRating';
import RatingBar from './common/RatingBar';
import Review from './common/Review';
import Icofont from 'react-icofont';
import {restaurantsService} from "../services/restaurants-service";
import {imageService} from "../services/image-service";
import RestaurantInfo from "./ResturantInfo";
import Order from "./Order";
import {connect} from "react-redux";
import {selectRestaurant} from "../actions/select-restaurant";

function Detail(props) {

    const defaultState = {
        users: [
            {
                name: 'Osahan Singh',
                image: '/img/user/5.png',
                url: '#'
            },
            {
                name: 'Gurdeep Osahan',
                image: '/img/user/2.png',
                url: '#'
            },
            {
                name: 'Askbootstrap',
                image: '/img/user/3.png',
                url: '#'
            },
            {
                name: 'Osahan Singh',
                image: '/img/user/4.png',
                url: '#'
            }
        ]
    };

    const {id} = useParams()

    const [state, setState] = useState(defaultState);
    const [restaurant, setRestaurant] = useState()

    const getQty = ({id, quantity}) => {
        //console.log(id);
        //console.log(quantity);
    }
    const getStarValue = ({value}) => {
        console.log(value);
        //console.log(quantity);
    }

    useEffect(() => {
            restaurantsService.getRestaurant(id)
                .then(restaurant => {
                    props.selectRestaurant(restaurant)
                    setRestaurant(restaurant)
                })
        },
        [id])

    return (
        <>
            {restaurant && <div>
                <section className="restaurant-detailed-banner">
                    <div className="text-center">
                        <Image fluid className="cover" src="/img/mall-dedicated-banner.png"/>
                    </div>
                    <div className="restaurant-detailed-header">
                        <Container>
                            <Row className="d-flex align-items-end">
                                <Col md={8}>
                                    <div className="restaurant-detailed-header-left">
                                        <Image fluid className="mr-3 float-left" alt="osahan"
                                               src={imageService.getUrlById(restaurant.photos[0])}/>
                                        <h2 className="text-white">{restaurant.title}</h2>
                                        <p className="text-white mb-1"><Icofont
                                            icon="location-pin"/> {restaurant.address} <Badge
                                            variant="success">OPEN</Badge>
                                        </p>
                                        <p className="text-white mb-0"><Icofont
                                            icon="food-cart"/> {restaurant.phoneNumber}
                                        </p>
                                    </div>
                                </Col>
                                <Col md={4}>
                                    <div className="restaurant-detailed-header-right text-right">
                                        <Button variant='success' type="button"><Icofont icon="clock-time"/> 10–15 мин
                                        </Button>
                                        <h6 className="text-white mb-0 restaurant-detailed-ratings">
	                           <span className="generator-bg rounded text-white">
	                              <Icofont icon="star"/> 5.0
	                           </span> 23 звезды
                                            <Icofont icon="speech-comments" className="ml-3"/> 91 отзыв
                                        </h6>
                                    </div>
                                </Col>
                            </Row>
                        </Container>
                    </div>
                </section>

                <Tab.Container defaultActiveKey="first">
                    <section className="offer-dedicated-nav bg-white border-top-0 shadow-sm">
                        <Container>
                            <Row>
                                <Col md={12}>
		                  <span className="restaurant-detailed-action-btn float-right">
		                     <Button variant='light' size='sm' className="border-light-btn mr-1" type="button"><Icofont
                                 icon="heart" className='text-danger'/> Mark as Favourite</Button>
		                     <Button variant='light' size='sm' className="border-light-btn mr-1" type="button"><Icofont
                                 icon="cauli-flower" className='text-success'/>  Pure Veg</Button>
		                     <Button variant='outline-danger' size='sm' type="button"><Icofont icon="sale-discount"/>  OFFERS</Button>
		                  </span>
                                    <Nav id="pills-tab">
                                        <Nav.Item>
                                            <Nav.Link eventKey="first">Информация о ресторане</Nav.Link>
                                        </Nav.Item>
                                        <Nav.Item>
                                            <Nav.Link eventKey="second">Галерея</Nav.Link>
                                        </Nav.Item>
                                        <Nav.Item>
                                            <Nav.Link eventKey="third">Заказ</Nav.Link>
                                        </Nav.Item>
                                        <Nav.Item>
                                            <Nav.Link eventKey="fourth">Бронь</Nav.Link>
                                        </Nav.Item>
                                        <Nav.Item>
                                            <Nav.Link eventKey="fifth">Отзывы</Nav.Link>
                                        </Nav.Item>
                                    </Nav>
                                </Col>
                            </Row>
                        </Container>
                    </section>

                    <section className="offer-dedicated-body pt-2 pb-2 mt-4 mb-4">
                        <Container>
                            <Row>
                                <Col md={8}>
                                    <div className="offer-dedicated-body-left">
                                        <Tab.Content className='h-100'>
                                            <Tab.Pane eventKey="first">
                                                <RestaurantInfo restaurant={restaurant}/>
                                            </Tab.Pane>
                                            <Tab.Pane eventKey="second">
                                                <div className='position-relative'>
                                                    <GalleryCarousel images={restaurant.photos}/>
                                                </div>
                                            </Tab.Pane>
                                            <Tab.Pane eventKey="third">
                                                <Order restaurantId={restaurant.id}/>
                                            </Tab.Pane>
                                            <Tab.Pane eventKey="fourth">
                                                <div id="book-a-table"
                                                     className="bg-white rounded shadow-sm p-4 mb-5 rating-review-select-page">
                                                    <h5 className="mb-4">Book A Table</h5>
                                                    <Form>
                                                        <Row>
                                                            <Col sm={6}>
                                                                <Form.Group>
                                                                    <Form.Label>Full Name</Form.Label>
                                                                    <Form.Control type="text"
                                                                                  placeholder="Enter Full Name"/>
                                                                </Form.Group>
                                                            </Col>
                                                            <Col sm={6}>
                                                                <Form.Group>
                                                                    <Form.Label>Email Address</Form.Label>
                                                                    <Form.Control type="text"
                                                                                  placeholder="Enter Email address"/>
                                                                </Form.Group>
                                                            </Col>
                                                        </Row>
                                                        <Row>
                                                            <Col sm={6}>
                                                                <Form.Group>
                                                                    <Form.Label>Mobile number</Form.Label>
                                                                    <Form.Control type="text"
                                                                                  placeholder="Enter Mobile number"/>
                                                                </Form.Group>
                                                            </Col>
                                                            <Col sm={6}>
                                                                <Form.Group>
                                                                    <Form.Label>Date And Time</Form.Label>
                                                                    <Form.Control type="text"
                                                                                  placeholder="Enter Date And Time"/>
                                                                </Form.Group>
                                                            </Col>
                                                        </Row>
                                                        <Form.Group className="text-right">
                                                            <Button variant="primary" type="button"> Submit </Button>
                                                        </Form.Group>
                                                    </Form>
                                                </div>
                                            </Tab.Pane>
                                            <Tab.Pane eventKey="fifth">
                                                <div id="ratings-and-reviews"
                                                     className="bg-white rounded shadow-sm p-4 mb-4 clearfix restaurant-detailed-star-rating">
                                                    <div className="star-rating float-right">
                                                        <StarRating fontSize={26} star={5} getValue={getStarValue}/>
                                                    </div>
                                                    <h5 className="mb-0 pt-1">Rate this Place</h5>
                                                </div>
                                                <div
                                                    className="bg-white rounded shadow-sm p-4 mb-4 clearfix graph-star-rating">
                                                    <h5 className="mb-0 mb-4">Ratings and Reviews</h5>
                                                    <div className="graph-star-rating-header">
                                                        <div className="star-rating">
                                                            <StarRating fontSize={18} disabled={true} star={5}
                                                                        getValue={getStarValue}/>
                                                            <b className="text-black ml-2">334</b>
                                                        </div>
                                                        <p className="text-black mb-4 mt-2">Rated 3.5 out of 5</p>
                                                    </div>
                                                    <div className="graph-star-rating-body">
                                                        <RatingBar leftText="5 Star" barValue={56}/>
                                                        <RatingBar leftText="4 Star" barValue={23}/>
                                                        <RatingBar leftText="3 Star" barValue={11}/>
                                                        <RatingBar leftText="2 Star" barValue={6}/>
                                                        <RatingBar leftText="1 Star" barValue={4}/>
                                                    </div>
                                                    <div className="graph-star-rating-footer text-center mt-3 mb-3">
                                                        <Button type="button" variant="outline-primary" size="sm">Rate
                                                            and
                                                            Review</Button>
                                                    </div>
                                                </div>
                                                <div
                                                    className="bg-white rounded shadow-sm p-4 mb-4 restaurant-detailed-ratings-and-reviews">
                                                    <Link to="#" className="btn btn-outline-primary btn-sm float-right">Top
                                                        Rated</Link>
                                                    <h5 className="mb-1">All Ratings and Reviews</h5>
                                                    <Review
                                                        image="/img/user/1.png"
                                                        ImageAlt=""
                                                        ratingStars={5}
                                                        Name='Singh Osahan'
                                                        profileLink="#"
                                                        reviewDate="Tue, 20 Mar 2020"
                                                        reviewText="Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classNameical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classNameical literature, discovered the undoubtable source. Lorem Ipsum comes from sections"
                                                        likes="856M"
                                                        dislikes="158K"
                                                        otherUsers={state.users}
                                                    />
                                                    <hr/>
                                                    <Review
                                                        image="/img/user/6.png"
                                                        ImageAlt=""
                                                        ratingStars={5}
                                                        Name='Gurdeep Osahan'
                                                        profileLink="#"
                                                        reviewDate="Tue, 20 Mar 2020"
                                                        reviewText="It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English."
                                                        likes="88K"
                                                        dislikes="1K"
                                                        otherUsers={state.users}
                                                    />
                                                    <hr/>
                                                    <Link className="text-center w-100 d-block mt-4 font-weight-bold"
                                                          to="#">See All Reviews</Link>
                                                </div>
                                                <div
                                                    className="bg-white rounded shadow-sm p-4 mb-5 rating-review-select-page">
                                                    <h5 className="mb-4">Leave Comment</h5>
                                                    <p className="mb-2">Rate the Place</p>
                                                    <div className="mb-4">
                                                        <div className="star-rating">
                                                            <StarRating fontSize={26} star={5} getValue={getStarValue}/>
                                                        </div>
                                                    </div>
                                                    <Form>
                                                        <Form.Group>
                                                            <Form.Label>Your Comment</Form.Label>
                                                            <Form.Control as="textarea"/>
                                                        </Form.Group>
                                                        <Form.Group>
                                                            <Button variant="primary" size="sm" type="button"> Submit
                                                                Comment </Button>
                                                        </Form.Group>
                                                    </Form>
                                                </div>
                                            </Tab.Pane>
                                        </Tab.Content>
                                    </div>
                                </Col>
                                <Col md={4}>
                                    <div className="generator-bg rounded shadow-sm mb-4 p-4 osahan-cart-item">
                                        <h5 className="mb-1 text-white">Your Order
                                        </h5>
                                        <p className="mb-4 text-white">6 Items</p>
                                        <div className="bg-white rounded shadow-sm mb-2">
                                            <CheckoutItem
                                                itemName="Chicken Tikka Sub"
                                                price={314}
                                                priceUnit="$"
                                                id={1}
                                                qty={2}
                                                show={true}
                                                minValue={0}
                                                maxValue={7}
                                                getValue={getQty}
                                            />
                                            <CheckoutItem
                                                itemName="Cheese corn Roll"
                                                price={260}
                                                priceUnit="$"
                                                id={2}
                                                qty={1}
                                                show={true}
                                                minValue={0}
                                                maxValue={7}
                                                getValue={getQty}
                                            />
                                            <CheckoutItem
                                                itemName="Mixed Veg"
                                                price={122}
                                                priceUnit="$"
                                                id={3}
                                                qty={1}
                                                show={true}
                                                minValue={0}
                                                maxValue={7}
                                                getValue={getQty}
                                            />
                                            <CheckoutItem
                                                itemName="Black Dal Makhani"
                                                price={652}
                                                priceUnit="$"
                                                id={1}
                                                qty={1}
                                                show={true}
                                                minValue={0}
                                                maxValue={7}
                                                getValue={getQty}
                                            />
                                            <CheckoutItem
                                                itemName="Mixed Veg"
                                                price={122}
                                                priceUnit="$"
                                                id={4}
                                                qty={1}
                                                show={true}
                                                minValue={0}
                                                maxValue={7}
                                                getValue={getQty}
                                            />

                                        </div>
                                        <div className="mb-2 bg-white rounded p-2 clearfix">
                                            <Image fluid className="float-left" src="/img/wallet-icon.png"/>
                                            <h6 className="font-weight-bold text-right mb-2">Subtotal : <span
                                                className="text-danger">$456.4</span></h6>
                                            <p className="seven-color mb-1 text-right">Extra charges may apply</p>
                                            <p className="text-black mb-0 text-right">You have saved $955 on the
                                                bill</p>
                                        </div>
                                        <Link to="/thanks" className="btn btn-success btn-block btn-lg">Checkout
                                            <Icofont icon="long-arrow-right"/></Link>
                                        <div className="pt-2"></div>
                                        <div className="alert alert-success" role="alert">
                                            You have saved <strong>$1,884</strong> on the bill
                                        </div>
                                        <div className="pt-2"></div>
                                    </div>
                                </Col>
                            </Row>
                        </Container>
                    </section>

                </Tab.Container>
            </div>}
        </>
    );
}

function mapStateToProps(state) {
    return {
    }
}

function mapDispatchToProps(dispatch) {
    return {
        selectRestaurant: (restaurant) => dispatch(selectRestaurant(restaurant))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Detail);
