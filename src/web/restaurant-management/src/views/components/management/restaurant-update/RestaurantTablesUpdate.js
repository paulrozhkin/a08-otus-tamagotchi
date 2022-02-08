import React, {useEffect, useState} from 'react';
import {
  CButton, CCloseButton,
  CCol, CFormInput,
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
import {cilCheckCircle, cilXCircle} from '@coreui/icons';
import CIcon from "@coreui/icons-react";
import "./RestaurantTablesUpdate.css"

function RestaurantTablesUpdate() {
  const {id} = useParams()

  const [newTableName, setNewTableName] = useState('')
  const [newTableNumberOfPlace, setNewTableNumberOfPlace] = useState('')
  const [isNewTableNumberOfPlaceValid, setIsNewTableNumberOfPlaceValid] = useState(true)
  const [isNewTableNameValid, setIsNewTableNameValid] = useState(true)


  const [error, setError] = useState(null);
  const [isLoaded, setIsLoaded] = useState(false);

  const [tables, setTables] = useState([])

  useEffect(() => {
    axios.get(`/restaurants/${id}/tables?PageNumber=1&PageSize=${MAX_INT32}`)
      .then((result) => {
        const tablesFromServer = result.data.items.map((table) => {
          return {...table, isSaved: true}
        });

        setTables(tablesFromServer);
        setIsLoaded(true);
      })
      .catch((error) => {
        setError(error);
        setIsLoaded(true);
      })
  }, [])

  function deleteTable(event, tableId) {
    event.preventDefault()

    if (tableId === "-1") {
      removeTableFromView(tableId)
      return
    }

    axios.delete(`/restaurants/${id}/tables/${tableId}`)
      .then((result) => {
        removeTableFromView(tableId)
      }).catch((error) => {
      alert(error)
    })
  }

  function removeTableFromView(tableId) {
    const newTables = tables.filter(x => x.id !== tableId)
    if (newTables.length !== tables.length) {
      setTables(newTables)
    }
  }

  function createNewTable(event) {
    event.preventDefault()

    let newTable = tables.filter(x => x.id === "-1")

    if (newTable.length !== 0) {
      setNewTableName('Новый столик')
      setNewTableNumberOfPlace(2)
      return
    }

    newTable = {
      id: "-1",
      name: 'Новый столик',
      numberOfPlaces: 2,
      isSaved: false
    }

    setIsNewTableNameValid(true)
    setIsNewTableNumberOfPlaceValid(true)

    setNewTableName(newTable.name)
    setNewTableNumberOfPlace(newTable.numberOfPlaces)
    setTables([...tables, newTable])
  }

  function saveTable(event, table) {
    event.preventDefault()

    let newIsNewTableNameValid = true
    if (!!!newTableName) {
      newIsNewTableNameValid = false
    }

    let newIsNewTableNumberOfPlaceValid = true
    const numberOfPlaces = parseInt(newTableNumberOfPlace)
    if (!!!numberOfPlaces) {
      newIsNewTableNumberOfPlaceValid = false
    }

    setIsNewTableNameValid(newIsNewTableNameValid)
    setIsNewTableNumberOfPlaceValid(newIsNewTableNumberOfPlaceValid)

    if (!(newIsNewTableNameValid && newIsNewTableNumberOfPlaceValid)) {
      return
    }

    setIsNewTableNameValid(true)
    setIsNewTableNumberOfPlaceValid(true)

    const newTableDto = {
      name: newTableName,
      numberOfPlaces: numberOfPlaces
    }

    axios.post(`restaurants/${id}/tables`, newTableDto)
      .then(res => {
        const tableResponse = res.data

        const table = {...tableResponse, isSaved: true}
        const tablesWithoutCreate = tables.filter(x => x.id !== "-1")
        setTables([...tablesWithoutCreate, table])
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
      if (table.isSaved) {
        return <CTableRow key={table.id}>
          <CTableHeaderCell scope="row">{table.id}</CTableHeaderCell>
          <CTableDataCell>{table.name}</CTableDataCell>
          <CTableDataCell>{table.numberOfPlaces}</CTableDataCell>
          <CTableDataCell>
            <CIcon className="mt-1 action-button" width={25} height={25} icon={cilXCircle}
                   onClick={event => deleteTable(event, table.id)}/>
          </CTableDataCell>
        </CTableRow>
      } else {
        return <CTableRow key={"-1"} className="content">
          <CTableHeaderCell scope="row"/>
          <CTableDataCell><CFormInput type="text" value={newTableName}
                                      onChange={event => setNewTableName(event.target.value)}
                                      invalid={!isNewTableNameValid}
          /></CTableDataCell>
          <CTableDataCell><CFormInput type="number" value={newTableNumberOfPlace}
                                      onChange={event => setNewTableNumberOfPlace(event.target.value ?? 0)}
                                      invalid={!isNewTableNumberOfPlaceValid}/></CTableDataCell>
          <CTableDataCell width={90}>
            <CRow>
              <CCol>
                <CIcon className="action-button" width={25} height={25} icon={cilCheckCircle}
                       onClick={event => saveTable(event, table)}/>
              </CCol>
              <CCol>
                <CIcon className="action-button" width={25} height={25} icon={cilXCircle}
                       onClick={event => deleteTable(event, table.id)}/>
              </CCol>
            </CRow>
          </CTableDataCell>
        </CTableRow>
      }
    })
  }


  return (
    <CRow>
      <CCol sm={10}>
        <h2 className="display-6">Restaurant tables</h2>
      </CCol>
      <CCol sm={2} className="d-flex my-auto justify-content-end">
        <CButton onClick={(event) => {
          createNewTable(event)
        }}
                 color="success">
          Create
        </CButton>
      </CCol>

      <div>
        <CCol sm={12}>
          <CTable align="middle">
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
