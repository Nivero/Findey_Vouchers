import React from 'react';
import {ElementsConsumer, IdealBankElement} from '@stripe/react-stripe-js';

import IdealBankSection from './idealBankSection/IdealBankSection';

class CheckoutForm extends React.Component {
    componentDidMount() {
        this.fetchClientSecret()
    }

    handleSubmit = async (event) => {
        // We don't want to let default form submission happen here,
        // which would refresh the page.
        event.preventDefault();

        const {stripe, elements} = this.props

        if (!stripe || !elements) {
            // Stripe.js has not yet loaded.
            // Make  sure to disable form submission until Stripe.js has loaded.
            return;
        }

        const idealBank = elements.getElement(IdealBankElement);

        // For brevity, this example is using uncontrolled components for
        // the accountholder's name. In a real world app you will
        // probably want to use controlled components.
        // https://reactjs.org/docs/uncontrolled-components.html
        // https://reactjs.org/docs/forms.html#controlled-components

        const accountholderName = "test";

        const {error} = await stripe.confirmIdealPayment(this.state.secret, {
            payment_method: {
                ideal: idealBank,
                billing_details: {
                    name: accountholderName,
                },
            },
            return_url: 'https://your-website.com/checkout/complete',
        });

        if (error) {
            // Show error to your customer.
            console.log(error.message);
        }

        // Otherwise the customer will be redirected away from your
        // page to complete the payment with their bank.
    };

    async fetchClientSecret() {
        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({company: 'nivero'})
        };
        const response = await fetch('payment/intent', requestOptions);
        const data = await response.json();
        this.setState({ secret: data.client_secret });
    }
    render() {
        const {stripe} = this.props;

        return (
            <form onSubmit={this.handleSubmit}>
                <div class="form-row">
                    <label>
                        Name
                        <input
                            value="test"
                            name="accountholder-name"
                            placeholder="Jenny Rosen"
                            required
                        />
                    </label>
                </div>
                <div class="form-row">
                    <IdealBankSection />
                </div>
                <button type="submit" disabled={!stripe}>
                    Submit Payment
                </button>
            </form>
        );
    }
}

export default function InjectedCheckoutForm() {
    return (
        <ElementsConsumer>
            {({stripe, elements}) => (
                <CheckoutForm  stripe={stripe} elements={elements} />
            )}
        </ElementsConsumer>
    );
}