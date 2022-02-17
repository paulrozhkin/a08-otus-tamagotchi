import React from 'react';
import {Row, Col} from 'react-bootstrap';
import PaymentCard from '../common/PaymentCard';
import DeleteAddressModal from '../modals/DeleteAddressModal';

class Payments extends React.Component {
    constructor(props, context) {
        super(props, context);

        this.state = {
            showDeleteModal: false,
        };
    }

    hideDeleteModal = () => this.setState({showDeleteModal: false});

    render() {
        return (
            <>
                <DeleteAddressModal show={this.state.showDeleteModal} onHide={this.hideDeleteModal}/>
                <div className='p-4 bg-white shadow-sm'>
                    <Row>
                        <Col md={12}>
                            <h4 className="font-weight-bold mt-0 mb-3">Способы оплаты</h4>
                        </Col>
                        <Col md={6}>
                            <PaymentCard
                                title='6070-XXXXXXXX-0666'
                                logoImage='img/bank/1.png'
                                imageclassName='mr-3'
                                subTitle='VALID TILL 10/2025'
                                onClick={() => this.setState({showDeleteModal: true})}
                            />
                        </Col>
                    </Row>
                </div>
            </>
        );
    }
}

export default Payments;
