import React from 'react';
import {Button, Card, CardBody, Col, Collapse, Row, Spinner, Toast, ToastBody, ToastHeader} from 'reactstrap';
import {
    CardCvcElement,
    CardExpiryElement,
    CardNumberElement,
    ElementsConsumer,
    IdealBankElement,
} from '@stripe/react-stripe-js';
import {ErrorResult, Result} from './util';
import './checkout.css';

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
            errorMessage: null,
            paymentMethod: null,
            isOpenIDEAL: false,
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
        this.setState({isOpenIDEAL: true, isOpenBank: false});

    };

    toggleBANK = () => {
        this.setState({isOpenIDEAL: false, isOpenBank: true});
    };

    createOrder = (data) => {
        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
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
        const intentData = {
            companyName: "Nivero",
            Amount: this.props.total * 100
        }
        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify(intentData)
        };

        fetch(`order/payment/intent`, requestOptions)
            .then((response) => {
                return response.json();
            })
            .then((responseJson) => {
                this.setState({secret: responseJson.client_secret})
            });
    }


    handleSubmit = async (event) => {
        event.preventDefault();

        const {firstname, lastname, email, phoneNumber, isOpenIDEAL, secret} = this.state;
        const {stripe, elements, cartItems} = this.props;
        if (!stripe || !elements) {
            return;
        }
        
        this.setState({paymentSent: true})
        
        this.createOrder({
            customer: {
                firstname: firstname,
                lastname: lastname,
                email: email,
                phoneNumber: phoneNumber
            },
            vouchers: cartItems,
            paymentId: secret.substr(0, secret.indexOf("_secret"))
        });

        if (isOpenIDEAL) {
            const {error} = stripe.confirmIdealPayment(secret, {
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
                console.log(error.message);
            }
        } else {
            const result = await stripe.confirmCardPayment(secret, {
                payment_method: {
                    card: elements.getElement(CardNumberElement),
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
                errorMessage: result.error ? result.error.message : ""
            }
            const requestOptions = {
                method: 'POST',
                headers: {'Content-Type': 'application/json'},
                body: JSON.stringify(data)
            };
            fetch(`order/payment/response`, requestOptions)
                .then(function (response) {
                    if (result.error) {
                        window.location.href = "/checkout-status/error";
                    } else {
                        if (result.paymentIntent.status === 'succeeded') {
                            window.location.href = "/checkout-status/success";
                        }
                    }
                });
        }
    }


    render() {
        const {stripe} = this.props
        const {paymentSent} = this.state;
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
                                        name="firstname"
                                        type="text"
                                        required
                                        placeholder="Uw voornaam"
                                        value={this.state.firstname}
                                        onChange={this.handleChange}
                                    />
                                </Col>
                                <Col sm="12" md="6" className="d-inline-block">
                                    <label htmlFor="lastname">Last Name</label>
                                    <input
                                        name="lastname"
                                        type="text"
                                        required
                                        placeholder="Uw achternaam"
                                        value={this.state.lastname}
                                        onChange={this.handleChange}
                                    />
                                </Col>
                                <Col sm="12" md="6" className="d-inline-block">
                                    <label htmlFor="name">Email</label>
                                    <input
                                        name="email"
                                        type="email"
                                        required
                                        placeholder="voorbeeld@hotmail.com"
                                        value={this.state.email}
                                        onChange={this.handleChange}
                                    />
                                </Col>
                                <Col sm="12" md="6" className="d-inline-block">
                                    <label htmlFor="name">Phone Number</label>
                                    <input
                                        name="phoneNumber"
                                        type="tel"
                                        required
                                        placeholder="012 123 1234"
                                        value={this.state.phoneNumber}
                                        onChange={this.handleChange}
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
                                            <div>
                                                <label htmlFor="ideal">iDEAL</label>
                                                <IdealBankElement
                                                    id="ideal"
                                                    options={ELEMENT_OPTIONS}
                                                />
                                            </div>
                                        </CardBody>
                                    </Card>
                                    <button className="mci-checkout-button" type="submit"
                                            disabled={!stripe}>
                                        Confirm & Pay € {this.props.total}
                                    </button>
                                </Collapse>
                                <Collapse isOpen={this.state.isOpenBank}>
                                    <Card>
                                        <CardBody>
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
                                        </CardBody>
                                    </Card>
                                    <button className="mci-checkout-button" type="submit"
                                            disabled={!stripe || paymentSent}>
                                        {!paymentSent ? <span>Confirm & Pay € {this.props.total}</span> :

                                            <Spinner class="p-1" animation="border" variant="primary" />}
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

export const InjectedCheckoutForm = (props) => {

    return (
        <ElementsConsumer>
            {({stripe, elements}) => (
                <CheckoutForm stripe={stripe} elements={elements} total={props.totalAmount}
                              cartItems={props.cartItems}/>
            )}
        </ElementsConsumer>
    );
}