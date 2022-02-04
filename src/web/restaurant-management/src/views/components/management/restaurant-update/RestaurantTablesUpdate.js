import React, {useEffect, useState} from 'react';
import {
  CButton, CCloseButton,
  CCol,
  CRow,
  CTable,
  CTableBody,
  CTableDataCell,
  CTableHead,
  CTableHeaderCell,
  CTableRow
} from "@coreui/react";
import {useParams} from "react-router-dom";
import axios from "axios";
import MAX_INT32 from 'const-max-int32';

function RestaurantTablesUpdate() {
  const {id} = useParams()

  const [error, setError] = useState(null);
  const [isLoaded, setIsLoaded] = useState(false);

  const [tables, setTables] = useState([])

  useEffect(() => {
    axios.get(`/restaurants/${id}/tables?PageNumber=1&PageSize=${MAX_INT32}`)
      .then((result) => {
        setTables(result.data.items);
        setIsLoaded(true);
      })
      .catch((error) => {
        setError(error);
        setIsLoaded(true);
      })
  }, [])

  function deleteTable(event, tableId) {
    event.preventDefault()
    axios.delete(`/restaurants/${id}/tables/${tableId}`)
      .then((result) => {
        const newTables = tables.filter(x => x.id !== tableId)
        setTables(newTables)
      }).catch((error) => {
      alert(error)
    })
  }

  let tablesDom;
  if (error) {
    tablesDom = (<div>Error: {error.message}</div>)
  } else if (!isLoaded) {
    tablesDom = (<div>Loading...</div>)
  } else {
    tablesDom = tables.map((table) => {

      return <CTableRow key={table.id}>
        <CTableHeaderCell scope="row">{table.id}</CTableHeaderCell>
        <CTableDataCell>{table.name}</CTableDataCell>
        <CTableDataCell>{table.numberOfPlaces}</CTableDataCell>
        <CTableDataCell>
          <CCloseButton className="mt-1" onClick={event => deleteTable(event, table.id)}/>
        </CTableDataCell>
      </CTableRow>
    })
  }


  return (
    <CRow>
      <CCol sm={10}>
        <h2 className="display-6">Restaurant tables</h2>
      </CCol>
      <CCol sm={2} className="d-flex my-auto justify-content-end">
        <CButton onClick={() => {
        }} color="success">
          Create
        </CButton>
      </CCol>

      <div>
        <CCol sm={12}>
          <CTable>
            <CTableHead>
              <CTableRow>
                <CTableHeaderCell scope="col">#</CTableHeaderCell>
                <CTableHeaderCell scope="col">Title</CTableHeaderCell>
                <CTableHeaderCell scope="col">Count of chairs</CTableHeaderCell>
                <CTableHeaderCell scope="col"/>
              </CTableRow>
            </CTableHead>
            <CTableBody>
              {tablesDom}
            </CTableBody>
          </CTable>
        </CCol>
      </div>
    </CRow>
  )
}

export default RestaurantTablesUpdate
