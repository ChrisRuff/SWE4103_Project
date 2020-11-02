import React, { Component } from "react";
import { Button } from "react-bootstrap";
import "./Seat.css";
import {StateManager} from "../StateManager.js"

export default class Seat extends Component {
	constructor(props) {
		super(props);
		this.y = StateManager.getY();
		this.x = StateManager.getX();
		if(props.seatType == null || props.seatType === "")
		{
			this.state = {
				seatType: "available",
			};
		}
		else
		{
			this.state = {
				seatType: props.seatType,
			};
		}
		StateManager.incX();
	}
	componentWillReceiveProps(props)
	{
		this.y = StateManager.getY();
		this.x = StateManager.getX();
		if(props.seatType == null || props.seatType === "")
		{
			this.setState({
				seatType: "available",
			});
		}
		else
		{
			this.setState({
				seatType: props.seatType,
			});
		}
		StateManager.incX();
	}
	disable = () => {
		this.setState({seatType: "disabled"});
		StateManager.changeSeatType(this.x, this.y, "disabled")
	}
	reserve = () => {
		this.setState({seatType: "reserved"}); 
		StateManager.changeSeatType(this.x, this.y, "reserved")
	}

	handleClick = () => {
		const currentState = this.state.seatType;
		switch(currentState) {
			case "available":
				this.reserve();
				break;
			case "reserved":
				this.setState({seatType: "accessible"}); 
				StateManager.changeSeatType(this.x, this.y, "accessible")
				break;
			case "accessible":
				this.disable();
				break;
			case "disabled":
				this.setState({seatType: "available"});
				StateManager.changeSeatType(this.x, this.y, "available")
				break;
			default:
				this.disable();
		} 
	}
	
	render() {
		StateManager.addSeat(this.x, this.y, this.state.seatType, this)
		return (
			<Button className={this.state.seatType} onClick = {this.handleClick}>Seat</Button>
		);
	}
}
	
