import React, {useState} from 'react'
import {CButton, CCol, CForm, CFormInput, CFormLabel, CFormSelect, CRow} from "@coreui/react";
import {useHistory} from "react-router-dom";
import axios from "axios";

const UserCreate = (props) => {
  const history = useHistory();

  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [name, setName] = useState('')
  const [roles, setRoles] = useState([])

  const handleSubmit = (e) => {
    e.preventDefault()

    const newUserDto = {
      username,
      password,
      name,
      roles
    }

    axios.post('users', newUserDto)
      .then(res => {
        history.goBack()
      }).catch((error) => {
      alert(error)
    })
  }

  return (
    <CRow>
      <CCol sm={10}>
        <h2 className="display-6">Create new user</h2>
      </CCol>

      <CForm>
        <div className="mb-3">
          <CFormLabel htmlFor="usernameInput">Username</CFormLabel>
          <CFormInput type="text" id="usernameInput" value={username}
                      onChange={(e) => setUsername(e.target.value)}/>
        </div>

        <div className="mb-3">
          <CFormLabel htmlFor="passwordInput">Password</CFormLabel>
          <CFormInput type="password" id="passwordInput" value={password}
                      onChange={(e) => setPassword(e.target.value)}/>
        </div>

        <div className="mb-3">
          <CFormLabel htmlFor="nameInput">Name</CFormLabel>
          <CFormInput type="text" id="nameInput" value={name}
                      onChange={(e) => setName(e.target.value)}/>
        </div>

        <div className="mb-3">
          <CFormLabel htmlFor="rolesInput">Roles</CFormLabel>
          <CFormSelect
            onChange={(e) => setRoles([e.target.value])}
            id="rolesInput">
            <option>Select user role</option>
            <option value="Administrator">Administrator</option>
            <option value="Stuff">Stuff</option>
            <option value="Client">Client</option>
          </CFormSelect>
        </div>

        <CButton type="submit" color="primary" onClick={handleSubmit}>
          Submit
        </CButton>
      </CForm>
    </CRow>
  )
}

export default UserCreate
