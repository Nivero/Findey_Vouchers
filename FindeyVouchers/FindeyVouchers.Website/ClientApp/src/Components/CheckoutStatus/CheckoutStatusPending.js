import React from 'react';
import { Container, Jumbotron, Button } from 'reactstrap';
import { FaHourglass} from "react-icons/fa";
import { Link } from 'react-router-dom';
import './checkoutStatus.css';

export default class CheckoutStatusSuccess extends React.Component {
    render(){
        return (
            <Container>
                <Jumbotron>
                    <h1 className="display-5 text-uppercase text-warning"><FaHourglass size={64} /> Uw betaling wordt verwerkt!</h1>
                    <hr className="my-2" />
                    <p>Houd uw inbox in de gaten voor uw vouchers.</p>
                    <p className="lead">
                        <Link to="/"><Button color="success">Terug naar de homepage</Button></Link>
                    </p>
                </Jumbotron>
            </Container>
        );
    }
}
