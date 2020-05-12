import React from 'react';
import {Container} from 'reactstrap';
import Header from './Header';
import VoucherList from './VoucherList'
import Footer from './Footer'

export default class CompanyVouchers extends React.Component {
    constructor(props) {
        super(props);
    }


    render() {
        return (
            <Container fluid={true} className="p-0">
                <Header merchant={this.props.data.merchant}/>
                <VoucherList vouchers={this.props.data.vouchers}/>
                <Footer/>
            </Container>
        );
    }
}
