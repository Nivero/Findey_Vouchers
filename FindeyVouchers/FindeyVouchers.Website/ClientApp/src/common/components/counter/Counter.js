import React from 'react';
import { Button, ButtonGroup } from 'reactstrap';

import styles from './counter.module.css'

const Counter = ({ onAdd, onRemove, value }) => {
  return (
    <ButtonGroup>
      <Button className={styles.add}  outline onClick={onAdd}>-</Button>
      <Button outline
              disabled> {value} </Button>
      <Button className={styles.remove}  outline onClick={onRemove} >+</Button>
    </ButtonGroup>
  );
};

Counter.displayName = 'Counter';

export default Counter;
