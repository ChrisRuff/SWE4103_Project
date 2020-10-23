import React, { Component } from "react";
import { Button } from "react-bootstrap";
import "./Seat.css";
import {StateManager} from "../StateManager.js"

export default class Seat extends Component {

	constructor(props) {
		super(props);
		this.y = StateManager.getY();
		this.x = StateManager.getX();
		this.state = {
            seatType: "available",
        };
		StateManager.incX();
	}

	handleClick = () => {
		const currentState = this.state.seatType;
		switch(currentState) {
			case "available":
				this.setState({seatType: "reserved"}); 
				StateManager.changeSeatType(this.x, this.y, "reserved")
				break;
			case "reserved":
				this.setState({seatType: "accessible"}); 
				StateManager.changeSeatType(this.x, this.y, "accessible")
				break;
			case "accessible":
				this.setState({seatType: "disabled"}); 
				StateManager.changeSeatType(this.x, this.y, "disabled")
				break;
			case "disabled":
				this.setState({seatType: "available"});
				StateManager.changeSeatType(this.x, this.y, "available")
				break;
			default:
				this.setState({seatType: "disabled"}); 
				StateManager.changeSeatType(this.x, this.y, "disabled")
		  } 
	}
	
	render() {
				StateManager.addSeat(this.x, this.y, this.state.seatType)
		return (
			<Button className={this.state.seatType} onClick = {this.handleClick}>Seat</Button>
		);
	}
}
	
