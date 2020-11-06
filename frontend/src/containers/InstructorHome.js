import React, { Fragment, useState, useEffect } from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Button, Dropdown, DropdownButton, MenuItem} from "react-bootstrap";
import { AspNetConnector } from "../AspNetConnector.js";
import Grid from "@material-ui/core/Grid";
import "./InstructorHome.css";
import { StateManager } from "../StateManager.js";
import Seat from "../components/Seat.js";
import { useHistory } from "react-router-dom";

export default function InstructorHome() {

	const history = useHistory();

	// If there is no prof object(not signed in) then return to the homepage
	if(StateManager.getProf() == null)
	{
		StateManager.setProf(JSON.parse(localStorage.getItem('user')));
		if(StateManager.getProf() == null)
		{
			history.push("/");
		}
	}

	const cs = AspNetConnector.getAllClasses();
	const [title, setTitle] = useState("--");
	const useStyles = makeStyles((theme) => ({
		paper: {
			padding: theme.spacing(1),
			textAlign: "center",
			color: theme.palette.text.secondary,
		},
	}));

	const createLayout = (numRows, numCols) => {
		StateManager.wipeSeats();
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

	let classList = JSON.parse(AspNetConnector.profGetClasses([StateManager.getProf()]).response);
	let emptyLayout = [];

	const classes = useStyles();
	const [layout, setLayout] = useState(StateManager.getClassLayout() === null ? emptyLayout : StateManager.getClassLayout());

	const makeClass = () => {
		if(title === "--")
		{
			return;
		}
		var cols = layout[0].props.children.props.children[0].props.children.length;
		var rows = layout[0].props.children.props.children.length;
		var className = title;
		var newClass = [{"className": className, "height": cols, "width": rows}];
		AspNetConnector.makeClass(newClass);
		AspNetConnector.profAddClass([{"email": StateManager.getProf().email, "classes" : [{"className": title}]}]);
		addSeats();
	}
	
	const addSeats = () => {
		var currentLayout = StateManager.getSeats();
		AspNetConnector.wipeSeats([{"className": title}]);

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
			else if(currentLayout[i].seatType === "accessible"){
				var accessibleSeat = {"x": currentLayout[i].x, "y": currentLayout[i].y};
				classDTO[0].seat = accessibleSeat;

				AspNetConnector.makeSeatAccessible(classDTO);
			}
		}
	}

	const handleSelect = (eventKey, event) => {
		StateManager.setSelectedClass(classList[eventKey]);
		setTitle(classList[eventKey]);
		let classLayout = JSON.parse(AspNetConnector.getClasses([{"className": classList[eventKey]}]).response);
		if(classLayout[0].response)
		{
			StateManager.setClassLayout(classLayout[0]);
			setLayout(loadLayout(classLayout[0]));
		}
	}
	const newClass = () =>
	{
		let name = prompt("New Class Name");
		
		for(let i = 0; i < cs.length; ++i)
		{
			if(cs[i] === name)
			{
				alert("Class already exists");
				return;
			}
		}
		if (name !== null){
			setLayout(createLayout(5,5));
			StateManager.setSelectedClass(name);
			setTitle(name);
		}
	}

	function directToEditSeatPlanPage() {
		history.push("/EditSeatPlan");
	}

	if (StateManager.getClassLayout() === null){
		if (classList[0] !== null && classList[0] !== undefined){
			setTitle(classList[0]);
			StateManager.setSelectedClass(classList[0]);
			let classLayout = JSON.parse(AspNetConnector.getClasses([{"className": classList[0]}]).response);
			if(classLayout[0].response){
				console.log(classLayout[0]);
				StateManager.setClassLayout(classLayout[0]);
				setLayout(loadLayout(classLayout[0]));
			}
		}
		else {
			emptyLayout.push( //gives this statement if prof has no classes
				<div key="root" className="root">
					<h1 style= {{textAlign: 'center', padding: '50px' }}> There are no classes to display </h1>
				</div> 
			);
			return emptyLayout;
		}
	}

return (
    <div>
		<div className="layoutHeader">
			<div style={{width: "100%"}}>
				<DropdownButton 
					title={StateManager.getSelectedClass()}
					id="classDropdown"
					onSelect={handleSelect.bind(this)}>
					{classList.map((opt, i) => (
						<MenuItem key={i} eventKey={i}>
							{opt}
						</MenuItem>
					))}
				</DropdownButton>
				<div style={{width: "15px", height: "auto", display: "inline-block"}}/>
				<Button onClick={newClass} variant="light">Add</Button>
				<Button onClick={makeClass} variant="light" className="pull-right">Submit</Button>
			</div>
		</div>
		<Fragment>{layout}</Fragment>
		<div className="layoutFooter">
			<Button onClick={directToEditSeatPlanPage} variant="light">Edit Seat Plan</Button>
			<Button variant="light">More Options...</Button>
		</div>
    </div>
);
}
