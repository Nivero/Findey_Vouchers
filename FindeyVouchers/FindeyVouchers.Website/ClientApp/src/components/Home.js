import React, {Component} from 'react';
import VoucherPageHeader from './voucherPageHeader/VoucherPageHeader'
import Voucher from "./voucher/Voucher";
import CheckoutBar from "./CheckoutBar/CheckoutBar";

export class Home extends Component {
    constructor(props) {
        super(props);
        this.state = {response: {}, shoppingCart: []};
        this.AddToCart = this.AddToCart.bind(this);
        this.RemoveFromCart = this.RemoveFromCart.bind(this);

    }

    AddToCart(id) {

    }

    RemoveFromCart(id) {

    }

    componentDidMount() {
        this.fetchMerchant();
    }

    async fetchMerchant() {
        const requestOptions = {
            method: 'GET',
        };
        // Merchant name should be here.
        // It will be the first part of the url IE. nivero.findey.nl
        fetch(`merchant/nivero`, requestOptions)
            .then((response) => response.json())
            .then((response) => {
                this.setState({
                    response: response
                });
            });
    }

    render() {
        return (
            <div className="d-flex flex-column justify-content-start">
                <VoucherPageHeader merchant={this.state.response.merchant}/>
                {this.state.response.vouchers ? this.state.response.vouchers.map((item, key) =>
                    <div className="d-flex flex-row justify-content-center" key={key}>
                        <Voucher voucher={item} increment={this.AddToCart} decrement={this.RemoveFromCart}/>
                    </div>
                ) : ""}

                <CheckoutBar/>
            </div>

        );
    }


}
