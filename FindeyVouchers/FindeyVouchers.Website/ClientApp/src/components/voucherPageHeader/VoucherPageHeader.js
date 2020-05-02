import React, {Component} from 'react';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faMapMarkerAlt} from "@fortawesome/free-solid-svg-icons";
export default class VoucherPageHeader extends Component {
    constructor(props) {
        super(props);

    }

    componentDidMount() {
    }

    render() {
        return (
            <div className="d-flex justify-content-start background-color-default text-white">
                <div className="mr-5 w-75 p-2 text-center">Company name</div>
                <div className="d-flex justify-content-center flex-column border-left p-2 mt-2 mb-2">
                    <div className="d-block">
                        <FontAwesomeIcon icon={faMapMarkerAlt} className="mr-2"/>
                        <span>Locatie</span>
                    </div>
                    <div className="d-block">
                        <FontAwesomeIcon icon={faMapMarkerAlt} className="mr-2"/>
                        <span>Website</span>
                    </div>
                    <div className="d-block">
                        <FontAwesomeIcon icon={faMapMarkerAlt} className="mr-2"/>
                        <span>E-mail</span>
                    </div>
                    <div className="d-block">
                        <FontAwesomeIcon icon={faMapMarkerAlt} className="mr-2"/>
                        <span>Telefoon</span>
                    </div>
                </div>
            </div>
        );
    }


}
