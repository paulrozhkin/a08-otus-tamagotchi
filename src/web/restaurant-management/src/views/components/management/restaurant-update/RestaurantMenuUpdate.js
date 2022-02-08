import React, {useEffect, useState} from 'react';
import {
  CButton, CCard, CCardBody, CCardFooter, CCardImage, CCardText, CCardTitle, CCol, CCollapse, CRow, CFormInput
} from "@coreui/react";
import axios from "axios";
import MAX_INT32 from "const-max-int32";
import {useParams} from "react-router-dom";
import './ResturantMenuUpdate.css'

function RestaurantMenuItem(props) {
  const {id} = useParams()
  const menu = props.menu
  const firstPhoto = menu.dish.photos[0]
  const [price, setPrice] = useState(menu.priceRubles)
  const [priceValid, setPriceValid] = useState(true)
  const [isEdit, setIsEdit] = useState(false)

  function enableEdit(event) {
    event.preventDefault()
    setIsEdit(true)
  }

  function disableEdit(event) {
    event.preventDefault()
    setIsEdit(false)
  }

  function handleUpdate(event) {
    event.preventDefault()

    const priceValue = parseInt(price)
    if (!!!priceValue) {
      setPriceValid(false)
      return
    }

    setPriceValid(true)

    const updateMenuDto = {
      DishId: menu.dish.id,
      PriceRubles: price
    }

    axios.put(`restaurants/${id}/menu/${menu.id}`, updateMenuDto)
      .then(res => {
        setIsEdit(false)
      }).catch((error) => {
      alert(error)
    })
  }

  let content
  if (isEdit) {
    content = (
      <>
        <CCardText>
          <b>Цена ₽:</b>
          <CFormInput type="number" value={price}
                      onChange={(event) => setPrice(event.target.value)}
                      invalid={!priceValid}/>
        </CCardText>
        <CButton onClick={event => handleUpdate(event, menu, price)} style={{width: 130}}>
          Update
        </CButton>
        <CButton color="danger" onClick={event => disableEdit(event)} className="mx-2"
                 style={{width: 130}}>
          Cancel
        </CButton>
      </>
    )
  } else {
    content = (
      <>
        <CCardText>
          <b>Цена:</b> {price}₽
        </CCardText>
        <CButton onClick={event => enableEdit(event)} style={{width: 130}}>
          Edit
        </CButton>
        <CButton color="danger" onClick={event => props.handleDelete(event, menu)} className="mx-2"
                 style={{width: 130}}>
          Delete
        </CButton>
      </>
    )
  }

  return (<CCol xl>
    <CCard>
      <CCardImage orientation="top" src={`${axios.defaults.baseURL}/resources/${firstPhoto}`}/>
      <CCardBody>
        <CCardTitle>{menu.dish.name}</CCardTitle>
        <CCardText>
          {menu.dish.description}
        </CCardText>
        {content}
      </CCardBody>
    </CCard>
  </CCol>)
}

function DishItem(props) {
  const dish = props.dish
  const firstPhoto = dish.photos[0]

  return (<CCol xl>
    <CCard>
      <CCardImage orientation="top" src={`${axios.defaults.baseURL}/resources/${firstPhoto}`}/>
      <CCardBody>
        <CCardTitle>{dish.name}</CCardTitle>
        <CCardText>
          {dish.description}
        </CCardText>
      </CCardBody>
    </CCard>
  </CCol>)
}

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
  const dishesComponents = dishes.map(dish => <DishItem key={dish.id} dish={dish}/>)

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
