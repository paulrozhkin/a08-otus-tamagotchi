import React, {useEffect, useState} from 'react';
import {Container} from "react-bootstrap";
import {HubConnectionBuilder} from "@microsoft/signalr";
import {BASE_URL, KITCHEN_ORDER_HUB_URL} from "../config";
import axios from "axios";

const Kitchen = () => {
    const [connection, setConnection] = useState(null);
    const [orders, setOrders] = useState([]);

    useEffect(() => {
        axios.get(BASE_URL + "api/v1/OrderQueue")
            .then(res => {
                const newOrders = res.data.map((order) => {
                    return { id: order.id, createTime: order.createTime };
                });
                setOrders(newOrders);
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
            <table className="table">
                <thead>
                    <tr>
                        <th>Номер заказа</th>
                        <th>Время создания</th>
                    </tr>
                </thead>
                <tbody>
                {
                    orders.map((order, idx) => (
                      <tr key={idx}>
                          <td>{order.id}</td>
                          <td>{order.createTime}</td>
                      </tr>
                    ))
                }
                </tbody>
            </table>
        </Container>
    );
}

export default Kitchen;