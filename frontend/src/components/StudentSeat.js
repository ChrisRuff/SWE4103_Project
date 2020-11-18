import React, { Component } from "react";
import { Button } from "react-bootstrap";
import "./StudentSeat.css";
import {StateManager} from "../StateManager.js"
import { AspNetConnector } from "../AspNetConnector.js" 

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
		
		if(StateManager.getSelectedSeat() === null){
		
			if(this.state.seatType === "available"){
				this.setState({seatType: "reserved"}); 
				StateManager.changeSeatType(this.x, this.y, "reserved");
			}
			else{
				return;
			}
		}

		else {
			
			if(this.x === StateManager.getSelectedSeat().x && this.y === StateManager.getSelectedSeat().y){
				StateManager.setSelectedSeat(null);
			
				this.setState({seatType: "available"});
				StateManager.changeSeatType(this.x, this.y, "available");
				return;
			}
			else if(this.state.seatType === "available"){

			
				this.setState({seatType: "reserved"}); 
				StateManager.changeSeatType(this.x, this.y, "reserved");
				StateManager.getSelectedSeat().setState({seatType: "available"});
				StateManager.changeSeatType(StateManager.getSelectedSeat().x, StateManager.getSelectedSeat().y, "available");
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
				Seat<br/>
				X:{this.x}&nbsp;&nbsp;&nbsp;Y:{this.y}
			</Button>
		);
	}
}
	
