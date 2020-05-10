import React from 'react';
import {Route, Switch} from 'react-router-dom';
import CompanyVouchers from './Components/CompanyVouchers';
import Checkout from './Components/Checkout';
import LoadingBar from 'react-top-loading-bar';
//vervangen door call in App()
//Tot hier
import {Provider} from 'react-redux';
import {createStore} from 'redux';
import reducer from './redux/reducer';
import CheckoutStatusSuccess from "./Components/CheckoutStatus/CheckoutStatusSuccess";
import CheckoutStatusError from "./Components/CheckoutStatus/CheckoutStatusError";


export default class App extends React.Component {
    constructor(props) {
        super(props);
        this.state = {response: {}, isLoaded: false, error: null};
    }

    componentDidMount() {
        this.fetchMerchant();
    }


    async fetchMerchant() {
        var location = window.location.host.split(".");
        var merchantName = location[location.indexOf("findey") - 1]
        console.log(merchantName);
        const requestOptions = {
            method: 'GET',
        };
        // Merchant name should be here.
        // It will be the first part of the url IE. nivero.findey.nl
        fetch(`merchant/nivero`, requestOptions)
            .then((response) => response.json())
            .then(
                (result) => {
                    this.setState({
                        isLoaded: true,
                        response: result
                    });
                },
                // Note: it's important to handle errors here
                // instead of a catch() block so that we don't swallow
                // exceptions from actual bugs in components.
                (error) => {
                    this.setState({
                        isLoaded: true,
                        error
                    });
                });
    }

    render() {
        const {isLoaded, response, error} = this.state;
        if (error) {
            return (
                <Switch>
                    <Route path="/checkout-status/success" component={CheckoutStatusSuccess}/>
                    <Route path="/checkout-status/error" component={CheckoutStatusError}/>
                </Switch>)
        } else if (!isLoaded) {
            return <div>Loading...</div>;
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
                        <Route path="/" component={() => <CompanyVouchers data={response}/>}/>
                        <Route path="/checkout" component={Checkout}/>
                        <Route path="/checkout-status/success" component={CheckoutStatusSuccess}/>
                        <Route path="/checkout-status/error" component={CheckoutStatusError}/>
                    </Switch>
                </Provider>
            );
        }
    }
}
