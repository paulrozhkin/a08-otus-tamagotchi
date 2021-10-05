import React, { Component } from 'react';
import { MapContainer, TileLayer, Marker, Popup } from 'react-leaflet';
import 'leaflet/dist/leaflet.css';
import L from 'leaflet';

import icon from 'leaflet/dist/images/marker-icon.png';
import iconShadow from 'leaflet/dist/images/marker-shadow.png';

let DefaultIcon = L.icon({
    iconUrl: icon,
    shadowUrl: iconShadow
});

L.Marker.prototype.options.icon = DefaultIcon;

class Map extends Component {

    constructor(props) {
        super(props);
        this.state = {position : [59.92411630927836, 30.348041808214337]};
    }

    render() {
        return(
        <MapContainer center={this.state.position} zoom={13} scrollWheelZoom={false}>
            <TileLayer
                attribution='&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            />

            <Marker position={this.state.position}>
                <Popup>
                    A pretty CSS3 popup. <br /> Easily customizable.
                </Popup>
            </Marker>

{/*             {this.props.items.map(item => (
                <Marker position={item.Latitude, item.Longitude}></Marker>
            ))} */}

        </MapContainer>);
    }
}

export default Map;