import React from 'react';
import {Row, Col} from 'reactstrap';
import {FaMapMarkerAlt, FaGlobe, FaTelegramPlane, FaPhoneSquareAlt} from "react-icons/fa";
import styles from './contact-info.module.css';

const iconSize = 18;

const CompanyInfo = ({ merchant }) => {
    return (
      <Row className="border-bottom">
          <Col sm="8" className="pr-lg-5">
              <div className="pr-lg-5">
                  <h3> {merchant.name} </h3>

                  <p> {merchant.description || "Hieronder vindt u de vouchers die wij beschikbaar hebben gesteld. Na uw aankoop ontvangt u een e-mail met daarin de vouchers die besteed kunnen worden bij onze onderneming. \n" +
                    "We willen u alvast bedanken en graag tot snel!" }</p>
              </div>
          </Col>
          <Col sm="4" className={styles.contact}>
              <div className="mb-1"><FaMapMarkerAlt size={iconSize} /> {merchant.address || '--'}</div>
              <div className="mb-1"><FaGlobe size={iconSize}/> {merchant.website || "/"}</div>
              <div className="mb-1"><FaTelegramPlane size={iconSize}/> {merchant.email || "/"}</div>
              <div className="mb-1"><FaPhoneSquareAlt size={iconSize}/> {merchant.phoneNumber || "/"}</div>
          </Col>
      </Row>
    );
};

CompanyInfo.displayName = 'CompanyInfo';

export default CompanyInfo;


