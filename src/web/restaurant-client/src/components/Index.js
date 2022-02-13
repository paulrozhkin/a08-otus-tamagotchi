import React, {useEffect, useState} from 'react';
import {Row, Col, Container} from 'react-bootstrap';
import {Link} from 'react-router-dom';
import OwlCarousel from 'react-owl-carousel3';
import TopSearch from './home/TopSearch';
import ProductBox from './home/ProductBox';
import CardItem from './common/CardItem';
import SectionHeading from './common/SectionHeading';
import FontAwesome from './common/FontAwesome';
import {connect} from "react-redux";
import {restaurantsService} from "../services/restaurants-service";
import {imageService} from "../services/image-service";

function Index() {

	const [restaurants, setRestaurants] = useState([])

    useEffect(() => {
        restaurantsService.getRestaurants()
			.then(restaurantsDto => {
				setRestaurants(restaurantsDto)
			})
    }, [])

    let items = null
    if (restaurants.length > 0) {
        items = restaurants.map((restaurant) => {
            return <div key={restaurant.id} className="item">
                <CardItem
                    title='Tamagotchi на Рубинштейна'
                    subTitle={restaurant.phoneNumber}
                    imageAlt='Restaurant'
                    image={imageService.getUrlById(restaurant.photos[0])}
                    imageClass='img-fluid item-img'
                    linkUrl='detail'
                    time='10–15 min'
                    favIcoIconColor='text-dark'
                    rating='5.0 (300+)'
                />
            </div>
        })
    }

    return (
        <>
            <TopSearch/>
            <section className="section pt-5 pb-5 bg-white homepage-add-section">
                <Container>
                    <Row>
                        <Col md={3} xs={6}>
                            <ProductBox
                                image='img/pro1.jpg'
                                imageClass='img-fluid rounded'
                                imageAlt='product'
                                linkUrl='#'
                            />
                        </Col>
                        <Col md={3} xs={6}>
                            <ProductBox
                                image='img/pro2.jpg'
                                imageClass='img-fluid rounded'
                                imageAlt='product'
                                linkUrl='#'
                            />
                        </Col>
                        <Col md={3} xs={6}>
                            <ProductBox
                                image='img/pro3.jpg'
                                imageClass='img-fluid rounded'
                                imageAlt='product'
                                linkUrl='#'
                            />
                        </Col>
                        <Col md={3} xs={6}>
                            <ProductBox
                                image='img/pro4.jpg'
                                imageClass='img-fluid rounded'
                                imageAlt='product'
                                linkUrl='#'
                            />
                        </Col>
                    </Row>
                </Container>
            </section>

            <section className="section pt-5 pb-5 products-section">
                <Container>
                    <SectionHeading
                        heading='Рестораны'
                        subHeading='Наши рестораны с возможностью предварительного заказа'
                    />
                    <Row>
                        <Col md={12}>
                            <OwlCarousel nav loop {...options} className="owl-carousel-four owl-theme">
                                {items}

                                <div className="item">
                                    <CardItem
                                        title='Polo Lounge'
                                        subTitle='North Indian • American • Pure veg'
                                        imageAlt='Product'
                                        image='img/list/9.png'
                                        imageClass='img-fluid item-img'
                                        linkUrl='detail'
                                        offerText='65% off | Use Coupon OSAHAN50'
                                        time='20–25 min'
                                        price='$250 FOR TWO'
                                        showPromoted={true}
                                        promotedVariant='dark'
                                        favIcoIconColor='text-danger'
                                        rating='3.1 (300+)'
                                    />
                                </div>
                            </OwlCarousel>
                        </Col>
                    </Row>
                </Container>
            </section>
        </>
    );
}


const options = {
    responsive: {
        0: {
            items: 1,
        },
        600: {
            items: 2,
        },
        1000: {
            items: 4,
        },
        1200: {
            items: 4,
        },
    },

    lazyLoad: true,
    pagination: false.toString(),
    loop: true,
    dots: false,
    autoPlay: 2000,
    nav: true,
    navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"]
}


function mapStateToProps(state) {
    return {}
}

export default connect(mapStateToProps)(Index);
