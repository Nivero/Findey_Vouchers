import React, {Component} from 'react';
import VoucherPageHeader from './voucherPageHeader/VoucherPageHeader'
import Voucher from "./voucher/Voucher";

export class Home extends Component {
    constructor(props) {
        super(props);
    }

    componentDidMount() {
    }

    render() {


        return (
            <div className="d-flex flex-column justify-content-center">
                <VoucherPageHeader/>
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
            </div>
        );
    }


}
