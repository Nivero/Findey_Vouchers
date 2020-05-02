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
        let dict = this.state.shoppingCart;
        if (dict.key) {
            this.state.shoppingCart.push({key: id, value: this.state.shoppingCart.key + 1})
        } else {
            this.state.shoppingCart.push({key: id, value: 1})
        }
    }

    RemoveFromCart(id) {
        if (this.state.shoppingCart.key && this.state.shoppingCart.key > 0) {
            this.state.shoppingCart.push({key: id, value: this.state.shoppingCart.key - 1})
        }

    }

    componentDidMount() {
        this.fetchMerchant();
    }

    async fetchMerchant() {
        const requestOptions = {
            method: 'GET',
        };
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
