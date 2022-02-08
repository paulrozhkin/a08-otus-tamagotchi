import {CButton, CCard, CCardBody, CCardImage, CCardText, CCardTitle, CCol, CFormInput} from "@coreui/react";
import axios from "axios";
import React, {useState} from "react";

export function DishItem(props) {
  const dish = props.dish
  const firstPhoto = dish.photos[0]
  const [isEdit, setIsEdit] = useState(false)
  const [price, setPrice] = useState(0)
  const [priceValid, setPriceValid] = useState(true)

  function enableEdit(event) {
    event.preventDefault()
    if (!isEdit) {
      setIsEdit(true)
    }
  }

  function disableEdit(event) {
    event.preventDefault()
    setIsEdit(false)
  }

  function handleAdd(event) {
    event.preventDefault()

    const priceValue = parseInt(price)
    if (!!!priceValue) {
      setPriceValid(false)
      return
    }
    setPriceValid(true)

    props.handleAdd(event, dish, priceValue)
  }

  return (<CCol xl={4} onClick={enableEdit}>
    <CCard>
      <CCardImage orientation="top" src={`${axios.defaults.baseURL}/resources/${firstPhoto}`}/>
      <CCardBody>
        <CCardTitle>{dish.name}</CCardTitle>
        <CCardText>
          {dish.description}
        </CCardText>
        {isEdit &&
          <>
            <CCardText>
              <b>Цена ₽:</b>
              <CFormInput type="number" value={price}
                          onChange={(event) => setPrice(event.target.value)}
                          invalid={!priceValid}/>
            </CCardText>
            <CButton onClick={event => handleAdd(event, dish, price)} style={{width: 130}}>
              Add
            </CButton>
            <CButton color="danger" onClick={event => disableEdit(event)} className="mx-2"
                     style={{width: 130}}>
              Cancel
            </CButton>
          </>}
      </CCardBody>
    </CCard>
  </CCol>)
}
