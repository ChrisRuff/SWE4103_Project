import React, { Component } from "react";
import { Button } from "react-bootstrap";
import "./StudentSeat.css";
import {StateManager} from "../StateManager.js"
import { AspNetConnector } from "../AspNetConnector.js" 

export default class StudentSeat extends Component {
	constructor(props) {
		super(props);
		this.y = StateManager.getY();
		this.x = StateManager.getX();
		var reg = new RegExp("[a-zA-Z]+[ a-zA-Z].");
		if(props.seatType == null || props.seatType === "")
		{
			this.state = {
				seatType: "available",
				original: "available",
				email: "",
				name: ""
			};
		}
		else if(props.name !== "")
		{
			this.state = {
				seatType: props.seatType,
				original: props.seatType,
				email: props.email,
				//displays only the first name & last name initial for privacy reasons
				name: reg.exec(props.name)
			};
		}
		else
		{
			this.state = {
				seatType: props.seatType,
				original: props.seatType,
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
		var reg = new RegExp("[a-zA-Z]+[ a-zA-Z].");
		if(props.seatType == null || props.seatType === "")
		{
			this.setState({
				seatType: "available",
				email: "",
				name: ""
			});
		}
		else if(props.name !== "")
		{
			this.state = {
				seatType: props.seatType,
				original: props.seatType,
				email: props.email,
				name: reg.exec(props.name)
			};
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
	disable = () => {
		this.setState({seatType: "disabled"});
		StateManager.changeSeatType(this.x, this.y, "disabled")
	}
	reserve = () => {
		this.state.original = this.state.seatType;
		this.setState({seatType: "reserved"}); 
		StateManager.changeSeatType(this.x, this.y, "reserved")
	}

	handleClick = () => {
		if(StateManager.getSelectedSeat() === null){
			if(this.state.seatType === "available" || this.state.seatType === "accessible"){
				this.state.original = this.state.seatType;
				this.setState({seatType: "reserved"}); 
				StateManager.changeSeatType(this.x, this.y, "reserved");
			} else if(this.state.seatType === "reserved" && this.state.email == StateManager.getStudent().email) {
				this.setState({seatType: "available"});
				StateManager.changeSeatType(this.x, this.y, "available");
			}
			else {
				return;
			}
		}
		else {
			if(this.x === StateManager.getSelectedSeat().x && this.y === StateManager.getSelectedSeat().y){
				StateManager.setSelectedSeat(null);
				this.setState({seatType: this.state.original});
				StateManager.changeSeatType(this.x, this.y, this.state.original);
				return;
			}
			else if(this.state.seatType === "available" || this.state.seatType === "accessible"){
				this.state.original = this.state.seatType;
				this.setState({seatType: "reserved"}); 
				StateManager.changeSeatType(this.x, this.y, "reserved");
				StateManager.getSelectedSeat().setState({seatType: StateManager.getSelectedSeat().state.original});
				StateManager.changeSeatType(StateManager.getSelectedSeat().x, StateManager.getSelectedSeat().y, 
					StateManager.getSelectedSeat().state.original);
			}
			else{
				return;
			}
			
		}
		
		StateManager.setSelectedSeat(this);
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
	
