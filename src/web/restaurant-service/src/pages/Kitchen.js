import React, {useEffect, useState} from 'react';
import {Container, ListGroup} from "react-bootstrap";
import {HubConnectionBuilder} from "@microsoft/signalr";
import {BASE_URL, KITCHEN_ORDER_HUB_URL} from "../config";
import axios from "axios";
import Order from "./Order";

const Kitchen = () => {
    const [connection, setConnection] = useState(null);
    const [orders, setOrders] = useState([]);
    const [isLoaded, setIsLoaded] = useState(false)

    useEffect(() => {
        axios.get("/OrderQueue")
            .then(res => {
                setIsLoaded(true)
                setOrders(res.data);
            })
            .catch(e => console.log(e.message()));
    }, []);

    useEffect(() => {
       const newConnection = new HubConnectionBuilder()
           .withUrl(KITCHEN_ORDER_HUB_URL)
           .withAutomaticReconnect()
           .build();

       setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
                    console.log('Connected!');

                    connection.on('messageReceived', message => {
                        console.log(message);
                        setOrders(orders => [...orders, message]);
                    })
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    });

    return (
        <Container>
            {!isLoaded && <p>Загрузка...</p>}
            {isLoaded && orders.length === 0 && <p>Активные заказы отсутсвуют</p>}
            <ListGroup>
                {orders.map(order => (<Order key={order.id} order={order} />))}
            </ListGroup>
        </Container>
    );
}

export default Kitchen;
