import React, {Component} from 'react';
import {Elements} from "@stripe/react-stripe-js";
import {loadStripe} from '@stripe/stripe-js';

import CheckoutForm from './CheckoutForm';

export class Home extends Component {
    stripePromise = loadStripe("pk_test_t9hsTVk7AWp7pZI7UmZKKQ7r00PyLH5QmB");
    constructor(props) {
        super(props);
        this.state = {
            secret: "",
        };
    }

    componentDidMount() {
    }
    render() {


        return (
            <div>
                <h1>Hello world!</h1>
                <Elements stripe={this.stripePromise}>
                    <CheckoutForm/>
                </Elements>
            </div>
        );
    }


}
