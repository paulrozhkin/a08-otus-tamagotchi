import React, {useState} from 'react';
import {Link, useHistory} from 'react-router-dom';
import {Row, Col, Container, Form, Button, FormGroup} from 'react-bootstrap';
import {accountService} from "../services/account-service";

function Register() {
	const history = useHistory();

    const [login, setLogin] = useState('')
    const [password, setPassword] = useState('')
    const [name, setName] = useState('')

    const [validated, setValidated] = useState(false)
    const [loading, setLoading] = useState(false)

    function handleSubmit(event) {
        event.preventDefault();
        event.stopPropagation();

        const form = event.currentTarget;
        if (form.checkValidity() === false) {
            setValidated(true);
        } else {
            setLoading(true)
            accountService.register(login, password, name)
                .then(newUser => {
					history.push('/login')
                }, e => {
                    setLoading(false);
                    alert(e.message)
                })
        }
    }


    return (
        <Container fluid className='bg-white'>
            <Row>
                <Col md={4} lg={6} className="d-none d-md-flex bg-image"/>
                <Col md={8} lg={6}>
                    <div className="login d-flex align-items-center py-5">
                        <Container>
                            <Row>
                                <Col md={9} lg={8} className="mx-auto pl-5 pr-5">
                                    <h3 className="login-heading mb-4">New Buddy!</h3>
                                    <Form noValidate validated={validated} onSubmit={handleSubmit}>
                                        <FormGroup className="form-label-group">
                                            <Form.Control type="email" id="inputEmail" placeholder="Email address"
                                                          value={login}
                                                          onChange={(event) => setLogin(event.target.value)}
                                                          required/>
                                            <Form.Label htmlFor="inputEmail">Email address</Form.Label>
                                        </FormGroup>
                                        <FormGroup className="form-label-group">
                                            <Form.Control type="password" id="inputPassword"
                                                          placeholder="Password"
                                                          value={password}
                                                          onChange={(event) => setPassword(event.target.value)}
                                                          required/>
                                            <Form.Label htmlFor="inputPassword">Password</Form.Label>
                                        </FormGroup>

                                        <FormGroup className="form-label-group">
                                            <Form.Control type="text" id="inputName" placeholder="Name"
                                                          value={name}
                                                          onChange={(event) => setName(event.target.value)}
                                                          required/>
                                            <Form.Label htmlFor="inputName">Name</Form.Label>
                                        </FormGroup>

                                        <Button type="submit"
                                                className="btn btn-lg btn-outline-primary btn-block btn-login text-uppercase font-weight-bold mb-2"
                                                disabled={loading}>
                                            Sign Up</Button>
                                        <div className="text-center pt-3">
                                            Already have an account? <Link className="font-weight-bold" to="/login">Sign
                                            In</Link>
                                        </div>
                                    </Form>
                                </Col>
                            </Row>
                        </Container>
                    </div>
                </Col>
            </Row>
        </Container>
    );
}


export default Register;
