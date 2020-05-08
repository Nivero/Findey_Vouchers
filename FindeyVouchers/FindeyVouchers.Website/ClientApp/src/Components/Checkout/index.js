import React from 'react';
import {Container, Row} from 'reactstrap';
import {connect} from 'react-redux';
import './checkout.css';
import OrderOverView from "./OrderOverView";
import CheckoutForm from "./CheckoutForm";


const
    Checkout = (props) => {
        return (
            <Container className="p-3 bg-secondary my-2 rounded">
                <span className="text-center text-light"><h1>Your Order</h1></span>
                <Row>
                    <OrderOverView cartItems={props.cartItems} cartTotal={props.cartTotal}/>
                    <CheckoutForm/>
                </Row>
                <hr/>
                <span className="text-center text-light">
        <p>Bij het bestellen van de voucher(s), gaat u akkoord met onze <a href="">Voorwaarden & Privacy</a></p>
      </span>
            </Container>
        );
    };


const
    mapStateToProps = state => {
        return {cartTotal: state.cartTotal, cartItems: state.cartItems}
    }

export default connect(mapStateToProps)

(
    Checkout
)
;