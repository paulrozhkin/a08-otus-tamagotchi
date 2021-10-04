import React from 'react'
import {CButton, CCol, CForm, CFormCheck, CFormInput, CFormLabel, CFormText, CRow} from "@coreui/react";
import {useHistory} from "react-router-dom";

const RestaurantCreate = (props) => {
  const history = useHistory();

  return (
    <CRow>
      <CCol sm={10}>
        <h2 className="display-6">Create new restaurant</h2>
      </CCol>

      <CForm>
        <CRow>
          <CCol sm={6}>
            <div className="mb-3">
              <CFormLabel htmlFor="latitudeInput">Latitude</CFormLabel>
              <CFormInput type="number" id="latitudeInput"/>
            </div>
          </CCol>

          <CCol sm={6}>
            <div className="mb-3">
              <CFormLabel htmlFor="longitudeInput">Longitude</CFormLabel>
              <CFormInput type="number" id="longitudeInput"/>
            </div>
          </CCol>
        </CRow>

        <div className="mb-3">
          <CFormLabel htmlFor="phoneNumberInput">Phone number</CFormLabel>
          <CFormInput type="text" id="phoneNumberInput"/>
        </div>

        <CFormCheck
          className="mb-3"
          label="Is parking present"
        />

        <CFormCheck
          className="mb-3"
          label="Is card payment present"
        />

        <CFormCheck
          className="mb-3"
          label="Is WiFi Present"
        />

        <CButton type="submit" color="primary" onClick={() => {
          history.goBack()
        }}>
          Submit
        </CButton>
      </CForm>
    </CRow>
  )
}

export default RestaurantCreate
