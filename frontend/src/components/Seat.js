import React, { Component } from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Button, Dropdown, DropdownButton } from "react-bootstrap";
import Paper from "@material-ui/core/Paper";


export default class Seat extends Component {

	constructor(props) {
		super(props);
		this.y = {}
		this.x = {}
	}

	handleClick = () => {
		console.log(this.props);
	}
	
	render() {
		return (
			<Paper onClick = {() => this.handleClick()}>yes</Paper>
		);
	}
}
	
