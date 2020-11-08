import React, { Fragment, useState, useEffect }  from "react";
import "./StudentHome.css";
import { makeStyles } from "@material-ui/core/styles";
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

    // useEffect's run every render, since there are no
    // dependants declared in this one, it will run only
    // on page load
    useEffect(() => {
        var url_string = window.location.href;
        var url = new URL(url_string);
        let code = url.searchParams.get("code");
        // If there is no student object(not signed in) then return to the homepage
        if(StateManager.getStudent() == null) {
            StateManager.setStudent(JSON.parse(localStorage.getItem('user')));
            if(StateManager.getStudent() == null) {
                if (code == null) {
                    history.push("/login");
                }
                else {
                    history.push(`/login?code=${code}`);
                }
            }
        }
        if (code != null && StateManager.getStudent() != null) {
			//get class from class code
			var newClass = [{
				"classCode": code
			}]
			var request = AspNetConnector.getClassCode(newClass);
			request.onload = async function() {
				var response = await JSON.parse(request.response);
				var classFromCode = response[0].className;
				// if valid class code, ask user if they would like to register
				if (classFromCode!=null) {
					let answer = window.confirm(`Would you like to register for ${classFromCode}?`);
					// if they would like to register, call addClassToStudent
					if (answer) {
						var student = [{
							"classes":[{"className": classFromCode}], 
							"email": StateManager.getStudent().email
						}];
						request = AspNetConnector.addClassToStudent(student);
						request.onload = async function() {
							response = await JSON.parse(request.response);
						}
					}
				}
				else {
					alert("Invalid registration link.");
				}
			}
			history.push("/StudentHome");
		}
    });

  const [title, setTitle] = useState("--");
	const useStyles = makeStyles((theme) => ({
		paper: {
			padding: theme.spacing(1),
			textAlign: "center",
			color: theme.palette.text.secondary,
		},
  }));

  let hash = sha512.sha512("abc");   //this needs to be changed later, works now only for a dummy student
	let student = [];
	try {
		student = JSON.parse(AspNetConnector.getStudents([{
			"email": "tester@tester.test",   //needs to be changed later
			"pass": hash,
		}]).response)[0];
	} catch (e) {
		onError(e);
	}
	let classList = [];
	if (student !== undefined) {
		for(let i = 0; i < student.classes.length; i++){
			classList.push(student.classes[i].className);
		}
	}

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
					<div key={i} className="studentSeat">
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

	let emptyLayout = [];
	if (StateManager.getClassLayout() === null){
		if (classList[0] !== null){
			StateManager.setSelectedClass(classList[0]);
			let classLayout = JSON.parse(AspNetConnector.getClasses([{"className": classList[0]}]).response);
			//StateManager.setClassLayout(classLayout[0]);
			if(classLayout[0].response){
				StateManager.setClassLayout(loadLayout(classLayout[0]));
				//setLayout(loadLayout(classLayout[0]));
			}
		}
		else {
			emptyLayout.push( //gives this statement if student has no classes
				<div key="root" className="root">
					<h1 style= {{textAlign: 'center', padding: '50px' }}> There are no classes to display </h1>
				</div>
				);
		}
	}

	const [layout, setLayout] = useState(StateManager.getClassLayout() == null ? emptyLayout : StateManager.getClassLayout());

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

  return (
    <div className="StudentHome">
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
        </DropdownButton>
          <Button variant="light">Submit</Button>
      </div>
	   <Fragment>{layout }</Fragment>
    </div>
  );
}
