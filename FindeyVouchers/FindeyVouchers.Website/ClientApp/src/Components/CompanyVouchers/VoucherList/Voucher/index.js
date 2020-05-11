import React from 'react';
import {connect} from 'react-redux';
import {CART_ADD, CART_REMOVE} from '../../../../redux/actions';
import {
    Row,
    Col,
    Card,
    CardImg,
    Modal,
    ModalBody,
    ModalHeader,
    ModalFooter,
    CardTitle,
    Button,
    ButtonGroup
} from 'reactstrap';
import './voucher.css';

class Voucher extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            modal: false,
            amountOfVoucher: 0
        };
        this.handleVoucherRemove = this.handleVoucherRemove.bind(this);
        this.handleVoucherAdd = this.handleVoucherAdd.bind(this);
    }

    calcDiscount(price, precentage) {
        return (price * (1 - (precentage / 100))).toFixed(2);
    }

    toggle = () => {
        this.setState({modal: !this.state.modal});
    }

    handleVoucherRemove() {
        if (this.state.amountOfVoucher > 0) {
            this.setState({amountOfVoucher: this.state.amountOfVoucher - 1});
            this.props.remove();
        }
    }

    handleVoucherAdd() {
        this.setState({amountOfVoucher: this.state.amountOfVoucher + 1});
        this.props.add();
    }

    render() {
        return (
            <Card body className="mci-card m-1 mb-2">
                <Row>
                    <Col md="3">
                        <CardImg className="mci-card-image" src={this.props.data.image} alt="Name of image"/>
                    </Col>
                    <Col md="6">
                        <Row>
                            <CardTitle className="text-uppercase h4"> {this.props.data.name} </CardTitle>
                        </Row>
                        <Row>
                            <span className="mci-voucher-discription text-muted"> 
                                {(this.props.data.description.length > 150)
                                    ? <div>
                                        {this.props.data.description.substring(0, 150) + ' ... '}
                                        <span className="mci-readmore" onClick={this.toggle}>Read more</span>
                                        <Modal isOpen={this.state.modal} toggle={this.toggle}>
                                            <ModalHeader toggle={this.toggle}> {this.props.data.name} </ModalHeader>
                                            <ModalBody>
                                                {this.props.data.description}
                                            </ModalBody>
                                            <ModalFooter>
                                                <Button color="secondary" onClick={this.toggle}>Close</Button>
                                            </ModalFooter>
                                        </Modal>
                                    </div>
                                    : this.props.data.description
                                }
                            </span>
                        </Row>
                        <Row>
                            <ButtonGroup>
                                <Button outline onClick={this.handleVoucherRemove} size="sm">-</Button>
                                <Button outline color="secondary" size="sm"
                                        disabled> {this.state.amountOfVoucher} </Button>
                                <Button outline onClick={this.handleVoucherAdd} size="sm">+</Button>
                            </ButtonGroup>
                        </Row>
                    </Col>
                    <Col md="3" className="text-right position-relative">
                        <span className="mci-price font-weight-bold"> € {this.props.data.price} </span>
                    </Col>
                </Row>
            </Card>
        );
    }
}

const mapDispatchToProps = (dispatch, voucherProps) => {
    return {
        remove: () => dispatch({type: CART_REMOVE, payload: {voucher: voucherProps.data}}),
        add: () => dispatch({type: CART_ADD, payload: {voucher: voucherProps.data}})
    };
};

export default connect(null, mapDispatchToProps)(Voucher);