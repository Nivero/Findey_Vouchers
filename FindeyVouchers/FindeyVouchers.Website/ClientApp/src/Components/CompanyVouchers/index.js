import React from 'react';
import {Container} from 'reactstrap';
import CompanyInfo from './company-info';
import VoucherList from './VoucherList'
import Footer from './Footer'
import TopHeader from '../../common/components/top-header/TopHeader';

export default class CompanyVouchers extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
          <div>
            <TopHeader title={'OVERZICHT VOUCHERS'} />
            <Container className="mt-5">
              <CompanyInfo merchant={this.props.data.merchant}/>
              <div className="mt-5">
                <VoucherList vouchers={this.props.data.vouchers}/>
              </div>

            </Container>
            <Footer/>
          </div>

        );
    }
}
