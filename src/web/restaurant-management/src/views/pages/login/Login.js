import React, {useState} from 'react'
import {
  CButton,
  CCard,
  CCardBody,
  CCardGroup, CCol,
  CContainer,
  CForm,
  CFormInput,
  CInputGroup,
  CInputGroupText,
  CRow,
} from '@coreui/react'
import CIcon from '@coreui/icons-react'
import { cilLockLocked, cilUser } from '@coreui/icons'
import { useHistory } from 'react-router-dom';
import {userService} from "../../../services/user-service";
import {connect} from "react-redux";
import mapDispatchToProps from "react-redux/lib/connect/mapDispatchToProps";
import {bindActionCreators} from "redux";
import {login} from "../../../actions/login";

const Login = (props) => {

  const [form, setForm] = useState({})
  const [errors, setErrors] = useState({})
  const history = useHistory();

  const setField = (field, value) => {
    setForm({
      ...form,
      [field]: value
    })

    // Check and see if errors exist, and remove them from the error object:
    if ( !!errors[field] ) setErrors({
      ...errors,
      [field]: null
    })
  }

  const findFormErrors = () => {
    const {login, password} = form
    const newErrors = {}
    // email errors
    if (!login || login === '') newErrors.login = 'cannot be blank!'
    else if (login.length > 50) newErrors.login = 'login is too long!'
    // password errors
    if (!password || password === '') newErrors.password = 'cannot be blank!'
    else if (password.length < 4) newErrors.password = 'password is too short!'

    return newErrors
  }

  function handleSubmit(event) {
    event.preventDefault();

    // get our new errors
    const newErrors = findFormErrors()

    // Conditional logic:
    if (Object.keys(newErrors).length > 0) {
      // We got errors!
      setErrors(newErrors)
    } else {
      userService.login(form['login'], form['password'])
        .then(jwt => {
          //props.login(jwt)
          history.push('/')
        }, e => alert(e));

      // props.login({
      //   login: form['login'],
      //   password: form['password']
      // })
    }
  }

  return (
    <div className="bg-light min-vh-100 d-flex flex-row align-items-center">
      <CContainer>
        <CRow className="justify-content-center">
          <CCol md={8}>
            <CCardGroup>
              <CCard className="p-4">
                <CCardBody>
                  <CForm>
                    <h1>Login</h1>
                    <p className="text-medium-emphasis">Sign In to your account</p>
                    <CInputGroup className="mb-3">
                      <CInputGroupText>
                        <CIcon icon={cilUser} />
                      </CInputGroupText>
                      <CFormInput placeholder="Username" autoComplete="username" onChange={e => setField('login', e.target.value)} invalid={!!errors.login} />
                    </CInputGroup>
                    <CInputGroup className="mb-4">
                      <CInputGroupText>
                        <CIcon icon={cilLockLocked} />
                      </CInputGroupText>
                      <CFormInput
                        type="password"
                        placeholder="Password"
                        autoComplete="current-password"
                        onChange={e => setField('password', e.target.value)}
                        invalid={!!errors.password}
                      />
                    </CInputGroup>
                    <CRow>
                      <CCol xs={6}>
                        <CButton color="primary" className="px-4" onClick={handleSubmit}>
                          Login
                        </CButton>
                      </CCol>
                      <CCol xs={6} className="text-right">
                        <CButton color="link" className="px-0">
                          Forgot password?
                        </CButton>
                      </CCol>
                    </CRow>
                  </CForm>
                </CCardBody>
              </CCard>
              <CCard className="text-white bg-primary py-5" style={{ width: '44%' }}>
                <CCardBody className="text-center">
                  <div>
                    <h2>Sign in</h2>
                  </div>
                </CCardBody>
              </CCard>
            </CCardGroup>
          </CCol>
        </CRow>
      </CContainer>
    </div>
  )
}

function matchDispatchToProps(dispatch) {
  return bindActionCreators({login: login}, dispatch)
}

export default connect(null, matchDispatchToProps)(Login)
