import React, { Fragment, useState, useEffect } from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Button, Dropdown, DropdownButton, MenuItem} from "react-bootstrap";
import { AspNetConnector } from "../AspNetConnector.js";
import Grid from "@material-ui/core/Grid";
import "./InstructorHome.css";
import { StateManager } from "../StateManager.js"
import Seat from "../components/Seat.js";

export default function InstructorHome() {

	const [title, setTitle] = useState("--");
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

    let layout = [];
    let rows = [];

    for (var j = 0; j < numRows; j++) {

		let cols = [];
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

    let layout = [];
    let rows = [];

    for (var j = 0; j < classDTO.height; j++) {

	let cols = [];
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

	const makeClass = () => {
		deleteClass()
		var cols = layout[0].props.children.props.children[0].props.children.length;
		var rows = layout[0].props.children.props.children.length;
		var className = title;
		var newClass = [{"className": className, "height": cols, "width": rows}]
		AspNetConnector.makeClass(newClass);	
		addSeats();
	}
	const deleteClass = () => {
		var className = title;
		var newClass = [{"className": className}]
		AspNetConnector.removeClass(newClass);
	}
	
	const addSeats = () => {
		var currentLayout = StateManager.getSeats();

		for( var i=0; i<currentLayout.length; i++){
			let classDTO = [{
				"className": title,
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

	const prof = [{
		"name":"Dawn",
		"email":"dawn@unb.ca",
		"pass":"pass",
		"classes": [{"className": "SWE4103"}, {"className": "CS2043"}]
	}];
	AspNetConnector.addProf(prof);
	AspNetConnector.profAddClass(prof);

	let classList = JSON.parse(AspNetConnector.profGetClasses(prof).response);
	const handleSelect = (eventKey, event) => {
		StateManager.setSelectedClass(classList[eventKey]);
		setTitle(classList[eventKey]);
		let classLayout = JSON.parse(AspNetConnector.getClasses([{"className": classList[eventKey]}]).response);
		if(classLayout[0].response)
		{
			StateManager.setClassLayout(classLayout[0]);
			console.log(StateManager.getClassLayout());
			setLayout(loadLayout(classLayout[0]));
		}
	}
	const newClass = () =>
	{
		let name = prompt("New Class Name");
		setLayout(createLayout(5,5));
		StateManager.setSelectedClass(name);
		setTitle(name);
	}


return (
    <div>
		<div className="layoutHeader">
			<DropdownButton 
				title={StateManager.getSelectedClass()}
				id="classDropdown"
				onSelect={handleSelect.bind(this)}>
				{classList.map((opt, i) => (
					<MenuItem key={i} eventKey={i}>
						{opt}
					</MenuItem>
				))}
			</DropdownButton>);
        <Button onClick={newClass} variant="light">Add</Button>
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
