import React, {useEffect, useRef, useState} from 'react'
import {
  CButton,
  CCol,
  CForm,
  CFormInput,
  CFormLabel,
  CImage,
  CRow,
  CSpinner
} from "@coreui/react";
import {useHistory, useParams} from "react-router-dom";
import axios from "axios";
import PlusImage from '../../../../assets/images/icons/icons8-plus-150.png'

const DishesUpdate = (props) => {
  const history = useHistory();
  const {id} = useParams()
  const imageRef = useRef();

  const [isLoading, setIsLoading] = useState(true)

  const [dish, setDish] = useState()
  const [name, setName] = useState('')
  const [description, setDescription] = useState('')
  const [images, setImages] = useState([])
  const [selectedImage, setSelectedImage] = useState()

  useEffect(() => {
    axios.get(`/dishes/${id}`)
      .then(response => {

        const dish = response.data
        setDish(dish)
        setName(dish.name)
        setDescription(dish.description)
        setImages(dish.photos)
        setIsLoading(false)
      })
      .catch((error) => {
          alert(error)
        }
      )
  }, [])

  const handleSubmit = (e) => {
    e.preventDefault()

    const updateDishDto = {
      name,
      description,
      photos: images
    }

    axios.put(`dishes/${id}`, updateDishDto)
      .then(res => {
        history.push('/dishes')
      }).catch((error) => {
      alert(error)
    })
  }

  function handleDelete() {
    axios.delete(`dishes/${id}`)
      .then(res => {
        history.push('/dishes')
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
        <h2 className="display-6">Update dish #{id}</h2>
      </CCol>

      {isLoading && <CRow><CSpinner/> </CRow>}

      {!isLoading &&
        <CForm>

          <div className="my-3">
            {imagesContent}
          </div>

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

export default DishesUpdate
