import React from 'react';
import styles from './top-header.module.css';

const TopHeader = ({ title }) => {
  return (
    <header className={styles['top-header']}>
        <h2>{title}</h2>
    </header>
  );
};

TopHeader.displayName = 'TopHeader';

export default TopHeader;
