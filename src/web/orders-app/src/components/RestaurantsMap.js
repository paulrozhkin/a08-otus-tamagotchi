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
        alert(`Would you like to book a table at a restaurant '${name}' ${id}?`);
    }

    render() {

        let content = (<p><em>Loading...</em></p>);
        var center = this.state.position;

        console.log(this.props.restaurants);

        if (this.props.restaurants) {
            content = this.props.restaurants.map(item => (
                <Marker position={[item.latitude, item.longitude]}>
                    <Popup>
                        <b>{item.name}</b><br />
                        <div>{item.address}</div><br />
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

                {/*                 <Marker position={this.state.position}>
                    <Popup>
                        A pretty CSS3 popup. <br /> Easily customizable.
                        <Button >Order</Button>
                    </Popup>
                </Marker>

                <Marker position={[59.94431630927836, 30.348061808214337]}>
                    <Popup>
                        A pretty CSS3 popup. <br /> Easily customizable.
                    </Popup>
                </Marker>

                <Marker position={[59.9431630927836, 30.37061808214337]}>
                    <Popup>
                        A pretty CSS3 popup. <br /> Easily customizable.
                    </Popup>
                </Marker>
 */}
                {content}

            </MapContainer>);
    }
}

export default RestaurantsMap;