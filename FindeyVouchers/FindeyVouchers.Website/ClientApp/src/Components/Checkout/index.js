import React, { useState } from 'react';
import { Container, Row, Col, Toast, ToastHeader, ToastBody, ListGroup, ListGroupItem, Button, Collapse, Card, CardBody } from 'reactstrap';
import { connect } from 'react-redux';
import {loadStripe} from '@stripe/stripe-js';
import {
  CardNumberElement,
  CardCvcElement,
  CardExpiryElement,
  Elements,
  ElementsConsumer,IdealBankElement
} from '@stripe/react-stripe-js';

import {logEvent, Result, ErrorResult} from './util';
import './checkout.css';

let total = 0;

const ELEMENT_OPTIONS = {
  style: {
    base: {
      fontSize: '18px',
      color: '#424770',
      letterSpacing: '0.025em',
      '::placeholder': {
        color: '#aab7c4',
      },
    },
    invalid: {
      color: '#9e2146',
    },
  },
};

class CheckoutForm extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      firstname: '',
      lastname: '',
      email: '',
      phoneNumber: '',
      postal: '',
      errorMessage: null,
      paymentMethod: null,
    };
  }

  handleSubmit = async (event) => {
    event.preventDefault();
    const {stripe, elements} = this.props;
    const {name, postal} = this.state;

    if (!stripe || !elements) {
      // Stripe.js has not loaded yet. Make sure to disable
      // form submission until Stripe.js has loaded.
      return;
    }

    const cardElement = elements.getElement(CardNumberElement);

    const payload = await stripe.createPaymentMethod({
      type: 'card',
      card: cardElement,
      billing_details: {
        name,
        address: {
          postal_code: postal,
        },
      },
    });

    if (payload.error) {
      this.setState({
        errorMessage: payload.error.message,
        paymentMethod: null,
      });
    } else {
      this.setState({
        paymentMethod: payload.paymentMethod,
        errorMessage: null,
      });
    }
  };

  render() {
    const {stripe} = this.props;
    const {postal, name, paymentMethod, errorMessage} = this.state;

    return (
      <form onSubmit={this.handleSubmit}>
        { 
        this.props.ideal.ideal ?
        <div>
          <label htmlFor="ideal">iDEAL Bank</label>
          <IdealBankElement
            id="ideal"
            onBlur={logEvent('blur')}
            onChange={logEvent('change')}
            onFocus={logEvent('focus')}
            onReady={logEvent('ready')}
            options={ELEMENT_OPTIONS}
          />
        </div>
        :
        <div>
          <label htmlFor="cardNumber">Card Number</label>
          <CardNumberElement
            id="cardNumber"
            onBlur={logEvent('blur')}
            onChange={logEvent('change')}
            onFocus={logEvent('focus')}
            onReady={logEvent('ready')}
            options={ELEMENT_OPTIONS}
          />
          <label htmlFor="expiry">Card Expiration</label>
          <CardExpiryElement
            id="expiry"
            onBlur={logEvent('blur')}
            onChange={logEvent('change')}
            onFocus={logEvent('focus')}
            onReady={logEvent('ready')}
            options={ELEMENT_OPTIONS}
          />
          <label htmlFor="cvc">CVC</label>
          <CardCvcElement
            id="cvc"
            onBlur={logEvent('blur')}
            onChange={logEvent('change')}
            onFocus={logEvent('focus')}
            onReady={logEvent('ready')}
            options={ELEMENT_OPTIONS}
          />
          <label htmlFor="postal">Postal Code</label>
          <input
            id="postal"
            required
            placeholder="12345"
            value={postal}
            onChange={(event) => {
              this.setState({postal: event.target.value});
            }}
          />
          {errorMessage && <ErrorResult>{errorMessage}</ErrorResult>}
          {paymentMethod && (
            <Result>Got PaymentMethod: {paymentMethod.id}</Result>
          )}
          <br />        
        </div>
        }
        <button className="mci-checkout-button" type="submit" disabled={!stripe}>
          Confirm & Pay € {total}
        </button>
      </form>
    );
  }
}

const InjectedCheckoutForm = (ideal) => (
  <ElementsConsumer>
    {({stripe, elements}) => (
      <CheckoutForm stripe={stripe} elements={elements} ideal={ideal}/>
    )}
  </ElementsConsumer>
);  

// Make sure to call `loadStripe` outside of a component’s render to avoid
// recreating the `Stripe` object on every render.
const stripePromise = loadStripe('pk_test_t9hsTVk7AWp7pZI7UmZKKQ7r00PyLH5QmB');

