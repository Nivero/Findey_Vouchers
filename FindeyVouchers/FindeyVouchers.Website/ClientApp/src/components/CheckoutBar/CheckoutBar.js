import React, {Component} from 'react';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faChevronRight} from "@fortawesome/free-solid-svg-icons";

export default class VoucherPageHeader extends Component {
    constructor(props) {
        super(props);

    }

    componentDidMount() {
    }

    render() {
        return (
            <div className="fixed-bottom">
                <div className="container">
                    <div
                        className="d-flex flex-row justify-content-between border-left border-top border-right rounded-top">
                        <div className="d-flex flex-row p-3">
                            <div className="d-flex m-2 text-custom-large">5 vouchers</div>
                            <div className="d-flex m-2 text-custom-large">|</div>
                            <div className="d-flex m-2 text-custom-large">€ 10</div>
                        </div>

                        <div className="d-flex flex-row p-3">
                            <a href="www.google.com"
                               className="d-flex pr-3 pl-3 pt-2 pb-2 text-custom-large border rounded background-color-default text-white text-decoration-none">
                                <div className="d-block">
                                    <span>Naar checkout</span>
                                    <FontAwesomeIcon icon={faChevronRight} className="ml-2 pt-1"/>
                                </div>
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        );
    }


}
