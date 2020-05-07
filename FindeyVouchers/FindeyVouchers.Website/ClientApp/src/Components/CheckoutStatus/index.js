import React from 'react';
import { Container, Jumbotron, Button } from 'reactstrap';
import { FaCreativeCommonsNcEu, FaCheck} from "react-icons/fa";
import { Link } from 'react-router-dom';
import './checkoutStatus.css';

export default class CheckoutStatus extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            success: false
        };
    }

    render(){
        return (
            <Container>
                { this.state.success 
                    ? 
                    <Jumbotron>
                        <h1 className="display-5 text-uppercase text-success"><FaCheck size={64} /> Payment successful</h1>
                        <hr className="my-2" />
                        <p>You have successfull transferred.</p>
                        <p className="lead">
                        <Link to="/"><Button color="success">Done</Button></Link>
                        </p>
                    </Jumbotron>
                    :
                    <Jumbotron>
                        <h1 className="display-5 text-uppercase text-danger"><FaCreativeCommonsNcEu size={64} /> Something Went Wrong</h1>
                        <hr className="my-2" />
                        <p>Something went wrong. Try one more again.</p>
                        <p className="lead">
                        <Link to="/checkout"><Button color="danger">Try Again</Button></Link>{' '}
                        <Link to="/"><Button color="secondary">Go to Voucherslist</Button></Link>
                        </p>
                    </Jumbotron>
                }
            </Container>
        );
    }
}
