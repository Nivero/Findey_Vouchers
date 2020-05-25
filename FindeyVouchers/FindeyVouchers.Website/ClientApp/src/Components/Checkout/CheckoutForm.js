import React from 'react';
import { Button, Card, CardBody, Col, Row, Spinner, Nav, NavItem, NavLink, FormGroup } from 'reactstrap';
import { AvForm, AvField } from 'availity-reactstrap-validation';
import {
  ElementsConsumer,
  IdealBankElement,
  CardElement,
} from '@stripe/react-stripe-js';
import { PHONE_REGEX } from '../../common/regex.constants';
import { ErrorResult, Result } from './util';
import './checkout.css';

const IDEAL_ELEMENT_OPTIONS = {
  // Custom styling can be passed to options when creating an Element
  style: {
    base: {
      padding: '5px 6px',
      color: '#32325d',
      fontSize: '16px',
      '::placeholder': {
        color: '#aab7c4'
      },
    },
  },
};
const CARD_ELEMENT_OPTIONS = {
  style: {
    base: {
      color: '#303238',
      fontSize: '16px',
      fontFamily: 'sans-serif',
      fontSmoothing: 'antialiased',
      '::placeholder': {
        color: '#CFD7DF'
      }
    },
    invalid: {
      color: '#e5424d',
      ':focus': {
        color: '#303238'
      }
    }
  },
  hidePostalCode: true
};

