import {useParams} from "react-router-dom";
import React, {useState} from "react";
import axios from "axios";
import {CButton, CCard, CCardBody, CCardImage, CCardText, CCardTitle, CCol, CFormInput} from "@coreui/react";

export function RestaurantMenuItem(props) {
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
            DishId: menu.dish.id, PriceRubles: price
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
        content = (<>
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
        </>)
    } else {
        content = (<>
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
        </>)
    }

    return (<CCol xl={4}>
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
