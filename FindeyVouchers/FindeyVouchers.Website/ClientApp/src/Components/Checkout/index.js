import React from 'react';
import { Container, Row } from 'reactstrap';
import { connect } from 'react-redux';
import './checkout.css';
import { loadStripe } from '@stripe/stripe-js';
import OrderOverView from './OrderOverView';
import { InjectedCheckoutForm } from './CheckoutForm';
import { Elements } from '@stripe/react-stripe-js';
import TopHeader from '../../common/components/top-header/TopHeader';


const stripePromise = loadStripe('pk_live_EnmsulJpsuqfx7wpKTqWDlON00nbiPNe1s');
const
  Checkout = (props) => {
    return (
      <div>
        <TopHeader title={'JOUW ORDER'}/>

        <Container className="mt-4">

          <Elements stripe={stripePromise}>
            <InjectedCheckoutForm totalAmount={props.cartTotal} cartItems={props.cartItems}>
              <OrderOverView cartItems={props.cartItems} cartTotal={props.cartTotal}/>
            </InjectedCheckoutForm>
          </Elements>

          <hr/>

          <div className="text-center text-secondary">
            <p>Bij het bestellen van de voucher(s), gaat u akkoord met onze <a href="">Voorwaarden & Privacy</a></p>
          </div>
        </Container>
      </div>
    );
  };


const
  mapStateToProps = state => {
    return { cartTotal: state.cartTotal, cartItems: state.cartItems }
  }

export default connect(mapStateToProps)

(
  Checkout
)
;
