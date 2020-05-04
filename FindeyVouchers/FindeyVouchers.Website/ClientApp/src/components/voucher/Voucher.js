import React, {Component} from 'react';
import './Voucher.css'
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPlus, faMinus} from "@fortawesome/free-solid-svg-icons";

export default class Voucher extends Component {
    constructor(props) {
        super(props);
        this.state = {
            amount: 0
        };
    }

    componentDidMount() {
    }

    increment() {
     this.props.increment(this.props.voucher.id);
    };

    decrement() {
        this.props.decrement(this.props.voucher.id);
    };

    render() {
        return (
            <div className="d-flex justify-content-between flex-row m-2 w-75 voucher-border-bottom">
                <div>
                    <img
                        src={`https://findeystorage.blob.core.windows.net/voucher-images/${this.props.voucher.image}`}
                        alt="voucher-image"
                        className="voucher-image p-2"
                    /></div>
                <div className="d-flex flex-column mr-auto">
                    <div className="voucher-title">{this.props.voucher.name}</div>
                    <div className="d-flex flex-grow-1">{this.props.voucher.description}
                    </div>
                    <div className="d-flex flex-row  mt-3 mb-3">
                        <FontAwesomeIcon className="mr-4 mt-1 button-hover" onClick={(e) => this.decrement(e)} icon={faMinus}/>
                        <div>{this.state.amount}</div>
                        <FontAwesomeIcon onClick={(e) => this.increment(e)} className="ml-4 mt-1 button-hover" icon={faPlus}/>
                                      

                    </div>
                </div>
                <div className="d-flex flex-column justify-content-between m-3 w-25">
                    <div className="d-flex flex-row justify-content-end text-danger text-custom-large">eventuele
                        korting
                    </div>
                    <div className="d-flex flex-row justify-content-end">€ {this.props.voucher.price}</div>
                </div>
            </div>
        );
    }


}
