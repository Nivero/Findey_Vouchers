import React from 'react';
import { Row, Col } from 'reactstrap';
import Voucher from './Voucher'
import './voucherList.css';

export default class VoucherList extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            vouchers: []
        };
    }

    componentDidMount(){
        this.setState({ vouchers: this.props.vouchers });
    }

    render(){
        return (
            <section className="overflow-hidden mci-section-footer-spacing">
                <Row className="justify-content-center" >
                    <Col md="8">
                        { this.state.vouchers.map((voucher) => { return <Voucher key={voucher.id} data={voucher}/> }) }
                    </Col>
                </Row>
            </section>
        );
    }
}
  