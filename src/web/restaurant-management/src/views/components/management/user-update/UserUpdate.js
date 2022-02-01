import React, {useEffect, useRef, useState} from 'react'
import {
  CButton,
  CCol,
  CForm,
  CFormCheck,
  CFormInput,
  CFormLabel, CFormSelect,
  CFormText,
  CImage,
  CRow,
  CSpinner
} from "@coreui/react";
import {useHistory, useParams} from "react-router-dom";
import axios from "axios";
import PlusImage from '../../../../assets/images/icons/icons8-plus-150.png'

const UserUpdate = (props) => {
  const history = useHistory();
  const {id} = useParams()
  const imageRef = useRef();

  const [isLoading, setIsLoading] = useState(true)

  const [user, setUser] = useState()
  const [username, setUsername] = useState('')
  const [name, setName] = useState('')
  const [role, setRole] = useState('')

  useEffect(() => {
    axios.get(`/users/${id}`)
      .then(response => {

        const user = response.data
        setUser(user)
        setUsername(user.userName)
        setName(user.name)
        setRole(user.roles[0])
        setIsLoading(false)
      })
      .catch((error) => {
          alert(error)
        }
      )
  }, [])

  const handleSubmit = (e) => {
    e.preventDefault()

    const updateUserDto = {
      name,
      roles: [role]
    }

    axios.put(`users/${id}`, updateUserDto)
      .then(res => {
        history.goBack()
      }).catch((error) => {
      alert(error)
    })
  }

  function handleDelete() {
    axios.delete(`users/${id}`)
      .then(res => {
        history.push('/users')
      }).catch((error) => {
      alert(error)
    })
  }

  return (
    <CRow>
      <CCol sm={10}>
        <h2 className="display-6">Update user #{id}</h2>
      </CCol>

      {isLoading && <CRow><CSpinner/> </CRow>}

      {!isLoading &&
        <CForm>

          <div className="mb-3">
            <CFormLabel htmlFor="usernameLabel">Username:</CFormLabel>
            <CFormLabel type="text" id="usernameLabel" className="ms-2">{username}</CFormLabel>
          </div>

          <div className="mb-3">
            <CFormLabel htmlFor="nameInput">Name:</CFormLabel>
            <CFormInput type="text" id="nameInput" value={name}
                        onChange={(e) => setName(e.target.value)}/>
          </div>

          <div className="mb-3">
            <CFormLabel htmlFor="rolesInput">Roles</CFormLabel>
            <CFormSelect
              value={role}
              onChange={(e) => setRole(e.target.value)}
              id="rolesInput">
              <option>Select user role</option>
              <option value="Administrator">Administrator</option>
              <option value="Stuff">Stuff</option>
              <option value="Client">Client</option>
            </CFormSelect>
          </div>

          <CButton type="submit" color="primary" onClick={handleSubmit} style={{width: 130}}>
            Update
          </CButton>

          <CButton type="submit" color="danger" onClick={handleDelete} className="mx-2" style={{width: 130}}>
            Delete
          </CButton>
        </CForm>}
    </CRow>
  )
}

export default UserUpdate
