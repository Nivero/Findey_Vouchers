import React, {Component} from 'react';
import VoucherPageHeader from './voucherPageHeader/VoucherPageHeader'
import Voucher from "./voucher/Voucher";
import CheckoutBar from "./CheckoutBar/CheckoutBar";

export class Home extends Component {
    constructor(props) {
        super(props);
        this.state = {response: {}};
    }

    componentDidMount() {
        this.fetchMerchant();
    }

    async fetchMerchant() {
        const requestOptions = {
            method: 'GET',
        };
        fetch(`merchant/info/nivero`, requestOptions)
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
                <VoucherPageHeader merchant={this.state.response}/>
                <div className="d-flex flex-row justify-content-center">
                    <Voucher/>
                </div>
                <div className="d-flex flex-row justify-content-center">
                    <Voucher/>
                </div>
                <div className="d-flex flex-row justify-content-center">
                    <Voucher/>
                </div>
                <div className="d-flex flex-row justify-content-center">
                    <Voucher/>
                </div>
                <CheckoutBar/>

            </div>

        );
    }


}
