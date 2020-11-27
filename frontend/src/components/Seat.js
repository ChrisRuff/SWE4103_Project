import React, { Component } from "react";
import { Button } from "react-bootstrap";
import "./Seat.css";
import {StateManager} from "../StateManager.js"

export default class Seat extends Component {
	constructor(props) {
		super(props);
		this.y = StateManager.getY();
		this.x = StateManager.getX();
		if(props.seatType === undefined || props.seatType === "")
		{
			this.state = {
				seatType: "available",
				email: "",
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
				email: "",
				name: ""
			});
		}
		else
		{
			this.setState({
				seatType: props.seatType,
				email: props.email,
				name: props.name === undefined ? "" : props.name
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
		if(StateManager.getTrackingMode())
		{
			if(currentState === "reserved")
			{
				this.setState({seatType: "absent"}); 
				StateManager.changeSeatType(this.x, this.y, "absent")
			}
			else if(currentState === "absent")
			{
				this.setState({seatType: "reserved"}); 
				StateManager.changeSeatType(this.x, this.y, "reserved")
			}
		}
		else
		{
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
					this.setState({seatType: "available"});
					StateManager.changeSeatType(this.x, this.y, "available")
					break;
			} 
		}
		console.log(currentState);
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
	