const Checkout = (props) => {
  const [isOpenIDEAL, setIsOpenIDEAL] = useState(false);
  const toggleIDEAL = () => { setIsOpenIDEAL(!isOpenIDEAL); setIsOpenBANK(isOpenIDEAL);};
  const [isOpenBANK, setIsOpenBANK] = useState(false);
  const toggleBANK = () => { setIsOpenBANK(!isOpenBANK); setIsOpenIDEAL(isOpenBANK);};

  total = props.cartTotal;
  console.log(props.cartItems)
  return (
    <Container className="p-3 bg-secondary my-2 rounded">
      <span className="text-center text-light"><h1>Your Order</h1></span>
      <Row>
        <Col sm="12" md="5" className="d-flex justify-content-center">
          <Toast className="mci-toast">
            <ToastHeader>
              1. Your vouchers
            </ToastHeader>
            <ToastBody>
            <ListGroup>
              {
                props.cartItems.map(item => {
                  if(item.amount > 0){
                    let totalItemPrice = item.price * item.amount;
                    return <ListGroupItem key={item.id}>{item.amount} x {item.name}<span className="float-right font-weight-bold">€ {totalItemPrice}</span></ListGroupItem>
                  }
                })  
              }
              <ListGroupItem className="fixed-bottom">Totaal: <span className="float-right font-weight-bold">€ {props.cartTotal}</span></ListGroupItem>
            </ListGroup>
            </ToastBody>
          </Toast>
        </Col>
        <Col sm="12" md="7" className="justify-content-center">
          <Row>
            <Toast className="mci-toast">
              <ToastHeader>
                2. Your information
              </ToastHeader>
              <ToastBody>
              <Col sm="12" md="6" className="d-inline-block">
              <label htmlFor="firstname">First Name</label>
                <input
                  id="firstname"
                  required
                  placeholder="Jenny Rosen"
                />
                </Col>
                <Col sm="12" md="6" className="d-inline-block">
                <label htmlFor="lastname">Last Name</label>
                <input
                  id="lastname"
                  required
                  placeholder="Rosen"
                />
                </Col>
                <Col sm="12" md="6" className="d-inline-block">
                <label htmlFor="name">Email</label>
                <input
                  id="email"
                  required
                  placeholder="jenny.rosen@outlook.com"
                />
                </Col>
                <Col sm="12" md="6" className="d-inline-block">
                  <label htmlFor="name">Phone Number</label>
                  <input
                    id="phoneNumber"
                    required
                    placeholder="012 123 1234"
                  />
                </Col>
              </ToastBody>
            </Toast>
          </Row>
          <Row>
            <Toast className="mci-toast">
              <ToastHeader>
                3. Choose a Payment Method
              </ToastHeader>
              <ToastBody>
              <Col sm="12" md="6" className="d-inline-block">
              <Button className="mci-checkout-button" color="primary" onClick={toggleIDEAL} style={{ marginBottom: '1rem' }}>IDEAL</Button>
              </Col>
              <Col sm="12" md="6" className="d-inline-block">
              <Button className="mci-checkout-button" color="primary" onClick={toggleBANK} style={{ marginBottom: '1rem' }}>BANK</Button>
              </Col>
              <Collapse isOpen={isOpenIDEAL}>
                <Card>
                  <CardBody>
                  <Elements stripe={stripePromise}>
                      <InjectedCheckoutForm cartItems={props.cartItems} cartTotal={props.cartTotal} ideal={true}/>
                    </Elements>
                  </CardBody>
                </Card>
              </Collapse>
              <Collapse isOpen={isOpenBANK}>
                <Card>
                  <CardBody>
                    <Elements stripe={stripePromise}>
                      <InjectedCheckoutForm cartItems={props.cartItems} cartTotal={props.cartTotal} ideal={false}/>
                    </Elements>
                  </CardBody>
                </Card>
              </Collapse>
              </ToastBody>
            </Toast>
          </Row>
        </Col>
      </Row>
      <hr />
      <span className="text-center text-light">
        <p>Bij het bestellen van de voucher(s), gaat u akkoord met onze <a href="">Voorwaarden & Privacy</a></p>
      </span>
      </Container >
  );
};


const mapStateToProps = state => {
  return { cartTotal: state.cartTotal, cartItems: state.cartItems }
}

export default connect(mapStateToProps)(Checkout);