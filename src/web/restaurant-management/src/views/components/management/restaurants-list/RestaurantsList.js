import React from 'react'
import { CButton, CCard, CCardBody, CCardHeader, CCol, CRow } from '@coreui/react'

const RestaurantsList = (props) => {
  return (
    <CRow>
      <CCol sm={10}>
        <h2 className="display-6">Restaurants</h2>
      </CCol>
      <CCol sm={2} className="my-auto">
        <CButton color="success">Create</CButton>
      </CCol>
    </CRow>
  )
}

export default RestaurantsList
