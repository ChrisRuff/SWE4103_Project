import React from "react";
import Tooltip from '@material-ui/core/Tooltip';
import "./Legend.css";

export default function Legend(){
    
    return (
        <div className="legend">
            Seat Legend:
            <Tooltip title={<h6 style={{ color: "white" }}> Students must reserve a seat to sit here. </h6>}>
            <div className="white">
                Selected Access
            </div>
            </Tooltip>
            <Tooltip title={<h6 style={{ color: "white" }}> Students can reserve these seats, but they have a more refined use. </h6>}>
            <div className="blue">
                Extended Access
            </div>
            </Tooltip>
            <Tooltip title={<h6 style={{ color: "white" }}> Anyone can sit here. These seats are not reservable. </h6>}>
            <div className="grey">
                Open Access
            </div>
            </Tooltip>
            <Tooltip title={<h6 style={{ color: "white" }}> No one is allowed to sit here. </h6>}>
            <div className="yellow">
                Locked
            </div>
            </Tooltip>
            <Tooltip title={<h6 style={{ color: "white" }}> These seats are reserved. </h6>}>
            <div className="red">
                Reserved seat
            </div>
            </Tooltip>
        </div>
    );
  }