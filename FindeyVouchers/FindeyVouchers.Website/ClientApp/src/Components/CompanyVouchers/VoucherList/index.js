import React from 'react';
import Voucher from './Voucher'
import { Button } from 'reactstrap';

import { mapCategories } from './helpers';
import styles from './voucher-list.module.css'

const VoucherList = ({ vouchers }) => {
  const categories = mapCategories(vouchers);

  return (
    <section  style={{ marginBottom: 125 }}>
      <div className="d-flex mb-3 mt-3">
        {categories.map((category) => {
          return (
            <div key={category.id} className="mt-2 mr-3">
              <Button className={'rounded-btn ' + styles['category-btn']} >{category.name}</Button>
            </div>);
        })
        }
      </div>

      <div className="pt-3">
        {categories.map((category) => {
          return (<div key={category.id}>
            <h3 className="border-bottom pb-3">{category.name}</h3>

            {category.vouchers.map((voucher) => {
              return (
                <div className="border-bottom">
                  <Voucher key={voucher.id} data={voucher}/>
                </div>
              )
            })
            }
          </div>)
        })}
      </div>



    </section>
  );
}

export default VoucherList
