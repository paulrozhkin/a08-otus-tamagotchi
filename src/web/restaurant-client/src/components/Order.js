import {Button, Col, Form, InputGroup, Row} from "react-bootstrap";
import Icofont from "react-icofont";
import QuickBite from "./common/QuickBite";
import React, {useEffect, useState} from "react";
import {menuService} from "../services/menu-service";
import {imageService} from "../services/image-service";
import {connect} from "react-redux";
import {updateMenu} from "../actions/update-menu";

function Order(props) {

    const {restaurantId} = props
    const [menu, setMenu] = useState([])

    useEffect(() => {
        menuService.getMenu(restaurantId)
            .then(menuDto => {
                const menuWithCount = menuDto.map(menu => {
                    const menuWithSameId = props.menu.filter(item => item.menuItem.id === menu.id)
                    let count = 0
                    if (menuWithSameId.length > 0) {
                        count = menuWithSameId.count
                    }
                    return {
                        ...menu,
                        count
                    }
                })

                setMenu(menuWithCount)
            })
    }, [restaurantId])

    const getQty = ({id, quantity}) => {
        props.updateMenu(menu.filter(x => x.id === id)[0], quantity)
    }

    return (
        <>
            <Form className="explore-outlets-search mb-4">
                <InputGroup>
                    <Form.Control type="text" placeholder="Название блюда..."/>
                    <InputGroup.Append>
                        <Button type="button" variant="link">
                            <Icofont icon="search"/>
                        </Button>
                    </InputGroup.Append>
                </InputGroup>
            </Form>

            <Row>
                <h5 className="mb-4 mt-3 col-md-12">Меню <small
                    className="h6 text-black-50">{menu.length}</small></h5>
                <Col md={12}>
                    {
                        menu.map(menuItem => (<div key={menuItem.id} className="bg-white rounded border shadow-sm mb-4">
                            <QuickBite
                                id={menuItem.id}
                                title={menuItem.dish.name}
                                price={menuItem.priceRubles}
                                priceUnit='₽'
                                image={imageService.getUrlById(menuItem.dish.photos[0])}
                                imageClass="img-responsive menu-image"
                                getValue={getQty}
                                qty={props.count}
                                description={menuItem.dish.description}
                            />
                        </div>))
                    }
                </Col>
            </Row>
        </>
    )
}

function mapStateToProps(state) {
    return {
        menu: state.order.menu
    }
}

function mapDispatchToProps(dispatch) {
    return {
        updateMenu: (menuItem, newCount) => dispatch(updateMenu(menuItem, newCount))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Order)
