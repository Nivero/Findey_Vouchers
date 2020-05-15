import React from 'react';
import { connect } from 'react-redux';
import { CART_RESET } from '../../../redux/actions';
import { Button, Container, Row } from 'reactstrap';
import styles from './footer.module.css';
import { Link } from 'react-router-dom';

const Footer = ( { cartTotal, cartAmount, dispatch } ) => {
    const handleCancel = (event) => {
        dispatch({type: CART_RESET});
        window.location.reload(false);
    }

    return (
        <footer className={`fixed-bottom  ${styles.footer} ${cartAmount > 0 ? 'visible' : 'invisible'}`}>
            <Container >
                <div style={{ minHeight: 80 }} className={`d-flex flex-column flex-md-row justify-content-md-between align-items-center `}>

                    <div className="font-weight-bold mt-2 mt-md-0">
                        <span> { cartAmount } Vouchers </span>|<span> € { cartTotal } </span>
                    </div>

                    <div className="ml-md-5 mt-3 mb-2 mb-md-0 mt-md-0 ">
                        <Link to="/checkout"><Button color="secondary" className={`text-uppercase rounded-btn ${styles.btn}`}>Naar Checkout</Button></Link>{' '}
                        <Button color="secondary" className="rounded-btn ml-4" onClick={handleCancel}>X</Button>
                    </div>
                </div>
            </Container>

        </footer>
    );
}

const mapStateToProps = state => {
    return { cartTotal: state.cartTotal, cartAmount: state.cartAmount }
}

export default connect(mapStateToProps)(Footer);
