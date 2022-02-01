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

const UsersList = () => {
  const history = useHistory();

  const [error, setError] = useState(null);
  const [isLoaded, setIsLoaded] = useState(false);
  const [usersResponse, setUsersResponse] = useState([]);
  const [paginationPage, setPaginationPage] = useState(1);

  function editUser(event, id) {
    event.preventDefault()
    history.push(`/users/${id}/update`)
  }


  useEffect(() => {
    axios.get(`/Users?PageNumber=${paginationPage}&PageSize=13`)
      .then((result) => {
        setUsersResponse(result.data);
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
    const usersDom = usersResponse.items.map((user) => {
      return <CTableRow key={user.id} onClick={(event) => editUser(event, user.id)}>
        <CTableHeaderCell scope="row">{user.id}</CTableHeaderCell>
        <CTableDataCell>{user.userName}</CTableDataCell>
        <CTableDataCell>{user.roles.join()}</CTableDataCell>
      </CTableRow>
    })

    const totalPages = usersResponse.totalPages
    const currentPage = usersResponse.currentPage
    const pagesNumbers = Array.from({length: totalPages}, (_, i) => i + 1)

    const isNextPageDisabled = totalPages === currentPage
    const isPreviousPageDisabled = 1 === currentPage

    content = (<div>
      <CCol sm={12}>
        <CTable>
          <CTableHead>
            <CTableRow>
              <CTableHeaderCell scope="col">#</CTableHeaderCell>
              <CTableHeaderCell scope="col">Username</CTableHeaderCell>
              <CTableHeaderCell scope="col">Roles</CTableHeaderCell>
            </CTableRow>
          </CTableHead>
          <CTableBody>
            {usersDom}
          </CTableBody>
        </CTable>
      </CCol>

      <CCol sm={12}>
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
    </div>)
  }

  return (<CRow>
    <CCol sm={10}>
      <h2 className="display-6">Users</h2>
    </CCol>
    <CCol sm={2} className="d-flex my-auto justify-content-end">
      <CButton onClick={() => {
        history.push('/users/create')
      }} color="success">
        Create
      </CButton>
    </CCol>
    {content}
  </CRow>)
}

export default UsersList
