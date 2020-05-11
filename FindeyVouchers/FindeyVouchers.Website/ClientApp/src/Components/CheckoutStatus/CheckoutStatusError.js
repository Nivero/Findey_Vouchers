import React from 'react';
import { Container, Jumbotron, Button } from 'reactstrap';
import { FaExclamationCircle} from "react-icons/fa";
import { Link } from 'react-router-dom';
import './checkoutStatus.css';

export default class CheckoutStatusError extends React.Component {
    render(){
        return (
            <Container>
                    <Jumbotron>
                        <h1 className="display-5 text-uppercase text-danger"><FaExclamationCircle size={64} /> Er is iets fout gegaan met uw betaling</h1>
                        <hr className="my-2" />
                        <p>Neem contact op met de <a href="mailto:info@findey.co">helpdesk!</a></p>
                        <p className="lead">
                        {/*<Link to="/checkout"><Button color="danger">Try Again</Button></Link>*/}
                        <Link to="/"><Button color="secondary">Terug naar de homepage</Button></Link>
                        </p>
                    </Jumbotron>
            </Container>
        );
    }
}
