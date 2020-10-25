import React, { Fragment, useState, useEffect } from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Button, Dropdown, DropdownButton } from "react-bootstrap";
import Grid from "@material-ui/core/Grid";
import "./InstructorHome.css";
import Seat from "../components/Seat.js";
import {AspNetConnector} from "../AspNetConnector.js";
import {StateManager} from "../StateManager.js";

export default function InstructorHome() {
	const useStyles = makeStyles((theme) => ({
		paper: {
			padding: theme.spacing(1),
			textAlign: "center",
			color: theme.palette.text.secondary,
		},
	}));

	const createLayout = (numRows, numCols) => {
		StateManager.setRows(numRows);
		StateManager.setCols(numCols);

    const layout = [];
    const rows = [];

    for (var j = 0; j < numRows; j++) {

		const cols = [];
		for (var i = 0; i < numCols; i++) {
			cols.push(
			<div key={i} className="seat">
				<Seat key={i} x={i} y={j} />
			</div>
			);
		}
		
		rows.push(
			<Grid item key={j} className="row" col={j} xs={12}>
				{cols}
			</Grid>
		);
    }

    layout.push(
	<div key="root" className="root">
        <Grid container spacing={3}>
			{rows}
        </Grid>
	</div>
    );
    return layout;
	};

	const classes = useStyles();
	const [layout, setLayout] = useState(createLayout(5,6));


	
	const loadLayout = (classDTO) => {
		StateManager.wipeSeats();
		StateManager.setRows(classDTO.height);
		StateManager.setCols(classDTO.width);

    const layout = [];
    const rows = [];

    for (var j = 0; j < classDTO.height; j++) {

	const cols = [];
	for (var i = 0; i < classDTO.width; i++) {

		// Find type of seat from classDTO
		let type = ""
		for(let k = 0; k < classDTO.disabledSeats.length; ++k)
		{
			if(classDTO.disabledSeats[k].x === i &&
			classDTO.disabledSeats[k].y === j )
			{
				type = "disabled"
			}
		}
		for(let k = 0; k < classDTO.reservedSeats.length; ++k)
		{
			if(classDTO.reservedSeats[k].x === i &&
				classDTO.reservedSeats[k].y === j )
			{
				type = "reserved"
			}
		}
		for(let k = 0; k < classDTO.accessibleSeats.length; ++k)
		{
			if(classDTO.accessibleSeats[k].x === i &&
				classDTO.accessibleSeats[k].y === j )
			{
				type = "accessible"
			}
		}
		// Add seat with specified seat type
        cols.push(
			<div key={i} className="seat">
				<Seat x={i} y={j} seatType={type}/>
			</div>
        );
	}
	
	rows.push(
        <Grid item className="row" key={j} col={j} xs={12}>
			{cols}
        </Grid>
	);
    }

    layout.push(
		<div key="root" className="root">
			<Grid container spacing={3}>
				{rows}
			</Grid>
		</div>
    );
    return layout;
	};

	const test = () => {
		let ClassDTO = {
			"disabledSeats": [{"x": 1, "y": 1}], 
			"height": 5,
			"width": 3,
			"reservedSeats": [{"x": 2, "y": 1}],
			"accessibleSeats": [{"x": 2, "y": 2}]
		}

		setLayout(loadLayout(ClassDTO));

	}

	const makeClass = () => {
		deleteClass()
		var cols = layout[0].props.children.props.children[0].props.children.length;
		var rows = layout[0].props.children.props.children.length;
		var className = "TestClass";
		var newClass = [{"className": className, "height": cols, "width": rows}]
		AspNetConnector.makeClass(newClass);	
		addSeats();
	}
	
	const addSeats = () => {
		var currentLayout = StateManager.getSeats();

		for( var i=0; i<currentLayout.length; i++){
			let classDTO = [{
				"className": "TestClass",
				"seat": null
			}]

			if(currentLayout[i].seatType === "reserved"){
				var reservedSeat = {"x": currentLayout[i].x, "y": currentLayout[i].y}
				classDTO[0].seat = reservedSeat;

				AspNetConnector.reserveSeat(classDTO);
			}
			else if(currentLayout[i].seatType === "disabled"){
				var disabledSeat = {"x": currentLayout[i].x, "y": currentLayout[i].y}
				classDTO[0].seat = disabledSeat;

				AspNetConnector.disableSeat(classDTO);
			}
			/*
			else if(currentLayout[i].seatType == "accessible"){
				var seat = [{"x": currentLayout[i].x, "y": currentLayout[i].y}]
				classDTO.seat = seat;

				AspNetConnector.makeSeatAccessible(classDTO);
			}	
			*/
		}
	}

	const deleteClass = () => {
		var className = "TestClass";
		var newClass = [{"className": className}]
		AspNetConnector.removeClass(newClass);
	}

return (
    <div>
		<div className="layoutHeader">
			<DropdownButton
			id="dropdown-basic-button"
			title="Dropdown button"
			></DropdownButton>
			<Button onClick={test} width='min-content' height='min-content' variant="light">Add</Button>
			<Button onClick={makeClass} variant="light">Submit</Button>
		</div>
		<Fragment>{layout}</Fragment>
		<div className="layoutFooter">
			<Button variant="light">Edit Seat Plan</Button>
			<Button variant="light">More Options...</Button>
		</div>
    </div>
);
}
