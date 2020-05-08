import React from 'react';
import { Container, Jumbotron, Button } from 'reactstrap';
import { FaCreativeCommonsNcEu} from "react-icons/fa";
import { Link } from 'react-router-dom';
import './checkoutStatus.css';

export default class CheckoutStatusError extends React.Component {
    render(){
        return (
            <Container>
                    <Jumbotron>
                        <h1 className="display-5 text-uppercase text-danger"><FaCreativeCommonsNcEu size={64} /> Something Went Wrong</h1>
                        <hr className="my-2" />
                        <p>Something went wrong. Try one more again.</p>
                        <p className="lead">
                        <Link to="/checkout"><Button color="danger">Try Again</Button></Link>{' '}
                        <Link to="/"><Button color="secondary">Go to Voucherslist</Button></Link>
                        </p>
                    </Jumbotron>
            </Container>
        );
    }
}
