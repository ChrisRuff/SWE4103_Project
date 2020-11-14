import React, { Fragment, useState} from "react";
import { Button, DropdownButton, MenuItem} from "react-bootstrap";
import { AspNetConnector } from "../AspNetConnector.js";
import Grid from "@material-ui/core/Grid";
import "./InstructorHome.css";
import { StateManager } from "../StateManager.js";
import Seat from "../components/Seat.js";
import { useHistory } from "react-router-dom";
import { TextField } from "@material-ui/core";

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

	const [layout, setLayout] = useState(StateManager.getClassLayout() == null ? createLayout(5,5) : StateManager.getClassLayout());
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

	const [noClasses, setNoClasses] = useState(classList.length === 0 && StateManager.getClassLayout() === null);

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
		let name = prompt("New Class Name (Enter section as well e.g. SWE4103_FR01A)");
		
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
			setNoClasses(false);
		}
	}

	function directToEditSeatPlanPage() {
		history.push("/EditSeatPlan");
	}

	const generateLink = () => {
		var selectedClass = [{
			"className": StateManager.getSelectedClass()
		}];
		var request = AspNetConnector.generateClassCode(selectedClass);
		request.onload = async function() {
			var response = await JSON.parse(request.response);
			var url = window.location.href.split("/");
			document.getElementById("link-field").value=`https://${url[2]}/StudentHome?code=${response[0].classCode}`;
		}
	}
	if (StateManager.getClassLayout() === null){
		if (classList[0] !== null && classList[0] !== undefined){
			setTitle(classList[0]);
			StateManager.setSelectedClass(classList[0]);
			let classLayout = JSON.parse(AspNetConnector.getClasses([{"className": classList[0]}]).response);
			if(classLayout[0].response){
				StateManager.setClassLayout(classLayout[0]);
				setLayout(loadLayout(classLayout[0]));
			}
		}
	}
	const moreOptions = (eventKey, event) =>
	{
		switch(eventKey)
		{
			case "notFreq":
				let input = 0;
				do
				{
					input = parseInt(prompt("New notification frequency"));
				}while(isNaN(input));
				AspNetConnector.changeNotificationFreq([{"className": StateManager.getSelectedClass(), "notificationFreq": input }]);
		}
	}

	/* 
	var newClass = [{"className": "CS1073"}]
	
	var request = AspNetConnector.removeClass(newClass);
	
	request.onload = function() {
	JSON.parse(request.response)
	}
	*/
	const removeClass = () =>
	{
		if(StateManager.getSelectedClass() != null && StateManager.getSelectedClass() != "--") {
			var currentClass = [{"className": StateManager.getSelectedClass()}]
			AspNetConnector.removeClass(currentClass);

			setNoClasses(true)
			StateManager.setSelectedClass("--");
			setTitle("--");
		}
		else 
			alert("Please select a class!")
	}

	const setMandatory = () =>
	{
		var currentClass = [{"className": StateManager.getSelectedClass()}]
		let fullClass = JSON.parse(AspNetConnector.getClasses(currentClass).response);
		
		if(fullClass[0].response === true)
		{
			if(fullClass[0].mandatory === false)
			{
				currentClass = [{"className": StateManager.getSelectedClass(), "mandatory": true}]
				let response = AspNetConnector.setMandatoryStatus(currentClass);
				alert("Mandatory class attendance set to: " + response[0].mandatory)
			}
			else
			{
				currentClass = [{"className": StateManager.getSelectedClass(), "mandatory": false}]
				let response = AspNetConnector.setMandatoryStatus(currentClass);
				alert("Mandatory class attendance set to: " + response[0].mandatory)
			}
		}
		else
			alert("Please save your class!")
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
		{ (noClasses === false) &&
		<div>
			<Fragment>{layout}</Fragment>
			<div className="layoutFooter">
				<Button onClick={directToEditSeatPlanPage} variant="light">Edit Seat Plan</Button>
				<DropdownButton 
						onSelect={moreOptions.bind(this)}
						title="More Options..."
						id="moreOptionsDropdown">
							<MenuItem key="remove" eventKey={"remove"} onClick={removeClass}>
								Remove Class
							</MenuItem>
							<MenuItem key="mandatory" eventKey={"mandatory"} onClick={setMandatory}>
								Set Mandatory
							</MenuItem>
							<MenuItem key="notificationFreq" eventKey={"notFreq"}>
								Change Notification Frequency: { StateManager.getClassLayout() != null ? StateManager.getClassLayout().notificationFreq : "" }
							</MenuItem>
					</DropdownButton>
        <Button className="pull-right" onClick={generateLink} variant="light">Generate Registration Link</Button>
        <TextField
        className="pull-right"
        id="link-field"
        style={{width: '250px', height: 'auto'}}
        defaultValue=""
        InputProps={{
          readOnly: true,
        }}
        />
			</div>
		</div>
		}
		{
			(noClasses === true ) &&
			<div key="root" className="root">
				<h1 style= {{textAlign: 'center', padding: '50px' }}> There are no classes to display </h1>
			</div> 
		}
    </div>
);
}