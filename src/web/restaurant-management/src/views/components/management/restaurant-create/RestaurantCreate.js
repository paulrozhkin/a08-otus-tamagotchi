import React, {useState} from 'react'
import {CButton, CCol, CForm, CFormCheck, CFormInput, CFormLabel, CFormText, CRow} from "@coreui/react";
import {useHistory} from "react-router-dom";

const RestaurantCreate = (props) => {
  const history = useHistory();

  const [latitude, setLatitude] = useState(0)
  const [longitude, setLongitude] = useState(0)
  const [phoneNumber, setPhoneNumber] = useState('')
  const [isParkingPresent, setIsParkingPresent] = useState(false)
  const [isCardPaymentPresent, setIsCardPaymentPresent] = useState(false)
  const [isWiFiPresent, setIsWiFiPresent] = useState(false)

  const handleSubmit = (e) => {
    e.preventDefault()

    const newRestaurantDto = {
      latitude,
      longitude,
      phoneNumber,
      isParkingPresent,
      isCardPaymentPresent,
      isWiFiPresent
    }

    console.log(newRestaurantDto)

    fetch('http://localhost:5000/api/v1/Restaurants', {
      method: 'POST',
      headers: {"Content-Type": "application/json"},
      body: JSON.stringify(newRestaurantDto)
    }).then(res => res.json())
      .then((response) => {
          history.goBack()
        },
        (error) => {
          console.log(error)
        })

  }

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
              <CFormInput type="number" id="latitudeInput" value={latitude}
                          onChange={(e) => setLatitude(e.target.value)}/>
            </div>
          </CCol>

          <CCol sm={6}>
            <div className="mb-3">
              <CFormLabel htmlFor="longitudeInput">Longitude</CFormLabel>
              <CFormInput type="number" id="longitudeInput" value={longitude}
                          onChange={(e) => setLongitude(e.target.value)}/>
            </div>
          </CCol>
        </CRow>

        <div className="mb-3">
          <CFormLabel htmlFor="phoneNumberInput">Phone number</CFormLabel>
          <CFormInput type="tel" id="phoneNumberInput" value={phoneNumber} placeholder="+7910 120 54 54"
                      onChange={(e) => setPhoneNumber(e.target.value)}/>
        </div>

        <CFormCheck
          className="mb-3"
          label="Is parking present"
          checked={isParkingPresent}
          onChange={(e) => setIsParkingPresent(e.target.checked)}
        />

        <CFormCheck
          className="mb-3"
          label="Is card payment present"
          checked={isCardPaymentPresent}
          onChange={(e) => setIsCardPaymentPresent(e.target.checked)}
        />

        <CFormCheck
          className="mb-3"
          label="Is WiFi Present"
          checked={isWiFiPresent}
          onChange={(e) => setIsWiFiPresent(e.target.checked)}
        />

        <CButton type="submit" color="primary" onClick={handleSubmit}>
          Submit
        </CButton>
      </CForm>
    </CRow>
  )
}

export default RestaurantCreate
