import React from 'react';
import {Container, Jumbotron} from 'reactstrap';
import {FaExclamationCircle} from "react-icons/fa";

export default class MerchantNotFound extends React.Component {
    render(){
        return (
            <Container>
                <Jumbotron>
                    <h1 className="display-5 text-danger"><FaExclamationCircle size={64} /> Deze pagina bestaat niet. Controleer de url en probeer het opnieuw</h1>
                    <hr className="my-2" />
                    <p>Neem contact op met de <a href="mailto:info@findey.co">helpdesk!</a></p>
                </Jumbotron>
            </Container>
        );
    }
}
