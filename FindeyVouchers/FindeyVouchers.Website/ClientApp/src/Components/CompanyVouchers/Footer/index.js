import React from 'react';
import { connect } from 'react-redux';
import { CART_RESET } from '../../../redux/actions';
import { Button, Col, Row } from 'reactstrap';
import './footer.css';
import { Link } from 'react-router-dom';

const Footer = ( { cartTotal, cartAmount, dispatch } ) => {
    const handleCancel = (event) => {
        dispatch({type: CART_RESET});
        window.location.reload(false);
    }

    return (
        <footer className={`fixed-bottom border-bottom-0 border-left-0 border-right-0 rounded-top mci-footer p-4 ${cartAmount > 0 ? 'visible' : 'invisible'}`}>
            <Row>
                <Col md={{ size: 1, offset: 2 }} className="font-weight-bolder p-2"> 
                    <span> { cartAmount } Vouchers </span>
                </Col>
                <Col md={{ size: 2, offset: 0 }} className="font-weight-bolder mci-left-border p-2"> 
                    <span style={{paddingLeft: '10px'}}> Total: € { cartTotal } </span>
                </Col>
                <Col md={{ size: 2, offset: 3 }} className="j">
                    <Link to="/checkout"><Button color="secondary" size="md" className="text-uppercase">Naar Checkout</Button></Link>{' '}
                    <Button color="secondary" onClick={handleCancel}>X</Button>
                </Col>
            </Row>
        </footer>
    );
}

const mapStateToProps = state => {
    return { cartTotal: state.cartTotal, cartAmount: state.cartAmount }
}

export default connect(mapStateToProps)(Footer);
  