import React from 'react';
import {Route, Link} from 'react-router-dom';
import CompanyVouchers from './Components/CompanyVouchers';
import Checkout from './Components/Checkout';
import CheckoutStatus from './Components/CheckoutStatus';
import LoadingBar from 'react-top-loading-bar';
//vervangen door call in App()
import data from './response.json';
//Tot hier
import {Provider} from 'react-redux';
import {createStore} from 'redux';
import reducer from './redux/reducer';
import CheckoutStatusSuccess from "./Components/CheckoutStatus/CheckoutStatusSuccess";
import CheckoutStatusError from "./Components/CheckoutStatus/CheckoutStatusError";


const initialStore = {
    cartItems: data.vouchers,
    cartTotal: 0,
    cartAmount: 0
};


const store = createStore(reducer, initialStore);

export default class App extends React.Component {
    constructor(props) {
        super(props);
        this.state = {response: {}, loadingBarProgress: 15};
    }

    componentDidMount() {
        this.setState({loadingBarProgress: 40})
        this.fetchMerchant();
        this.setState({loadingBarProgress: 80})
    }

    async fetchMerchant() {
        var location = window.location.host.split(".");
        var merchantName = location[location.indexOf("findey")-1]
        console.log(merchantName);
        const requestOptions = {
            method: 'GET',
        };
        // Merchant name should be here.
        // It will be the first part of the url IE. nivero.findey.nl
        fetch(`merchant/nivero`, requestOptions)
            .then((response) => response.json())
            .then((response) => {
                this.setState({
                    response: response,
                    loadingBarProgress: 100
                });
            });
    }

    render() {
        if (this.state.loadingBarProgress < 99) {
           return( <LoadingBar
               progress={this.state.loadingBarProgress}
               height={3}
               color='red'
            />)
        }
        const {response} = this.state;
        return (
            response ?
                <Provider store={store}>
                    <Route exact path="/" component={() => <CompanyVouchers data={response}/>}/>
                    <Route exact path="/checkout" component={Checkout}/>
                    <Route exact path="/checkout-status/success" component={CheckoutStatusSuccess}/>
                    <Route exact path="/checkout-status/error" component={CheckoutStatusError}/>
                </Provider> : ""
        );
    }
}
