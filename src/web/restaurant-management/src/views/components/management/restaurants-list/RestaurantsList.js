import React, {useEffect, useState} from 'react'
import {
  CButton,
  CCol,
  CForm,
  CFormInput,
  CPagination,
  CPaginationItem,
  CRow,
  CTable,
  CTableBody,
  CTableDataCell,
  CTableHead,
  CTableHeaderCell,
  CTableRow,
} from '@coreui/react'
import {useHistory} from "react-router-dom";
import axios from "axios";

const RestaurantsList = () => {
  const history = useHistory();

  const [error, setError] = useState(null);
  const [isLoaded, setIsLoaded] = useState(false);
  const [restaurantsResponse, setRestaurantsResponse] = useState([]);
  const [paginationPage, setPaginationPage] = useState(1);

  function editRestaurant(event, id) {
    event.preventDefault()
    history.push(`/restaurants/${id}/update`)
  }


  useEffect(() => {
    axios.get(`/Restaurants?PageNumber=${paginationPage}&PageSize=13`)
      .then((result) => {
        setRestaurantsResponse(result.data);
        setIsLoaded(true);
      })
      .catch((error) => {
        setError(error);
        setIsLoaded(true);
      })
  }, [paginationPage])

  let content;
  if (error) {
    content = (<div>Error: {error.message}</div>)
  } else if (!isLoaded) {
    content = (<div>Loading...</div>)
  } else {
    const restaurantsDom = restaurantsResponse.items.map((restaurant) => {
      return <CTableRow key={restaurant.id} onClick={(event) => editRestaurant(event, restaurant.id)}>
        <CTableHeaderCell scope="row">{restaurant.id}</CTableHeaderCell>
        <CTableDataCell>{restaurant.title}</CTableDataCell>
        <CTableDataCell>{restaurant.phoneNumber}</CTableDataCell>
      </CTableRow>
    })

    const totalPages = restaurantsResponse.totalPages
    const currentPage = restaurantsResponse.currentPage
    const pagesNumbers = Array.from({length: totalPages}, (_, i) => i + 1)

    const isNextPageDisabled = totalPages === currentPage
    const isPreviousPageDisabled = 1 === currentPage

    content = (
      <div>
        <CCol sm={12}>
          <CTable>
            <CTableHead>
              <CTableRow>
                <CTableHeaderCell scope="col">#</CTableHeaderCell>
                <CTableHeaderCell scope="col">Title</CTableHeaderCell>
                <CTableHeaderCell scope="col">Phone number</CTableHeaderCell>
              </CTableRow>
            </CTableHead>
            <CTableBody>
              {restaurantsDom}
            </CTableBody>
          </CTable>
        </CCol>

        <CCol sm={12} >
          <CPagination align="center" aria-label="Page navigation example">
            <CPaginationItem disabled={isPreviousPageDisabled} onClick={() => {
              setPaginationPage(currentPage - 1)
            }}>Previous</CPaginationItem>
            {pagesNumbers.map((number) => {
              const isActive = currentPage === number
              return <CPaginationItem key={number} active={isActive}
                                      onClick={() => {
                                        setPaginationPage(number)
                                      }}
              >
                {number}
              </CPaginationItem>
            })}
            <CPaginationItem disabled={isNextPageDisabled} onClick={() => {
              setPaginationPage(currentPage + 1)
            }}>Next</CPaginationItem>
          </CPagination>
        </CCol>
      </div>
    )
  }

  return (
    <CRow>
      <CCol sm={10}>
        <h2 className="display-6">Restaurants</h2>
      </CCol>
      <CCol sm={2} className="d-flex my-auto justify-content-end">
        <CButton onClick={() => {
          history.push('/restaurants/create')
        }} color="success">
          Create
        </CButton>
      </CCol>

      {content}
    </CRow>
  )
}

export default RestaurantsList
