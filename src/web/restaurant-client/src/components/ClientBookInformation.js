import {connect} from "react-redux";
import {Button, Col, Form, Row} from "react-bootstrap";
import React, {useEffect, useState} from "react";
import {updateMenu} from "../actions/update-menu";
import {setClientInfo} from "../actions/set-client-info";

function ClientBookInformation(props) {
    const [validated, setValidated] = useState(false)
    const [numberOfPersons, setNumberOfPersons] = useState(1)
    const [visitTime, setVisitTime] = useState('')
    const [comment, setComment] = useState('')

    const [minDate, setMinDate] = useState('')
    const [maxDate, setMaxDate] = useState('')

    useEffect(() => {
        const now = new Date();
        now.setSeconds(0, 0);
        const minTime = now.toISOString().replace(/:00.000Z/, "")
        const next = new Date()
        next.setDate(now.getDate() + 14)
        next.setSeconds(0, 0);
        const maxTime = next.toISOString().replace(/:00.000Z/, "")

        setMinDate(minTime)
        setMaxDate(maxTime)
    }, [])

    const handleSubmit = (event) => {
        const form = event.currentTarget;
        event.preventDefault();
        event.stopPropagation();

        setValidated(true);

        if (form.checkValidity()) {
            const visitTimeDate = new Date(visitTime)

            const clientInfo = {
                numberOfPersons,
                visitTime: visitTimeDate.toISOString(),
                comment
            }

            props.setClientInfo(clientInfo)
        }
    };

    return (
        <div id="book-a-table"
             className="bg-white rounded shadow-sm p-4 mb-5 rating-review-select-page">
            <h5 className="mb-4">Информация о бронировании</h5>
            <Form noValidate validated={validated} onSubmit={handleSubmit} >
                <Row>
                    <Col sm={6}>
                        <Form.Group>
                            <Form.Label>Количество посетителей</Form.Label>
                            <Form.Control type="number"
                                          value={numberOfPersons}
                                          onChange={event => setNumberOfPersons(event.target.value)}
                                          placeholder="Кол-во" required />
                        </Form.Group>
                    </Col>
                    <Col sm={6}>
                        <Form.Group>
                            <Form.Label>Дата посещения</Form.Label>
                            <Form.Control type="datetime-local"
                                          value={visitTime}
                                          onChange={event => setVisitTime(event.target.value)}
                                          min={minDate}
                                          max={maxDate}
                                          required />
                        </Form.Group>
                    </Col>
                </Row>
                <Row>
                    <Col sm={12}>
                        <Form.Group>
                            <Form.Label>Пожелания</Form.Label>
                            <Form.Control as="textarea"
                                          row={3}
                                          value={comment}
                                          onChange={event => setComment(event.target.value)}
                                          placeholder="Пожелания"/>
                        </Form.Group>
                    </Col>
                </Row>
                <Form.Group className="text-right">
                    <Button variant="primary" type="submit"> Submit </Button>
                </Form.Group>
            </Form>
        </div>
    )
}

function mapStateToProps(state) {
    const order = state.order
    return {
        numberOfPersons: order.numberOfPersons,
        visitTime: order.visitTime,
        comment: order.comment,
    }
}

function mapDispatchToProps(dispatch) {
    return {
        setClientInfo: (bookInfo) => dispatch(setClientInfo(bookInfo))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(ClientBookInformation)

