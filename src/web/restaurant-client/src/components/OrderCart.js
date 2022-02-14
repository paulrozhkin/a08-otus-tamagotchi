import CheckoutItem from "./common/CheckoutItem";
import {Button, Image} from "react-bootstrap";
import Icofont from "react-icofont";
import React, {useEffect, useState} from "react";
import {connect} from "react-redux";
import {updateMenu} from "../actions/update-menu";

function OrderCart(props) {
    const [amount, setAmount] = useState(0);
    const [count, setCount] = useState(0);


    const getQty = ({id, quantity}) => {
        props.updateMenu(props.menu.filter(x => x.menuItem.id === id)[0].menuItem, quantity)
    }

    useEffect(() => {
        let newAmount = 0
        let newCount = 0

        for (let i = 0; i < props.menu.length; i++) {
            newAmount += props.menu[i].count * props.menu[i].menuItem.priceRubles
            newCount += props.menu[i].count
        }

        setAmount(newAmount)
        setCount(newCount)
    }, [props.menu])

    return (
        <div className="generator-bg rounded shadow-sm mb-4 p-4 osahan-cart-item">
            <h5 className="mb-1 text-white">Ваш Заказ
            </h5>
            <p className="mb-4 text-white">В корзине {count} позиций</p>
            <div className="bg-white rounded shadow-sm mb-2">
                {
                    props.menu.map(x => {
                        return <CheckoutItem key={x.menuItem.id}
                                             itemName={x.menuItem.dish.name}
                                             price={x.menuItem.priceRubles}
                                             priceUnit="₽"
                                             id={x.menuItem.id}
                                             qty={x.count}
                                             getValue={getQty}
                        />
                    })
                }
            </div>
            <div className="mb-2 bg-white rounded p-2 clearfix">
                <Image fluid className="float-left" src="/img/wallet-icon.png"/>
                <h6 className="font-weight-bold text-right mb-2">К оплате: <span
                    className="text-danger">${amount}</span></h6>
            </div>
            <Button className="btn btn-success btn-block btn-lg" disabled={!props.canComplete}>Checkout
                <Icofont icon="long-arrow-right"/></Button>
            <div className="pt-2"/>
        </div>
    )
}

function mapStateToProps(state) {
    const order = state.order
    return {
        menu: order.menu,
        canComplete: order.isRestaurantSet && order.isMenuSet && order.isBookInfoSet
    }
}

function mapDispatchToProps(dispatch) {
    return {
        updateMenu: (menuItem, newCount) => dispatch(updateMenu(menuItem, newCount))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(OrderCart)
