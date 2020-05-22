import React from 'react';
import {
  Card,
  CardBody,
  CardFooter
} from 'reactstrap';
import './checkout.css'
import Voucher from '../../common/components/Voucher';

const OrderOverview = ({ cartItems, cartTotal }) => {
  const items = cartItems.filter((item) => item.amount > 0);

  return (<div>
    <h4 className="font-weight-bold">1. Jouw vouchers</h4>
    <Card>
      <CardBody className="mb-3" style={{ maxHeight:450, overflowY: 'auto' }}>
        {items.length === 0 && (<div>No vouchers selected</div>)}

        {items.map((voucher) => {
          return (
            <div key={voucher.id} className="border-bottom">
              <Voucher viewMode key={voucher.id} data={voucher}/>
            </div>
          )
        })}

      </CardBody>
      <CardFooter className="p-3" style={{ backgroundColor: 'transparent' }} >
        <div className="d-flex justify-content-between " >
          <h5 className="m-0 font-weight-bold" >Totaal</h5>
          <h5 className="m-0 font-weight-bold">€ {cartTotal}</h5>
        </div>

      </CardFooter>
    </Card>
  </div>)

}

export default OrderOverview;

