import React, { Component } from 'react';
import { MapContainer, TileLayer, Marker, Popup } from 'react-leaflet';
import 'leaflet/dist/leaflet.css';
import L from 'leaflet';

import icon from 'leaflet/dist/images/marker-icon.png';
import iconShadow from 'leaflet/dist/images/marker-shadow.png';
import { Button } from 'react-bootstrap';

let DefaultIcon = L.icon({
    iconUrl: icon,
    shadowUrl: iconShadow
});

L.Marker.prototype.options.icon = DefaultIcon;

class RestaurantsMap extends Component {

    constructor(props) {
        super(props);
        this.state = { position: [59.92411630927836, 30.348041808214337] };
    }

    handleItemChange = (e, data) => {
        // here the data is available 
        // ....
    }

    bookRestaurant(id, name) {

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ RestaurantId: id })
        };
        // TODO - вынести базовый адрес в настройки
        fetch(`http://localhost:5000/api/v1/Orders`, requestOptions)
            .then(async response => {
                const isJson = response.headers.get('content-type')?.includes('application/json');
                const data = isJson && await response.json();
    
                // check for error response
                if (!response.ok) {
                    // get error message from body or default to response status
                    const error = (data && data.message) || response.status;
                    return Promise.reject(error);
                }

                alert("Table was booked! Thank you!")
            })
            .catch(error => {
                this.setState({ errorMessage: error.toString() });
                console.error('There was an error!', error);
            });
    }

    render() {

        let content = (<p><em>Loading...</em></p>);
        var center = this.state.position;

        console.log(this.props.restaurants);

        if (this.props.restaurants) {
            content = this.props.restaurants.map(item => (
                <Marker key={item.id} position={[item.latitude, item.longitude]}>
                    <Popup>
                        <b>Restaurant id: {item.id}</b><br />
                        <b>{item.name}</b><br />
                        <div>{item.address}</div><br />
                        <div>Phone: {item.phoneNumber}</div><br/>
                        <Button onClick={() => this.bookRestaurant(item.id, item.name)} >Book a table</Button>
                    </Popup>
                </Marker>
            ));

            let firstRestaurant = this.props.restaurants[0];
            if (firstRestaurant) {
                center = [firstRestaurant.latitude, firstRestaurant.longitude];
            }
            console.log(center);
        }

        return (
            <MapContainer center={center} zoom={13} scrollWheelZoom={false}>
                <TileLayer
                    attribution='&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />

                {content}

            </MapContainer>);
    }
}

export default RestaurantsMap;
