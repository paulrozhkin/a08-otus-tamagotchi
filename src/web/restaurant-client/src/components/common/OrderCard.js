import React from 'react';
import PropTypes from 'prop-types';
import {Link} from 'react-router-dom';
import {Badge, Image, Media} from 'react-bootstrap';
import Icofont from 'react-icofont';

class OrderCard extends React.Component {

    render() {
        return (
            <div className="bg-white card mb-4 order-list shadow-sm">
                <div className="gold-members p-4">
                    <Media>
                        <Image className="mr-4" src={this.props.image} alt={this.props.imageAlt}/>
                        <Media.Body>
                            {this.props.deliveredDate ?
                                (
                                    <span className="float-right text-info">Бронь на {this.props.deliveredDate}
                                        <Icofont icon="check-circled" className="text-success ml-1"/>
			                      </span>
                                )
                                : ""
                            }
                            <h6 className="mb-2">
                                <Link to={this.props.detailLink} className="text-black">{this.props.orderTitle} </Link>
                            </h6>
							<Badge pill variant={this.props.orderStateVariant} className='mr-1'>{this.props.orderState}</Badge>
                            <p className="text-gray mb-1">
                                <Icofont icon="location-arrow"/> {this.props.address}
                            </p>
                            <p className="text-gray mb-3">
                                <Icofont icon="list"/> Заказ #{this.props.orderNumber}
                                <Icofont icon="clock-time" className="ml-2"/> {this.props.orderDate}
                            </p>
                            <p className="text-dark">
                                {this.props.orderProducts}
                            </p>
                            <hr/>
                            <p className="mb-0 text-black text-primary pt-2">
                                <span className="text-black font-weight-bold"> Итого:</span> {this.props.orderTotal}
                            </p>
                        </Media.Body>
                    </Media>
                </div>
            </div>
        );
    }
}

OrderCard.propTypes = {
    image: PropTypes.string.isRequired,
    imageAlt: PropTypes.string,
    orderNumber: PropTypes.string.isRequired,
    orderDate: PropTypes.string.isRequired,
    deliveredDate: PropTypes.string,
    orderTitle: PropTypes.string.isRequired,
    address: PropTypes.string.isRequired,
    orderProducts: PropTypes.string.isRequired,
    helpLink: PropTypes.string.isRequired,
    detailLink: PropTypes.string.isRequired,
    orderTotal: PropTypes.string.isRequired,
    orderState: PropTypes.string.isRequired,
	orderStateVariant: PropTypes.string.isRequired
};
export default OrderCard;
