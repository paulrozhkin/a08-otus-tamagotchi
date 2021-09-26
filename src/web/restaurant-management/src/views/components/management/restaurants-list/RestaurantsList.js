import React from 'react'
import { Link } from 'react-router'
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

const RestaurantsList = (props) => {
  return (
    <CRow>
      <CCol sm={10}>
        <h2 className="display-6">Restaurants</h2>
      </CCol>
      <CCol sm={2} className="d-flex my-auto justify-content-end">
        <CButton color="success">Create</CButton>
      </CCol>

      <CCol sm={12}>
        <CForm className="d-flex">
          <CFormInput type="search" className="me-2" placeholder="Search" />
          <CButton type="submit" color="success" variant="outline">
            Search
          </CButton>
        </CForm>
      </CCol>

      <CCol sm={12}>
        <CTable>
          <CTableHead>
            <CTableRow>
              <CTableHeaderCell scope="col">#</CTableHeaderCell>
              <CTableHeaderCell scope="col">Address</CTableHeaderCell>
              <CTableHeaderCell scope="col">Heading</CTableHeaderCell>
              <CTableHeaderCell scope="col">Heading</CTableHeaderCell>
            </CTableRow>
          </CTableHead>
          <CTableBody>
            <CTableRow>
              <CTableHeaderCell scope="row">1</CTableHeaderCell>
              <CTableDataCell>Address 1</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
            </CTableRow>
            <CTableRow>
              <CTableHeaderCell scope="row">2</CTableHeaderCell>
              <CTableDataCell>Address 2</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
            </CTableRow>
            <CTableRow>
              <CTableHeaderCell scope="row">3</CTableHeaderCell>
              <CTableDataCell>Address 3</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
            </CTableRow>
            <CTableRow>
              <CTableHeaderCell scope="row">4</CTableHeaderCell>
              <CTableDataCell>Address 4</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
            </CTableRow>
            <CTableRow>
              <CTableHeaderCell scope="row">5</CTableHeaderCell>
              <CTableDataCell>Address 5</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
            </CTableRow>
            <CTableRow>
              <CTableHeaderCell scope="row">6</CTableHeaderCell>
              <CTableDataCell>Address 6</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
            </CTableRow>
            <CTableRow>
              <CTableHeaderCell scope="row">7</CTableHeaderCell>
              <CTableDataCell>Address 7</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
            </CTableRow>
            <CTableRow>
              <CTableHeaderCell scope="row">8</CTableHeaderCell>
              <CTableDataCell>Address 8</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
            </CTableRow>
            <CTableRow>
              <CTableHeaderCell scope="row">9</CTableHeaderCell>
              <CTableDataCell>Address 9</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
            </CTableRow>
            <CTableRow>
              <CTableHeaderCell scope="row">10</CTableHeaderCell>
              <CTableDataCell>Address 10</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
            </CTableRow>
            <CTableRow>
              <CTableHeaderCell scope="row">11</CTableHeaderCell>
              <CTableDataCell>Address 11</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
            </CTableRow>
            <CTableRow>
              <CTableHeaderCell scope="row">12</CTableHeaderCell>
              <CTableDataCell>Address 12</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
            </CTableRow>
            <CTableRow>
              <CTableHeaderCell scope="row">13</CTableHeaderCell>
              <CTableDataCell>Address 13</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
              <CTableDataCell>???</CTableDataCell>
            </CTableRow>
          </CTableBody>
        </CTable>
      </CCol>

      <CCol sm={12}>
        <CPagination align="center" aria-label="Page navigation example">
          <CPaginationItem disabled>Previous</CPaginationItem>
          <CPaginationItem>1</CPaginationItem>
          <CPaginationItem>2</CPaginationItem>
          <CPaginationItem>3</CPaginationItem>
          <CPaginationItem>Next</CPaginationItem>
        </CPagination>
      </CCol>
    </CRow>
  )
}

export default RestaurantsList
