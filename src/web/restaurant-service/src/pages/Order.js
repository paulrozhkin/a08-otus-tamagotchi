import {Button, Col, ListGroup, Row} from "react-bootstrap";
import {useState} from "react";
import axios from "axios";

function Order(props) {
    const {order} = props
    const [status, setStatus] = useState(order.orderStatus)

    function getMenuFromOrder(order) {
        return order.menu.map(x => `${x.menuItem.dish.name} x ${x.count}`).join()
    }

    function mapOrderStatusToText(orderStatus) {
        console.log(status)
        switch (orderStatus) {
            case "Created":
                return "Принят"
            case "Wait":
                return "Поступил на кухню"
            case "Work":
                return "Готовится"
            case "Ready":
                return "Готов"
            case "Completed":
                return "Завершен"
            case "Skipped":
                return "Посетитель не явился"
        }
    }

    function mapOrderStatusToNextStatusText(orderStatus) {
        switch (orderStatus) {
            case "Wait":
                return "Приступить к готовке"
            case "Work":
                return "Приготовлен"
            case "Ready":
                return "Завершить"
        }
    }

    function mapToNextState(orderStatus) {
        switch (orderStatus) {
            case "Wait":
                return "Work"
            case "Work":
                return "Ready"
            case "Ready":
                return "Completed"
            default:
                return "Terminated"
        }
    }

    function nextState() {
        axios.post(`/orders/${order.id}/status`)
            .then(res => {
                setStatus(res.data.orderStatus)
            }, e => {
                alert(e)
            })
    }

    return (
        <ListGroup.Item>
            <Row>
                <Col md={8}>
                    <h4>Заказ #{order.id}</h4>
                </Col>
                <Col>
                    <p className="text-center">Создан: {new Date(order.createdTime).toLocaleString()}</p>
                </Col>
            </Row>
            <Row>
                <p>Ресторан: {order.restaurant.title}</p>
            </Row>
            <Row>
                <p>Заказ: {getMenuFromOrder(order)}</p>
            </Row>
            <Row>
                <p>Время прихода: {new Date(order.visitTime).toLocaleString()}</p>
            </Row>
            <Row>
                <p>Состояние: {mapOrderStatusToText(status)}</p>
            </Row>
            <Row>
                {mapToNextState(status) !== "Terminated" &&
                    <Button onClick={nextState}>{mapOrderStatusToNextStatusText(status)}</Button>}
            </Row>
        </ListGroup.Item>
    )
}

export default Order
