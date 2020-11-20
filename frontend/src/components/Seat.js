import React, { Component } from "react";
import { Button } from "react-bootstrap";
import "./Seat.css";
import {StateManager} from "../StateManager.js"

export default class Seat extends Component {
	constructor(props) {
		super(props);
		this.y = StateManager.getY();
		this.x = StateManager.getX();
		if(props.seatType === null || props.seatType === "")
		{
			this.state = {
				seatType: "available",
				name: ""
			};
		}
		else
		{
			this.state = {
				seatType: props.seatType,
				email: props.email,
				name: props.name
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
	open = () => {
		this.setState({seatType: "open"});
		StateManager.changeSeatType(this.x, this.y, "open")
	} 
	disable = () => {
		this.setState({seatType: "disabledSeat"});
		StateManager.changeSeatType(this.x, this.y, "disabledSeat")
	}
	reserve = () => {
		this.setState({seatType: "reserved"}); 
		StateManager.changeSeatType(this.x, this.y, "reserved")
	}

	handleClick = () => {
		const currentState = this.state.seatType;
		switch(currentState) {
			case "available":
				this.setState({seatType: "accessible"}); 
				StateManager.changeSeatType(this.x, this.y, "accessible")
				break;
			case "accessible":
				this.open();
				break;
			case "open":
				this.disable();
				break;
			case "disabledSeat":
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
			<Button className={this.state.seatType} onClick = {this.handleClick}>
				{this.state.name === "" ? "Seat" : this.state.name }<br/>
				X:{this.x}&nbsp;&nbsp;&nbsp;Y:{this.y}
			</Button>
		);
	}
}
	
