import React from 'react';
import {Button, Card, CardBody, Col, Collapse, Row, Toast, ToastBody, ToastHeader} from 'reactstrap';
import {loadStripe} from '@stripe/stripe-js';
import {
    CardCvcElement,
    CardExpiryElement,
    CardNumberElement,
    Elements,
    IdealBankElement
} from '@stripe/react-stripe-js';

import {ErrorResult, Result} from './util';
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

export default class CheckoutForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            firstname: '',
            lastname: '',
            email: '',
            phoneNumber: '',
            errorMessage: null,
            paymentMethod: null,
            isOpenIDEAL: false,
            isOpenBANK: false,
            stripePromise: loadStripe('pk_test_t9hsTVk7AWp7pZI7UmZKKQ7r00PyLH5QmB')
        };
    }

    toggleIDEAL = () => {
        this.setState({isOpenIDEAL: true, isOpenBank: false});

    };

    toggleBANK = () => {
        this.setState({isOpenIDEAL: false, isOpenBank: true});
    };


    handleSubmit = async (event) => {
        event.preventDefault();
        const {elements, ideal, cartTotal} = this.props;
        const {name, stripePromise} = this.state;

        if (!stripePromise || !elements) {
            return;
        }
        const data = {
            companyName: "Nivero",
            Amount: total * 100
        }
        console.log(JSON.stringify(data));
        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify(data)
        };

        var response = await fetch(`payment/intent`, requestOptions)
            .then(function (response) {
                return response.json();
            })
            .then(function (responseJson) {
                var clientSecret = responseJson.client_secret;
                if (ideal.ideal) {
                    const {error} = stripePromise.confirmIdealPayment(clientSecret, {
                        payment_method: {
                            ideal: elements.getElement(IdealBankElement),
                            billing_details: {
                                name: name,
                            },
                        },
                        return_url: 'https://google.com',
                    });

                    if (error) {
                        // Show error to your customer.
                        console.log(error.message);
                    }
                } else {
                    const result = stripePromise.confirmCardPayment(clientSecret, {
                        payment_method: {
                            card: elements.getElement(CardNumberElement),
                            billing_details: {
                                name: name,
                            },
                        }
                    }).then(function (response) {
                        return response;
                    }).then(function (result) {
                        if (result.error) {
                            // Show error to your customer (e.g., insufficient funds)
                            console.log(result.error.message);
                        } else {
                            // The payment has been processed!
                            if (result.paymentIntent.status === 'succeeded') {
                                console.log(result);
                                // Show a success message to your customer
                                // There's a risk of the customer closing the window before callback
                                // execution. Set up a webhook or plugin to listen for the
                                // payment_intent.succeeded event that handles any business critical
                                // post-payment actions.
                            }
                        }
                    });
                }

            });
    };

    render() {
        return (

            <Col sm="12" md="7" className="justify-content-center">
                <form onSubmit={this.handleSubmit} className="m-0 p-0">
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
                                        placeholder="Uw voornaam"
                                    />
                                </Col>
                                <Col sm="12" md="6" className="d-inline-block">
                                    <label htmlFor="lastname">Last Name</label>
                                    <input
                                        id="lastname"
                                        required
                                        placeholder="Uw achternaam"
                                    />
                                </Col>
                                <Col sm="12" md="6" className="d-inline-block">
                                    <label htmlFor="name">Email</label>
                                    <input
                                        id="email"
                                        required
                                        placeholder="example@hotmail.com"
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
                                    <Button className="mci-checkout-button" color="primary" onClick={this.toggleIDEAL}
                                            style={{marginBottom: '1rem'}}>IDEAL</Button>
                                </Col>
                                <Col sm="12" md="6" className="d-inline-block">
                                    <Button className="mci-checkout-button" color="primary" onClick={this.toggleBANK}
                                            style={{marginBottom: '1rem'}}>Creditcard</Button>
                                </Col>
                                <Collapse isOpen={this.state.isOpenIDEAL}>
                                    <Card>
                                        <CardBody>
                                            <Elements stripe={this.state.stripePromise}>
                                                <div>
                                                    <label htmlFor="ideal">iDEAL</label>
                                                    <IdealBankElement
                                                        id="ideal"
                                                        options={ELEMENT_OPTIONS}
                                                    />
                                                </div>
                                            </Elements>
                                        </CardBody>
                                    </Card>
                                    <button className="mci-checkout-button" type="submit"
                                            disabled={!this.state.stripePromise}>
                                        Confirm & Pay € {this.props.total}
                                    </button>
                                </Collapse>
                                <Collapse isOpen={this.state.isOpenBank}>
                                    <Card>
                                        <CardBody>
                                            <Elements stripe={this.state.stripePromise}>
                                                <div>
                                                    <label htmlFor="cardNumber">Card Number</label>
                                                    <CardNumberElement
                                                        id="cardNumber"
                                                        options={ELEMENT_OPTIONS}
                                                    />
                                                    <label htmlFor="expiry">Card Expiration</label>
                                                    <CardExpiryElement
                                                        id="expiry"
                                                        options={ELEMENT_OPTIONS}
                                                    />
                                                    <label htmlFor="cvc">CVC</label>
                                                    <CardCvcElement
                                                        id="cvc"
                                                        options={ELEMENT_OPTIONS}
                                                    />
                                                    {this.state.errorMessage &&
                                                    <ErrorResult>{this.state.errorMessage}</ErrorResult>}
                                                    {this.state.paymentMethod && (
                                                        <Result>Got
                                                            PaymentMethod: {this.state.paymentMethod.id}</Result>
                                                    )}
                                                    <br/>
                                                </div>
                                            </Elements>
                                        </CardBody>
                                    </Card>
                                    <button className="mci-checkout-button" type="submit"
                                            disabled={!this.state.stripePromise}>
                                        Confirm & Pay € {this.props.total}
                                    </button>
                                </Collapse>
                            </ToastBody>
                        </Toast>
                    </Row>
                </form>
            </Col>

    );
    }
    }