import React, { Fragment, useState, useEffect }  from "react";
import "./StudentHome.css";
import { lighten, makeStyles } from "@material-ui/core/styles";
import { Button, Dropdown, DropdownButton, MenuItem} from "react-bootstrap";
import { StateManager } from "../StateManager.js";
import { useHistory } from "react-router-dom";
import { AspNetConnector } from "../AspNetConnector.js";
import Grid from "@material-ui/core/Grid"; 
import Seat from "../components/Seat.js";
import * as sha512 from "js-sha512";
import { onError } from "../libs/errorLib";

export default function StudentHome() {

  const history = useHistory();

	// If there is no student object(not signed in) then return to the homepage
	if(StateManager.getStudent() == null)
	{
		StateManager.setStudent(JSON.parse(localStorage.getItem('user')));
		if(StateManager.getStudent() == null)
		{
			history.push("/");
		}
  }
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
  
  //const classes = useStyles();
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

    //let classList = JSON.parse(AspNetConnector.profGetClasses([StateManager.getStudent()]).response);
	let hash = sha512.sha512("abc");
	let student = [];
	try {
		student = JSON.parse(AspNetConnector.getStudents([{
			"email": "tester@tester.test",
			"pass": hash,
		}]).response)[0];
	} catch (e) {
		onError(e);
	}
	let classList = [];
	if (student != undefined) {
		console.log(student)
		for(let i = 0; i < student.classes.length; i++){
			classList.push(student.classes[i].className);
		}
		console.log(classList);
	}
	/*const handleSelect = (eventKey, event) => {
		StateManager.setSelectedClass(classList[eventKey]);
		setTitle(classList[eventKey]);
    let classLayout = JSON.parse(AspNetConnector.getClasses([{"className": classList[eventKey]}]).response);
		if(classLayout[0].response)
		{
			StateManager.setClassLayout(classLayout[0]);
			console.log(StateManager.getClassLayout());
			setLayout(loadLayout(classLayout[0]));
		}
		console.log(StateManager.getSeats());
  }*/
  
  return (
    <div className="StudentHome">
      <div className="layoutHeader">
        <DropdownButton 
          title={StateManager.getSelectedClass()}
          id="classDropdown">
          {classList.map((opt, i) => (
            <MenuItem key={i} eventKey={i}>
              {opt}
            </MenuItem>
          ))}
        </DropdownButton>
          <Button variant="light">Submit</Button>
      </div>
      <Fragment>{layout}</Fragment>
    </div>
  );
}
