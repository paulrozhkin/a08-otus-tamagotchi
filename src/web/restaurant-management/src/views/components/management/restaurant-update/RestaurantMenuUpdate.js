import React, {useEffect, useState} from 'react';
import {CCol, CCollapse, CRow} from "@coreui/react";
import axios from "axios";
import MAX_INT32 from "const-max-int32";
import {useParams} from "react-router-dom";
import './ResturantMenuUpdate.css'
import {RestaurantMenuItem} from "./RestaurantMenuItem";
import {DishItem} from "./DishItem";

function RestaurantMenuUpdate() {
  const {id} = useParams()

  const [menu, setMenu] = useState([])
  const [isMenuLoaded, setIsMenuLoaded] = useState(false);
  const [menuVisible, setMenuVisible] = useState(true)

  const [dishes, setDishes] = useState([])
  const [isDishesLoaded, setIsDishesLoaded] = useState(false);
  const [dishesVisible, setDishesVisible] = useState(true)

  function handleDelete(event, menuItemForDelete) {
    event.preventDefault()

    axios.delete(`restaurants/${id}/menu/${menuItemForDelete.id}`)
      .then(res => {
        const menuWithoutDeleted = menu.filter(menuItem => menuItem !== menuItemForDelete)
        setMenu(menuWithoutDeleted)
        setDishes([...dishes, menuItemForDelete.dish])
      }).catch((error) => {
      alert(error)
    })
  }

  function handleAddDishToMenu(event, dishForAdd, price) {
    event.preventDefault()

    const menuDto = {
      DishId: dishForAdd.id,
      PriceRubles: price
    }

    axios.post(`restaurants/${id}/menu`, menuDto)
      .then(res => {
        const newMenuItem = res.data
        const dishesWithoutAdded = dishes.filter(dishItem => dishItem !== dishForAdd)
        setMenu([...menu, newMenuItem])
        setDishes(dishesWithoutAdded)
      }).catch((error) => {
      alert(error)
    })
  }

  function loadOtherDishes(menu) {
    axios.get(`/dishes?PageNumber=1&PageSize=${MAX_INT32}`)
      .then((result) => {
        const dishes = result.data.items

        const menuDishesIds = menu.map(menu => menu.dish.id)
        const notMenuDishes = dishes.filter(x => !menuDishesIds.includes(x.id))
        setDishes(notMenuDishes)
        setIsDishesLoaded(true)
      })
      .catch((error) => {
        alert(error)
      })
  }

  useEffect(() => {
    axios.get(`/restaurants/${id}/menu?PageNumber=1&PageSize=${MAX_INT32}`)
      .then((result) => {
        const menu = result.data.items
        setMenu(menu);
        setIsMenuLoaded(true)
        loadOtherDishes(menu)
      })
      .catch((error) => {
        alert(error)
      })
  }, [])

  const menuComponents = menu.map(menuItem => <RestaurantMenuItem key={menuItem.id} menu={menuItem}
                                                                  handleDelete={handleDelete}/>)
  const dishesComponents = dishes.map(dish => <DishItem key={dish.id} dish={dish}
                                                        handleAdd={handleAddDishToMenu}/>)

  return (<CRow>
    <CRow>
      <CCol className="mt-3" onClick={(event) => {
        event.preventDefault()
        setMenuVisible(!menuVisible)
      }}>
        <h3>Меню ресторана</h3>
      </CCol>
      <CCollapse visible={menuVisible}>
        <CRow xs={{cols: 1, gutter: 4}} md={{cols: 2}}>
          {isMenuLoaded && menuComponents}
          {!isMenuLoaded && <p>Loading</p>}
        </CRow>
      </CCollapse>
    </CRow>

    <CRow>
      <CCol className="mt-5" onClick={(event) => {
        event.preventDefault()
        setDishesVisible(!dishesVisible)
      }}>
        <h3>Доступные блюда</h3>
      </CCol>
      <CCollapse visible={dishesVisible}>
        <CRow xs={{cols: 1, gutter: 4}} md={{cols: 2}}>
          {isDishesLoaded && dishesComponents}
          {!isDishesLoaded && <p>Loading</p>}
        </CRow>
      </CCollapse>
    </CRow>
  </CRow>)
}

export default RestaurantMenuUpdate
