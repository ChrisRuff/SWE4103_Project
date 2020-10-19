import React, { Component } from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Button, Dropdown, DropdownButton } from "react-bootstrap";
import Paper from "@material-ui/core/Paper";
import "./Seat.css";


export default class Seat extends Component {

	constructor(props) {
		super(props);
		this.y = {}
		this.x = {}
		this.state = {
            seatType: "available",
        };
	}

	toggleClass = () => {
		const currentState = this.state.active;
        this.setState({ active: !currentState });
	}

	handleClick = () => {
		const currentState = this.state.seatType;
		switch(currentState) {
			case "available":
				this.setState({seatType: "reserved"}); 
				break;
			case "reserved":
				this.setState({seatType: "accessible"}); 
				break;
			case "accessible":
				this.setState({seatType: "disabled"}); 
				break;
			case "disabled":
				this.setState({seatType: "available"}); 
				break;
			default:
				this.setState({seatType: "disabled"}); 
		  } 
	}
	
	render() {
		return (
			<Button className={this.state.seatType} onClick = {this.handleClick}>Seat</Button>
		);
	}
}
	
