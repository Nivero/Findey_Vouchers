import React from 'react';
import { Route, Link } from 'react-router-dom';
import CompanyVouchers from './Components/CompanyVouchers';
import Checkout from './Components/Checkout';
import CheckoutStatus from './Components/CheckoutStatus';
//vervangen door call in App()
import data from './response.json';
//Tot hier
import { Provider } from 'react-redux';
import { createStore } from 'redux';
import reducer from './redux/reducer';


const initialStore = {
  cartItems: data.vouchers,
  cartTotal: 0,
  cartAmount: 0
};

const store = createStore(reducer, initialStore);

export default class App extends React.Component {
  render(){
    return (
      <Provider store={store}>
        <Route exact path="/" component={() => <CompanyVouchers data={data} />} />
        <Route exact path="/checkout" component={Checkout} />
        <Route exact path="/checkout-status" component={CheckoutStatus} />
      </Provider>
    );
  }
}
