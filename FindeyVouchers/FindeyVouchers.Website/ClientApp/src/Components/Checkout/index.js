import React from 'react';
import {Container, Row} from 'reactstrap';
import {connect} from 'react-redux';
import './checkout.css';
import {loadStripe} from '@stripe/stripe-js';
import OrderOverView from "./OrderOverView";
import {InjectedCheckoutForm} from "./CheckoutForm";
import { Elements } from "@stripe/react-stripe-js";
import ReactDOM from 'react-dom';
import App from './App';

const stripePromise = loadStripe('pk_live_EnmsulJpsuqfx7wpKTqWDlON00nbiPNe1s');
ReactDOM.render(
    <BrowserRouter>
        <App />
    </BrowserRouter>,
    document.getElementById('root')
);
const
    Checkout = (props) => {
        return (
            <Container className="p-3 bg-secondary my-2 rounded">
                <span className="text-center text-light"><h1>Uw Bestelling</h1></span>
                <Row>
                    <OrderOverView cartItems={props.cartItems} cartTotal={props.cartTotal}/>
                    <Elements stripe={stripePromise}>
                        <InjectedCheckoutForm totalAmount={props.cartTotal} cartItems={props.cartItems}/>
                    </Elements>

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
);