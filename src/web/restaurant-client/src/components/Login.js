import React, {useState} from 'react';
import {Link} from 'react-router-dom';
import {Row, Col, Container, Form, Button, FormGroup} from 'react-bootstrap';
import {accountService} from "../services/account-service";

function Login(props) {
    const [login, setLogin] = useState('')
    const [password, setPassword] = useState('')
    const [validated, setValidated] = useState(false)
    const [loading, setLoading] = useState(false)
    const [credentialsError, setCredentialsError] = useState(false)

    function handleSubmit(event) {
        event.preventDefault();
        event.stopPropagation();

        const form = event.currentTarget;
        if (form.checkValidity() === false) {
            setValidated(true);
        } else {
            setLoading(true)
            accountService.login(login, password)
                .then(r => {
                }, e => {
                    setLoading(false);
                    if (e.status === 400) {
                        setCredentialsError(true)
                    } else {
                        alert(e.message)
                    }
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
                                    <h3 className="login-heading mb-4">Welcome back!</h3>
                                    <Form noValidate validated={validated} onSubmit={handleSubmit}>
                                        <FormGroup className="form-label-group">
                                            <Form.Control type="email" id="inputEmail" placeholder="Email address"
                                                          value={login}
                                                          onChange={(event) => setLogin(event.target.value)}
                                                          required
                                                          isValid={validated && !credentialsError}
                                                          isInvalid={credentialsError}/>
                                            <Form.Label htmlFor="inputEmail">Email address</Form.Label>
                                            {!credentialsError && <Form.Control.Feedback type="invalid">
                                                Should be with '@'.
                                            </Form.Control.Feedback>}
                                            {credentialsError && <Form.Control.Feedback type="invalid">
                                                Email or password invalid.
                                            </Form.Control.Feedback>}
                                        </FormGroup>
                                        <FormGroup className="form-label-group">
                                            <Form.Control type="password" id="inputPassword" placeholder="Password"
                                                          value={password}
                                                          onChange={(event) => setPassword(event.target.value)}
                                                          required
                                                          isValid={validated && !credentialsError}
                                                          isInvalid={credentialsError}/>
                                            <Form.Label htmlFor="inputPassword">Password</Form.Label>
                                        </FormGroup>

                                        <Button type="submit"
                                                className="btn btn-lg btn-outline-primary btn-block btn-login text-uppercase font-weight-bold mb-2 mt-4"
                                                disabled={loading}>
                                            Sign in</Button>
                                        <div className="text-center pt-3">
                                            Don’t have an account? <Link className="font-weight-bold" to="/register">Sign
                                            Up</Link>
                                        </div>
                                        <hr className="my-4"/>
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


export default Login;
