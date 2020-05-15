import React from 'react';
import { connect } from 'react-redux';
import { CART_ADD, CART_REMOVE } from '../../../../redux/actions';
import {
  Row,
  Col,
  CardImg,
  Modal,
  ModalBody,
  ModalHeader,
  ModalFooter,
  Button,

} from 'reactstrap';

import Counter from '../../../../common/components/counter/Counter';

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
    this.setState({ modal: !this.state.modal });
  }

  handleVoucherRemove() {
    if (this.state.amountOfVoucher > 0) {
      this.setState({ amountOfVoucher: this.state.amountOfVoucher - 1 });
      this.props.remove();
    }
  }

  handleVoucherAdd() {
    this.setState({ amountOfVoucher: this.state.amountOfVoucher + 1 });
    this.props.add();
  }

  render() {
    return (

      <Row className="pt-4 pb-4">
        <Col md="3" lg={2}>
          <CardImg width="100%" src={this.props.data.image} alt="Name of image"/>
        </Col>
        <Col md="9" lg={10}>
          <div className="text-center text-md-left mt-2 mt-md-0">
            <h5>{this.props.data.name}</h5>
            <p>
               <span className="mci-voucher-discription">
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
            </p>
          </div>
          <div className="d-flex justify-content-between">
            <Counter
               value={this.state.amountOfVoucher}
               onAdd={this.handleVoucherRemove}
               onRemove={this.handleVoucherAdd}
            />

            <div> € {this.props.data.price} </div>
          </div>
        </Col>
      </Row>
    );
  }
}

const mapDispatchToProps = (dispatch, voucherProps) => {
  return {
    remove: () => dispatch({ type: CART_REMOVE, payload: { voucher: voucherProps.data } }),
    add: () => dispatch({ type: CART_ADD, payload: { voucher: voucherProps.data } })
  };
};

export default connect(null, mapDispatchToProps)(Voucher);
