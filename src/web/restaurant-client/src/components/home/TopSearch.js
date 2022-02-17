import React from 'react';
import {Row, Col, Container} from 'react-bootstrap';
import CategoriesCarousel from '../common/CategoriesCarousel';

function TopSearch() {
    return (
        <section className="homepage-search-block position-relative search-image d-flex align-items-center">
            <div className="banner-overlay"/>
            <Container className="position-relative">
                <Row>
                    <Col className="text-center homepage-search-title">
                        <h1 className="mb-2 font-weight-normal text-shadow text-white font-weight-bold">Сеть ресторанов
                            Tamagotchi</h1>
                        <h5 className="mb-5 text-shadow text-white-50 font-weight-normal">Вкусная еда и онлайн
                            бронирование</h5>
                    </Col>
                    <CategoriesCarousel/>
                </Row>
            </Container>
        </section>
    );
}

export default TopSearch;
