import React from 'react';
import { connect } from 'react-redux';
import { CART_RESET } from '../../../redux/actions';
import { Button, Col, Row } from 'reactstrap';
import styles from './footer.module.css';
import { Link } from 'react-router-dom';

const Footer = ( { cartTotal, cartAmount, dispatch } ) => {
    const handleCancel = (event) => {
        dispatch({type: CART_RESET});
        window.location.reload(false);
    }

    return (
        <footer className={`fixed-bottom ${cartAmount > 0 ? 'visible' : 'invisible'}`}>
            <div className={`d-flex flex-sm-column flex-md-row justify-content-center align-items-center ${styles.footer}`}>

                <div className="font-weight-bold ">
                    <span> { cartAmount } Vouchers </span>|<span> Total: € { cartTotal } </span>
                </div>

                <div className="ml-md-5 mt-sm-2 mt-md-0">
                    <Link to="/checkout"><Button color="secondary" className={`text-uppercase rounded-btn ${styles.btn}`}>Naar Checkout</Button></Link>{' '}
                    <Button color="secondary" className="rounded-btn ml-4" onClick={handleCancel}>X</Button>
                </div>
            </div>
        </footer>
    );
}

const mapStateToProps = state => {
    return { cartTotal: state.cartTotal, cartAmount: state.cartAmount }
}

export default connect(mapStateToProps)(Footer);
