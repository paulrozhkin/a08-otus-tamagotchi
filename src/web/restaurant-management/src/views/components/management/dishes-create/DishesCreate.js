import React, {useState} from 'react'
import {CButton, CCol, CForm, CFormInput, CFormLabel, CFormSelect, CRow} from "@coreui/react";
import {useHistory} from "react-router-dom";
import axios from "axios";

const DishesCreate = (props) => {
  const history = useHistory();

  const [name, setName] = useState('')
  const [description, setDescription] = useState('')

  const handleSubmit = (e) => {
    e.preventDefault()

    const newDishDto = {
      name,
      description
    }

    axios.post('dishes', newDishDto)
      .then(res => {
        history.goBack()
      }).catch((error) => {
      alert(error)
    })
  }

  return (
    <CRow>
      <CCol sm={10}>
        <h2 className="display-6">Create new dish</h2>
      </CCol>

      <CForm>
        <div className="mb-3">
          <CFormLabel htmlFor="nameInput">Dish title</CFormLabel>
          <CFormInput type="text" id="nameInput" value={name}
                      onChange={(e) => setName(e.target.value)}/>
        </div>

        <div className="mb-3">
          <CFormLabel htmlFor="descriptionInput">Dish description</CFormLabel>
          <CFormInput type="text" id="descriptionInput" value={description}
                      onChange={(e) => setDescription(e.target.value)}/>
        </div>

        <CButton type="submit" color="primary" onClick={handleSubmit}>
          Submit
        </CButton>
      </CForm>
    </CRow>
  )
}

export default DishesCreate
