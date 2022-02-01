import React, {useEffect, useRef, useState} from 'react'
import {
  CButton,
  CCol,
  CForm,
  CFormCheck,
  CFormInput,
  CFormLabel,
  CFormText,
  CImage,
  CRow,
  CSpinner
} from "@coreui/react";
import {useHistory, useParams} from "react-router-dom";
import axios from "axios";
import PlusImage from '../../../../assets/images/icons/icons8-plus-150.png'

const RestaurantUpdate = (props) => {
  const history = useHistory();
  const {id} = useParams()
  const imageRef = useRef();

  const [restaurant, setRestaurant] = useState()
  const [isLoading, setIsLoading] = useState(true)
  const [address, setAddress] = useState('')
  const [phoneNumber, setPhoneNumber] = useState('')
  const [isParkingPresent, setIsParkingPresent] = useState(false)
  const [isCardPaymentPresent, setIsCardPaymentPresent] = useState(false)
  const [isWiFiPresent, setIsWiFiPresent] = useState(false)
  const [images, setImages] = useState([])
  const [selectedImage, setSelectedImage] = useState()

  useEffect(() => {
    axios.get(`/restaurants/${id}`)
      .then(response => {

        const restaurant = response.data
        setRestaurant(restaurant)
        setAddress(restaurant.address)
        setPhoneNumber(restaurant.phoneNumber)
        setIsParkingPresent(restaurant.isParkingPresent)
        setIsCardPaymentPresent(restaurant.isCardPaymentPresent)
        setIsWiFiPresent(restaurant.isWiFiPresent)
        setImages(restaurant.photos)
        setIsLoading(false)
      })
      .catch((error) => {
          alert(error)
        }
      )
  }, [])

  const handleSubmit = (e) => {
    e.preventDefault()

    const updateRestaurantDto = {
      latitude: restaurant.latitude,
      longitude: restaurant.longitude,
      phoneNumber,
      isParkingPresent,
      isCardPaymentPresent,
      isWiFiPresent,
      photos: images
    }

    axios.put(`restaurants/${id}`, updateRestaurantDto)
      .then(res => {
        history.goBack()
      }).catch((error) => {
      alert(error)
    })
  }

  function handleDelete() {
    axios.delete(`restaurants/${id}`)
      .then(res => {
        history.push('/restaurants')
      }).catch((error) => {
      alert(error)
    })
  }

  const imagesContent = []

  function selectImage(event) {
    event.preventDefault()
    imageRef.current.click()
  }

  function onInputImageChange(event) {
    const file = event.target.files[0];
    setSelectedImage(file);
  }

  useEffect(() => {
    if (selectedImage) {
      const formData = new FormData();
      formData.append("file", selectedImage);

      axios.post('/resources', formData)
        .then(r => {
          setImages([...images, r.data.id])
          setSelectedImage(null)
          imageRef.current.value = ''
        }).catch(e => {
        alert(e)
      })
    }
  }, [selectedImage]);

  function removeImage(event, imageUrl) {
    event.preventDefault()
    const newImages = images.filter(x => x !== imageUrl)
    setImages(newImages)
  }

  if (!isLoading) {

    if (images.length !== 0) {
      imagesContent.push(images.map(imageUrl => <CImage key={imageUrl} rounded className="m-2"
                                                        src={`${axios.defaults.baseURL}/resources/${imageUrl}`}
                                                        onClick={(event) => removeImage(event, imageUrl)}
                                                        width={200} height={200}/>))

    }

    imagesContent.push(
      <CImage key={'plus'} rounded
              src={PlusImage}
              className="m-5"
              onClick={selectImage}
              width={100} height={100}/>)
  }

  return (
    <CRow>
      <CCol sm={10}>
        <h2 className="display-6">Update restaurant #{id}</h2>
      </CCol>

      {isLoading && <CRow><CSpinner/> </CRow>}

      {!isLoading &&
        <CForm>

          <div className="my-3">
            {imagesContent}
          </div>

          <div className="mb-3">
            <CFormLabel htmlFor="addressLabel">Address:</CFormLabel>
            <CFormLabel type="text" id="addressLabel" className="ms-2">{address}</CFormLabel>
          </div>

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

          <CButton type="submit" color="primary" onClick={handleSubmit} style={{width: 130}}>
            Update
          </CButton>

          <CButton type="submit" color="danger" onClick={handleDelete} className="mx-2" style={{width: 130}}>
            Delete
          </CButton>
        </CForm>}

      <input
        ref={imageRef}
        type="file"
        style={{ display: 'none' }}
        accept="image/*"
        onChange={onInputImageChange}
      />
    </CRow>
  )
}

export default RestaurantUpdate
