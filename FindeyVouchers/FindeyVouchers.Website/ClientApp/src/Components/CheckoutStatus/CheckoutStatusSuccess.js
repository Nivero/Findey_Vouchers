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
                        <h1 className="display-5 text-uppercase text-success"><FaCheck size={64} /> Payment successful</h1>
                        <hr className="my-2" />
                        <p>You have successfull transferred.</p>
                        <p className="lead">
                        <Link to="/"><Button color="success">Done</Button></Link>
                        </p>
                    </Jumbotron>
            </Container>
        );
    }
}
