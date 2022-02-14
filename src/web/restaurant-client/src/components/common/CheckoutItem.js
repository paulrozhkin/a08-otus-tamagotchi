import React, {useEffect, useState} from 'react';
import PropTypes from 'prop-types';
import {Button} from 'react-bootstrap';
import Icofont from 'react-icofont';

function CheckoutItem(props) {
    const [quantity, setQuantity] = useState(props.qty || 1)
    const [max] = useState(props.maxValue || 5)
    const [min] = useState(props.minValue || 0)

    useEffect(() => {
        setQuantity(props.qty)
    }, [props.qty])

    const IncrementItem = () => {
        if (quantity >= max) {

        } else {
            props.getValue({id: props.id, quantity: (quantity + 1)});
        }
    }
    const DecreaseItem = () => {
        if (quantity <= min) {

        } else {
            props.getValue({id: props.id, quantity: (quantity - 1)});
        }
    }

    return (
        <div className="gold-members p-2 border-bottom">
            <span className="count-number float-right">
               <Button variant="outline-secondary" onClick={DecreaseItem} className="btn-sm left dec"> <Icofont
                   icon="minus"/> </Button>
               <input className="count-number-input" type="text" value={quantity} readOnly/>
               <Button variant="outline-secondary" onClick={IncrementItem} className="btn-sm right inc"> <Icofont
                   icon="icofont-plus"/> </Button>
           </span>
            <p className="text-gray mb-0 float-right mr-2">{props.priceUnit}{props.price * quantity}</p>
            <div className="media">
                <div className="mr-2"><Icofont icon="ui-press" className="text-danger food-item"/></div>
                <div className="media-body">
                    <p className="mt-1 mb-0 text-black">{props.itemName}</p>
                </div>
            </div>
        </div>
    );
}

CheckoutItem.propTypes = {
    itemName: PropTypes.string.isRequired,
    price: PropTypes.number.isRequired,
    priceUnit: PropTypes.string.isRequired,
    id: PropTypes.string.isRequired,
    qty: PropTypes.number.isRequired,
    minValue: PropTypes.number,
    maxValue: PropTypes.number,
    getValue: PropTypes.func.isRequired
};
CheckoutItem.defaultProps = {
    priceUnit: '$'
}


export default CheckoutItem;
