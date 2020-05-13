import React from 'react';
import { Container, Jumbotron, Button } from 'reactstrap';
import { FaCheck} from "react-icons/fa";
import { Link } from 'react-router-dom';
import './checkoutStatus.css';

export default class CheckoutStatusSuccess extends React.Component {
    render(){
        return (
            <Container>
                    <Jumbotron>
                        <h1 className="display-5 text-uppercase text-success"><FaCheck size={64} />Uw betaling is geslaagd!</h1>
                        <hr className="my-2" />
                        <p>Bedankt voor uw aankoop. Controleer uw mailbox voor uw vouchers</p>
                        <p className="lead">
                        <Link to="/"><Button color="success">Terug naar de homepage</Button></Link>
                        </p>
                    </Jumbotron>
            </Container>
        );
    }
}
