import React from 'react';
import {
    Col, Row,
    Toast,
    ToastHeader,
} from 'reactstrap';
import './checkout.css'
import ToastBody from "reactstrap/es/ToastBody";
import ListGroup from "reactstrap/es/ListGroup";
import ListGroupItem from "reactstrap/es/ListGroupItem";

export default class OrderOverview extends React.Component {
    constructor(props) {
        super(props);

    }

    render() {
        return (
            <Col sm="12" md="5" className="d-flex justify-content-center">
                <Toast className="mci-toast">
                    <ToastHeader>
                        1. Jouw bestelling
                    </ToastHeader>
                    <ToastBody>
                        <ListGroup>
                            {
                                this.props.cartItems.map(item => {
                                    if (item.amount > 0) {
                                        let totalItemPrice = item.price * item.amount;
                                        return <ListGroupItem key={item.id}>{item.amount} x {item.name}<span
                                            className="float-right font-weight-bold">€ {totalItemPrice}</span></ListGroupItem>
                                    }
                                })
                            }
                            <ListGroupItem className="fixed-bottom">Totaal: <span
                                className="float-right font-weight-bold">€ {this.props.cartTotal}</span></ListGroupItem>
                        </ListGroup>
                    </ToastBody>
                </Toast>
            </Col>
        );
    }
}
