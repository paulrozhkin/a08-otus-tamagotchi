import React, {useEffect, useState} from 'react';
import OrderCard from '../common/OrderCard';
import {ordersService} from "../../services/orders-service";
import {imageService} from "../../services/image-service";

function Orders() {

	const [orders, setOrders] = useState([])
	const [isLoaded, setIsLoaded] = useState(false)

    useEffect(() => {
		ordersService.getAccountOrders().then(orders => {
			setOrders(orders.reverse())
			setIsLoaded(true)
		})
    }, [])

	function getDateTimeForUser(isoDateTime) {
		const date = new Date(isoDateTime)
		return `${date.toLocaleDateString()} ${date.toLocaleTimeString()}`
	}

	function getMenuFromOrder(order) {
		return order.menu.map(x => `${x.menuItem.dish.name} x ${x.count}`).join()
	}

	function mapOrderStatusToText(orderStatus) {
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
				return "Вы не явились"
		}
	}

	function mapOrderStatusToVariant(orderStatus) {
		switch (orderStatus) {
			case "Created":
				return "info"
			case "Wait":
				return "warning"
			case "Work":
				return "warning"
			case "Ready":
				return "warning"
			case "Completed":
				return "success"
			case "Skipped":
				return "danger"
		}
	}

    return (
        <>
            <div className='p-4 bg-white shadow-sm'>
                <h4 className="font-weight-bold mt-0 mb-4">Заказы</h4>
				{!isLoaded && <p>Загрузка...</p>}
				{isLoaded && orders.length === 0 && <p>Заказов нет</p>}
				{orders.length > 0 && orders.map(order =>
					(<OrderCard
						image={imageService.getUrlById(order.restaurant.photos[0])}
						imageAlt='Фото ресторана'
						orderNumber={order.id}
						orderDate={getDateTimeForUser(order.createdTime)}
						deliveredDate={getDateTimeForUser(order.visitTime)}
						orderTitle={`Заказ в ${order.restaurant.title}`}
						address={order.restaurant.address}
						orderProducts={getMenuFromOrder(order)}
						orderTotal={`₽${order.amountRubles}`}
						helpLink='#'
						detailLink='#'
						orderState={mapOrderStatusToText(order.orderStatus)}
						orderStateVariant={mapOrderStatusToVariant(order.orderStatus)}
					/>)
				)}
            </div>
        </>
    );
}

export default Orders;
