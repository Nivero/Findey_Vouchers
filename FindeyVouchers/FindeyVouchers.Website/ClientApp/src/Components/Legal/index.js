import React from 'react';
import {Container, Jumbotron, Button, ListGroup, ListGroupItem} from 'reactstrap';
import { FaBalanceScaleRight} from "react-icons/fa";
import { Link } from 'react-router-dom';
import termsAndConditions from './files/ALGEMENE VOORWAARDEN - klant.pdf'
import privacyStatemement from './files/PRIVACYVERKLARING - klant.pdf'

export default class Legal extends React.Component {
    render(){
        return (
            <Container>
                <Jumbotron>
                    <h1 className="display-5 text-uppercase text-primary"><FaBalanceScaleRight size={64} />Algemene voorwaarden en privacy verklaring</h1>
                    <hr className="my-2" />
                    <p>U vind hier:</p>
                    <ListGroup className="mb-4">
                        <ListGroupItem>Algemene voorwaarden  <a href={termsAndConditions} className="float-right" target="_blank">download</a></ListGroupItem>
                        <ListGroupItem>Privacy verklaring  <a href = {privacyStatemement} className="float-right" target="_blank">download</a></ListGroupItem>
                    </ListGroup>
                    <p className="lead">
                        <Link to="/"><Button color="primary">Terug naar de homepage</Button></Link>
                    </p>
                </Jumbotron>
            </Container>
        );
    }
}
