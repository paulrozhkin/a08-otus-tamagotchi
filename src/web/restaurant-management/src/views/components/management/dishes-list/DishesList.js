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

const DishesList = () => {
  const history = useHistory();

  const [error, setError] = useState(null);
  const [isLoaded, setIsLoaded] = useState(false);
  const [dishesResponse, setDishesResponse] = useState([]);
  const [paginationPage, setPaginationPage] = useState(1);

  function editDish(event, id) {
    event.preventDefault()
    history.push(`/dishes/${id}/update`)
  }


  useEffect(() => {
    axios.get(`/Dishes?PageNumber=${paginationPage}&PageSize=13`)
      .then((result) => {
        setDishesResponse(result.data);
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
    const dishesDom = dishesResponse.items.map((dish) => {
      return <CTableRow key={dish.id} onClick={(event) => editDish(event, dish.id)}>
        <CTableHeaderCell scope="row">{dish.id}</CTableHeaderCell>
        <CTableDataCell>{dish.name}</CTableDataCell>
        <CTableDataCell>{dish.description}</CTableDataCell>
      </CTableRow>
    })

    const totalPages = dishesResponse.totalPages
    const currentPage = dishesResponse.currentPage
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
                <CTableHeaderCell scope="col">Name</CTableHeaderCell>
                <CTableHeaderCell scope="col">Description</CTableHeaderCell>
              </CTableRow>
            </CTableHead>
            <CTableBody>
              {dishesDom}
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
        <h2 className="display-6">Dishes</h2>
      </CCol>
      <CCol sm={2} className="d-flex my-auto justify-content-end">
        <CButton onClick={() => {
          history.push('/dishes/create')
        }} color="success">
          Create
        </CButton>
      </CCol>
      {content}
    </CRow>
  )
}

export default DishesList
