import React from 'react';
import {Route, Switch} from 'react-router-dom';
import CompanyVouchers from './Components/CompanyVouchers';
import Checkout from './Components/Checkout';
import {Provider} from 'react-redux';
import {createStore} from 'redux';
import reducer from './redux/reducer';
import CheckoutStatusSuccess from "./Components/CheckoutStatus/CheckoutStatusSuccess";
import CheckoutStatusError from "./Components/CheckoutStatus/CheckoutStatusError";
import CheckoutStatusPending from "./Components/CheckoutStatus/CheckoutStatusPending";
import Legal from "./Components/Legal/";
import MerchantNotFound from "./Components/NotFound";


export default class App extends React.Component {
    constructor(props) {
        super(props);
        this.state = {response: {}, isLoaded: false, error: null};
    }

    componentDidMount() {
        this.fetchMerchant();
    }


    async fetchMerchant() {
        let completeUrl = window.location.host.split(".");
        let merchantName = completeUrl[completeUrl.indexOf("findey") - 1]
        const requestOptions = {
            method: 'GET',
        };
        // Merchant name should be here.
        // It will be the first part of the url IE. nivero.findey.nl
        fetch(`merchant/nivero`, requestOptions)
            .then((response) => {
                if (response.ok) {
                    return response.json()
                } else {
                    return Promise.reject('merchant not found')
                }
            })
            .then(
                (response) => {
                    this.setState({
                        isLoaded: true,
                        response: response
                    });

                })
            .catch((error) => {
                this.setState({
                    isLoaded: true,
                    error: true
                });
            });
    }

    render() {
        const {isLoaded, response, error} = this.state;
        if (!isLoaded) {
            return <div>Loading...</div>;
        } else {
            if (error) {
                return (
                    <Switch>
                        <Route exact path="/" component={MerchantNotFound}/>
                    </Switch>
                );
            } else {
                const initialStore = {
                    cartItems: response.vouchers,
                    cartTotal: 0,
                    cartAmount: 0
                };

                const store = createStore(reducer, initialStore);
                return (

                    <Provider store={store}>
                        <Switch>
                            <Route exact path="/" component={() => <CompanyVouchers data={response}/>}/>
                            <Route exact path="/checkout" component={Checkout}/>
                            <Route exact path="/checkout-status/success" component={CheckoutStatusSuccess}/>
                            <Route exact path="/checkout-status/error" component={CheckoutStatusError}/>
                            <Route exact path="/checkout-status/pending" component={CheckoutStatusPending}/>
                            <Route exact path="/legal" component={Legal}/>
                        </Switch>
                    </Provider>
                );
            }

        }
    }
}
