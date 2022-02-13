import Icofont from "react-icofont";
import {Badge} from "react-bootstrap";
import {Link} from "react-router-dom";
import React from "react";

function RestaurantInfo(props) {
    const {restaurant} = props;

    const parkingColor = restaurant.isParkingPresent ? 'text-success' : 'text-danger'
    const parkingIcon = restaurant.isParkingPresent ? 'check-circled' : 'close-circled'

    const cardPaymentColor = restaurant.isCardPaymentPresent ? 'text-success' : 'text-danger'
    const cardPaymentIcon = restaurant.isCardPaymentPresent ? 'check-circled' : 'close-circled'

    const wiFiColor = restaurant.isWiFiPresent ? 'text-success' : 'text-danger'
    const wiFiIcon = restaurant.isWiFiPresent ? 'check-circled' : 'close-circled'

    return (
        <div id="restaurant-info"
             className="bg-white rounded shadow-sm p-4 mb-4">
            <div className="address-map float-right ml-5">
                <div className="mapouter">
                    <div className="gmap_canvas">
                        <iframe title='addressMap' width="300" height="170"
                                id="gmap_canvas"
                                src={`https://maps.google.com/maps?q=${restaurant.latitude},${restaurant.longitude}&t=&z=15&ie=UTF8&iwloc=&output=embed`}
                                frameBorder="0" scrolling="no" marginHeight="0"
                                marginWidth="0"></iframe>
                    </div>
                </div>
            </div>
            <h5 className="mb-4">Информация о ресторане</h5>
            <p className="mb-3">{restaurant.address}
            </p>
            <p className="mb-2 text-black"><Icofont
                icon="phone-circle text-primary mr-2"/> {restaurant.phoneNumber}</p>
            <p className="mb-2 text-black"><Icofont
                icon="email text-primary mr-2"/> tamagotchi@gmail.com</p>
            <p className="mb-2 text-black"><Icofont
                icon="clock-time text-primary mr-2"/> С 11:00 – 23:00
                <Badge variant="success" className='ml-1'> OPEN NOW </Badge>
            </p>
            <hr className="clearfix"/>

            <h5 className="mt-4 mb-4">More Info</h5>
            <div className="border-btn-main mb-4">
                <Link className={`border-btn ${parkingColor} mr-2`} to="#"><Icofont
                    icon={parkingIcon} /> Парковка </Link>
                <Link className={`border-btn ${cardPaymentColor} mr-2`} to="#"><Icofont
                    icon={cardPaymentIcon} /> Оплата по карте </Link>
                <Link className={`border-btn ${wiFiColor} mr-2`} to="#"><Icofont
                    icon={wiFiIcon} /> Wi-Fi </Link>
            </div>
        </div>
    )
}

export default RestaurantInfo