class CheckoutForm extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      firstname: '',
      lastname: '',
      email: '',
      phoneNumber: '',
      errorMessage: null,
      paymentMethod: null,
      isOpenIDEAL: true,
      isOpenBANK: false,
      secret: '',
      paymentSent: false,
    };
    this.handleChange = this.handleChange.bind(this);
    this.setClientSecret();
  }


  handleChange(event) {
    this.setState({
      [event.target.name]: event.target.value
    })
  }

  toggleIDEAL = () => {
    this.setState({ isOpenIDEAL: true, isOpenBank: false });

  };

  toggleBANK = () => {
    this.setState({ isOpenIDEAL: false, isOpenBank: true });
  };

  createOrder = (data) => {
    const requestOptions = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data)
    };
    fetch(`order/create`, requestOptions)
      .then((response) => {
        return response;
      }).then((responseJson) => {
      return responseJson;
    });
  }

  setClientSecret = () => {
    let location = window.location.host.split('.');
    let merchantName = location[location.indexOf('findey') - 1]
    const intentData = {
      companyName: merchantName,
      Amount: this.props.total * 100
    }
    const requestOptions = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(intentData)
    };

    fetch(`order/payment/intent`, requestOptions)
      .then((response) => {
        return response.json();
      })
      .then((responseJson) => {
        this.setState({ secret: responseJson.client_secret })
      });
  }


  handleSubmit = async (event) => {
    event.preventDefault();

    const { firstname, lastname, email, phoneNumber, isOpenIDEAL, secret } = this.state;
    const { stripe, elements, cartItems } = this.props;
    if (!stripe || !elements) {
      return;
    }

    this.setState({ paymentSent: true })

    this.createOrder({
      customer: {
        firstname: firstname,
        lastname: lastname,
        email: email,
        phoneNumber: phoneNumber
      },
      vouchers: cartItems,
      paymentId: secret.substr(0, secret.indexOf('_secret'))
    });

    if (isOpenIDEAL) {
      const { error } = stripe.confirmIdealPayment(secret, {
        payment_method: {
          ideal: elements.getElement(IdealBankElement),
          billing_details: {
            name: lastname,
          },
        },
        return_url: window.location.origin + '/checkout-status/pending',
      });

      if (error) {
        // Show error to your customer.
        console.log('error', error.message);
      }
    } else {
      const result = await stripe.confirmCardPayment(secret, {
        payment_method: {
          card: elements.getElement(CardElement),
          billing_details: {
            name: lastname,
          },
        }
      });

      const data = {
        paymentStatus: result.paymentIntent.status,
        amount: result.paymentIntent.amount,
        created: result.paymentIntent.created,
        paymentId: result.paymentIntent.id,
        errorMessage: result.error ? result.error.message : ''
      }
      const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
      };
      fetch(`order/payment/response`, requestOptions)
        .then(function (response) {
          if (result.error) {
            window.location.href = '/checkout-status/error';
          } else {
            if (result.paymentIntent.status === 'succeeded') {
              window.location.href = '/checkout-status/success';
            }
          }
        });
    }
  }

  render() {
    const { stripe, children } = this.props
    const { paymentSent } = this.state;

    return (
      <div>
        <Row>
          <Col sm="12" md="6" className="mb-sm-3">{children}</Col>
          <Col sm="12" md="6">
            <AvForm
              id="checkoutForm"
              onValidSubmit={this.handleSubmit} className="">
              <h4>2. Jouw informatie</h4>
              <Card body className="bg-light p-4">
                <Row className="mb-4">
                  <Col sm="12" md="6">
                    <FormGroup>
                    <AvField
                      name="firstname"
                      type="text"
                      required
                      placeholder="Uw voornaam"
                      value={this.state.firstname}
                      onChange={this.handleChange}
                    />
                    </FormGroup>
                  </Col>
                  <Col sm="12" md="6">
                    <FormGroup>
                    <AvField
                      name="lastname"
                      type="text"
                      required
                      placeholder="Uw achternaam"
                      value={this.state.lastname}
                      onChange={this.handleChange}
                    />
                    </FormGroup>
                  </Col>
                </Row>
                <Row>
                  <Col sm="12" md="6">
                    <FormGroup>
                    <AvField
                      name="email"
                      type="email"
                      required
                      placeholder="voorbeeld@hotmail.com"
                      value={this.state.email}
                      onChange={this.handleChange}
                    />
                    </FormGroup>
                  </Col>
                  <Col sm="12" md="6">
                    <FormGroup>
                    <AvField
                      name="phoneNumber"
                      type="text"
                      required
                      validate={{ pattern: { value: PHONE_REGEX } }}
                      placeholder="012 123 1234"
                      value={this.state.phoneNumber}
                      onChange={this.handleChange}
                    />
                    </FormGroup>
                  </Col>

                </Row>
              </Card>

              <div className="mt-4">
                <h4>3. Kies een betalingsmethode</h4>
                <Card className="bg-light ">
                  <Nav style={{ backgroundColor: 'white' }} tabs>
                    <NavItem style={{ width: '50%' }}>
                      <NavLink

                        className={this.state.isOpenIDEAL ? 'active-tab' : ''}
                        onClick={() => this.toggleIDEAL()}
                      >
                        <div className="custom-control custom-radio">
                          <div className="center">
                            <input className="custom-control-input"
                                   type="radio"
                                   name="paymenttype"
                                   id="ideal"
                                   value="ideal"
                                   checked={this.state.isOpenIDEAL}
                            />
                            <label className="custom-control-label" htmlFor="ideal">
                              iDEAL</label>
                          </div>
                        </div>
                      </NavLink>
                    </NavItem>

                    <NavItem style={{ width: '50%' }}>
                      <NavLink
                        className={this.state.isOpenBank ? 'active-tab' : ''}
                        onClick={() => this.toggleBANK()}
                      >
                        <div className="custom-control custom-radio ccLine">
                          <div className="center">
                            <input className="custom-control-input btn-danger"
                                   type="radio"
                                   name="paymenttype"
                                   id="creditcard"
                                   value="creditcard"
                                   checked={this.state.isOpenBank}
                            />
                            <label className="custom-control-label" htmlFor="creditcard">
                              Credit Card</label>
                          </div>
                        </div>
                      </NavLink>
                    </NavItem>
                  </Nav>

                  <CardBody>
                    {this.state.isOpenIDEAL && (
                      <div>
                        <IdealBankElement
                          id="ideal"
                          options={IDEAL_ELEMENT_OPTIONS}
                        />
                      </div>
                    )}

                    {this.state.isOpenBank && (
                      <div>
                        <CardElement options={CARD_ELEMENT_OPTIONS}/>
                        {this.state.errorMessage &&
                        <ErrorResult>{this.state.errorMessage}</ErrorResult>}
                        {this.state.paymentMethod && (
                          <Result>Got PaymentMethod: {this.state.paymentMethod.id}</Result>
                        )}
                      </div>
                    )}
                  </CardBody>

                </Card>
              </div>
            </AvForm>
          </Col>
        </Row>

        <hr/>

        <div className="m-5 text-center">
          <Button className=" mci-checkout-button"
                  form="checkoutForm"
                  type="submit"
                  size="lg"
                  disabled={!stripe || paymentSent || this.props.total === 0}>
            {!paymentSent ? <span>Bevestig & Betaal € {this.props.total}</span> :

              <Spinner class="p-1" animation="border" variant="primary"/>}
          </Button>
        </div>


      </div>


    );
  }
}

export const InjectedCheckoutForm = (props) => {

  return (
    <ElementsConsumer>
      {({ stripe, elements }) => (
        <CheckoutForm stripe={stripe} elements={elements} total={props.totalAmount}
                      cartItems={props.cartItems}>{props.children}</CheckoutForm>
      )}
    </ElementsConsumer>
  );
}
