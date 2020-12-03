import React, { Fragment, useState} from "react";
import { Button, DropdownButton, MenuItem} from "react-bootstrap";
import { AspNetConnector } from "../AspNetConnector.js";
import Grid from "@material-ui/core/Grid";
import "./InstructorHome.css";
import { StateManager } from "../StateManager.js";
import Seat from "../components/Seat.js";
import { useHistory } from "react-router-dom";
import { TextField } from "@material-ui/core";
import Legend from "../components/Legend";
import copy from 'copy-to-clipboard';

// Material UI imports for roster view
import { withStyles, makeStyles } from '@material-ui/core/styles';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import StyledTableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';
import PropTypes from 'prop-types'
import { useTheme } from '@material-ui/core/styles';
import TableFooter from '@material-ui/core/TableFooter';
import TablePagination from '@material-ui/core/TablePagination';

// Material UI imports for attendance view
import ButtonMatUI from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import IconButton from '@material-ui/core/IconButton';
import Typography from '@material-ui/core/Typography';
import CloseIcon from '@material-ui/icons/Close';
import Slide from '@material-ui/core/Slide';

// Material UI imports for date and time picker
import DateFnsUtils from '@date-io/date-fns'; // choose your lib
import {
  DateTimePicker,
  MuiPickersUtilsProvider,
} from '@material-ui/pickers';

export default function InstructorHome() {

	const history = useHistory();
	const [attendancePopup, setAttendancePopup] = useState(false);
	const [selectedDate, handleDateChange] = useState(new Date());
	const [rosterPopup, setRosterPopup] = useState(false);
	const [linkShown, setLinkShown] = useState(false);

	let rosterStudents = null;
	if(StateManager.getClassLayout() != null) {
		rosterStudents = StateManager.getClassLayout().roster;
	}

	// If there is no prof object(not signed in) then return to the homepage
	if(StateManager.getProf() === null)
	{
		StateManager.setProf(JSON.parse(localStorage.getItem('user')));
		if(StateManager.getProf() === null)
		{
			history.push("/");
		}
	}

	const cs = AspNetConnector.getAllClasses();
	const [title, setTitle] = useState(StateManager.getSelectedClass());

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
				<Seat key={i} x={i} y={j} name={""}/>
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

	const [layout, setLayout] = useState(StateManager.getClassLayout() === null ? createLayout(5,5) : StateManager.getClassLayout());
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
				let type = "";
				let name = "";
				let email = "";
				for(let k = 0; k < classDTO.disabledSeats.length; ++k)
				{
					if(classDTO.disabledSeats[k].x === i &&
					classDTO.disabledSeats[k].y === j )
					{
						type = "disabledSeat"
					}
				}
				for(let k = 0; k < classDTO.openSeats.length; ++k)
				{
					if(classDTO.openSeats[k].x === i &&
					classDTO.openSeats[k].y === j )
					{
						type = "open"
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
				for(let k = 0; k < classDTO.reservedSeats.length; ++k)
				{
					if(classDTO.reservedSeats[k].x === i &&
						classDTO.reservedSeats[k].y === j )
					{
						name = classDTO.reservedSeats[k].name;
						email = classDTO.reservedSeats[k].email;
						type = "reserved"
					}
				}
				// Add seat with specified seat type
				cols.push(
					<div key={i} className="seat">
						<Seat x={i} y={j} seatType={type} email={email} name={name}/>
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
		var cols;
		var rows;

		if(layout[0].props === undefined)
		{
			cols = layout[0][0].props.children.props.children[0].props.children.length;
			rows = layout[0][0].props.children.props.children.length;
		}
		else
		{
			cols = layout[0].props.children.props.children[0].props.children.length;
			rows = layout[0].props.children.props.children.length;
		}

		var className = title;
		var newClass = [{"className": className, "height": rows, "width": cols}];
		let response = AspNetConnector.makeClass(newClass);
		if(response[0].response === false) {
			AspNetConnector.editClasses(newClass);
		}
		AspNetConnector.profAddClass([{"email": StateManager.getProf().email, "classes" : [{"className": title}]}]);
		addSeats();
		StateManager.setSubmitted(true);
	}
	
	const addSeats = () => {
		var currentLayout = StateManager.getSeats();
		AspNetConnector.wipeSeats([{"className": title}]);

		for( var i=0; i<currentLayout.length; i++){
			let classDTO = [{
				"className": title,
				"seat": null
			}]

			if(currentLayout[i].seatType === "open"){
				var openSeat = {"x": currentLayout[i].x, "y": currentLayout[i].y}
				classDTO[0].seat = openSeat;

				AspNetConnector.makeSeatOpen(classDTO);
			}
			else if(currentLayout[i].seatType === "disabledSeat"){
				var disabledSeat = {"x": currentLayout[i].x, "y": currentLayout[i].y}
				classDTO[0].seat = disabledSeat;

				AspNetConnector.disableSeat(classDTO);
			}
			else if(currentLayout[i].seatType === "accessible"){
				var accessibleSeat = {"x": currentLayout[i].x, "y": currentLayout[i].y};
				classDTO[0].seat = accessibleSeat;

				AspNetConnector.makeSeatAccessible(classDTO);
			}
			else if(currentLayout[i].seatType === "reserved"){
				var reservedSeat = {"x": currentLayout[i].x, "y": currentLayout[i].y, "email": currentLayout[i].seat.state.email};
				classDTO[0].seat = reservedSeat;

				AspNetConnector.reserveSeat(classDTO);
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
		StateManager.setSubmitted(true);
	}
	const newClass = () =>
	{
		let name = prompt("New Class Name (Enter section as well e.g. SWE4103_FR01A)");
		
		StateManager.setSubmitted(false);
		for(let i = 0; i < cs.length; ++i)
		{
			if(cs[i] === name)
			{
				alert("Class already exists");
				return;
			}
		}
		if (name !== null){
			let newLayout = createLayout(5,5);
			StateManager.setClassLayout(newLayout);
			setLayout(StateManager.getClassLayout());
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
			document.getElementById("link-field").value=`https://${url[2]}/Login?code=${response[0].classCode}`;
		}
		setLinkShown(true);
	}

	const copyLink = () => {
		var link = document.getElementById("link-field").value;
		copy(link);
	}

	if (StateManager.getClassLayout() === null){
		if (classList[0] !== null && classList[0] !== undefined){
			let classLayout = JSON.parse(AspNetConnector.getClasses([{"className": classList[0]}]).response);
			if(classLayout[0].response){
				StateManager.setSelectedClass(classList[0]);
				StateManager.setClassLayout(classLayout[0]);
				setTitle(StateManager.getSelectedClass());
				setLayout(loadLayout(classLayout[0]));
				StateManager.setSubmitted(true);
			}
			else
			{
				alert("Could not load class");
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
		if(StateManager.getSelectedClass() !== null && StateManager.getSelectedClass() !== "--") {
			var currentClass = [{"className": StateManager.getSelectedClass()}]
			AspNetConnector.removeClass(currentClass);

			if (classList.length === 0 ){
				setNoClasses(true)
			    StateManager.setSelectedClass("--");
			    setTitle("--");
			}
			window.location.reload(false);
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

	const [editing, setEditing] = useState(StateManager.getIsEditing() != null);
	var rowNum = StateManager.getRows();
	var colNum = StateManager.getCols();

	const showEditing = () =>
	{
		setEditing(true);
	}

	const changeSize = () =>
	{
		// Check if the user has entered a new row and column number
		if(StateManager.getRows() === rowNum && StateManager.getCols() === colNum) {
			window.alert("Please enter a new row and column number.");
		} else {
			// Confirm user's change
			let response = window.confirm("Are you sure you want to change the seat plan?");

			if(response) {
				setLayout(layout => [createLayout(rowNum, colNum)]);
				StateManager.setClassLayout(layout);
				setEditing(false);
			}
		}
	}

	/*
	anything entered in width and height textfields get saved into rowNum and colNum
	that are going to be used later when the user hits the confirm button
	*/
	const handleRowTextFieldChange = x => {
		if(isNumeric(x.target.value))
			rowNum = x.target.value;
		else if (x.target.value !== "")
		{
			x.target.value = "";
			window.alert("Please enter only numeric values!")
		}
	}
	const handleColTextFieldChange = x => {
		if(isNumeric(x.target.value))
			colNum = x.target.value;
		else if (x.target.value !== "")
		{
			x.target.value = "";
			window.alert("Please enter only numeric values!")
		}
	}

	const cancelEditSeatPlan= () => {
		setEditing(false);
	}

	function isNumeric(str) {
		if (typeof str != "string") return false
		return !isNaN(str) && !isNaN(parseFloat(str))
	}
  
	const openAttendancePopup = () => {
		if(StateManager.isSubmitted())
		{
			setAttendancePopup(true);
			StateManager.setTrackingMode(true);
		}
		else
		{
			alert("You must create a class and submit it before tracking");
		}
	}
	const closeAttendancePopup = () => {
		setAttendancePopup(false);
		StateManager.setTrackingMode(false);
	}

	const markAbsents = () => {
		let absents = StateManager.getAbsentSeats();
		let obj = 
			[{
				"studentNames": absents, 
				"date": (selectedDate.getDate() + "/" + selectedDate.getMonth() + "/" + selectedDate.getFullYear()), 
				"className": title
			}];

		console.log(obj);
		AspNetConnector.AddAttendanceRoster(obj)
	}

	const saveAttendanceAndClose = () => {
		markAbsents();
		StateManager.setTrackingMode(false);
		setAttendancePopup(false);
		window.location.reload(false);
	}

	const Transition = React.forwardRef(function Transition(props, ref) {
		return <Slide direction="up" ref={ref} {...props} />;
	});

	// columns is used for roster view
	const columns = [
		{ field: 'studentName', headerName: 'Student Name', width: 130 },
		{ field: 'date', headerName: 'Date of Absence', width: 130 }
	];

	const closeRosterPopup = () => {
		setRosterPopup(false);
	}

	const openRosterPopup = () => {
		if(rosterStudents != null) {
			setRosterPopup(true);
		} else {
			window.alert("Your current class does not have a roster!");
		}
	}

	const useStyles1 = makeStyles((theme) => ({
		root: {
			flexShrink: 0,
			marginLeft: theme.spacing(2.5),
		},
	}));

	const useStyles2 = makeStyles({
		table: {
			minWidth: 500,
		},
	});

	const classes = useStyles2();
	const [page, setPage] = React.useState(0);
	const [rowsPerPage, setRowsPerPage] = React.useState(5);

	var emptyRows;
	if(rosterStudents != null) {
		emptyRows = rowsPerPage - Math.min(rowsPerPage, rosterStudents.length - page * rowsPerPage);
	}

	const translate = (parameter) => {
		if(!parameter[0].includes(",")) {
			// translate each day missed for each student
			var tempDate = "";
			for (let j = 0; j < parameter.length; j++) {
				tempDate = parameter[j].split("/");
				parameter[j] = tempDate[0];
				parameter[j] = parameter[j].concat("/");
				parameter[j] = parameter[j].concat(Number(tempDate[1]) + 1);
				parameter[j] = parameter[j].concat("/");
				parameter[j] = parameter[j].concat(tempDate[2]);
				if (j != parameter.length - 1) {
					parameter[j] = parameter[j].concat(", ");
				}
			}
		}
		return parameter;
	}

	const handleChangePage = (event, newPage) => {
		setPage(newPage);
	};

	const handleChangeRowsPerPage = (event) => {
		setRowsPerPage(parseInt(event.target.value, 10));
		setPage(0);
	};

	function TablePaginationActions(props) {
		const classes = useStyles1();
		const theme = useTheme();
		const { count, page, rowsPerPage, onChangePage } = props;

		const handleFirstPageButtonClick = (event) => {
			onChangePage(event, 0);
		};

		const handleBackButtonClick = (event) => {
			onChangePage(event, page - 1);
		};

		const handleNextButtonClick = (event) => {
			onChangePage(event, page + 1);
		};

		const handleLastPageButtonClick = (event) => {
			onChangePage(event, Math.max(0, Math.ceil(count / rowsPerPage) - 1));
		};

		function LastPageIcon(props) {
			return (
				<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M5.59 7.41L10.18 12l-4.59 4.59L7 18l6-6-6-6zM16 6h2v12h-2z" /></svg>
			);
		}

		function FirstPageIcon(props) {
			return (
				<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M18.41 16.59L13.82 12l4.59-4.59L17 6l-6 6 6 6zM6 6h2v12H6z" /></svg>
			);
		}

		function KeyboardArrowRight(props) {
			return (
				<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8.59 16.34l4.58-4.59-4.58-4.59L10 5.75l6 6-6 6z" /></svg>
			);
		}

		function KeyboardArrowLeft(props) {
			return (
				<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M15.41 16.09l-4.58-4.59 4.58-4.59L14 5.5l-6 6 6 6z" /></svg>
			);
		}

		return (
			<div className={classes.root}>
				<IconButton
					onClick={handleFirstPageButtonClick}
					disabled={page === 0}
					aria-label="first page"
				>
					{theme.direction === 'rtl' ? <LastPageIcon /> : <FirstPageIcon />}
				</IconButton>
				<IconButton onClick={handleBackButtonClick} disabled={page === 0} aria-label="previous page">
					{theme.direction === 'rtl' ? <KeyboardArrowRight /> : <KeyboardArrowLeft />}
				</IconButton>
				<IconButton
					onClick={handleNextButtonClick}
					disabled={page >= Math.ceil(count / rowsPerPage) - 1}
					aria-label="next page"
				>
					{theme.direction === 'rtl' ? <KeyboardArrowLeft /> : <KeyboardArrowRight />}
				</IconButton>
				<IconButton
					onClick={handleLastPageButtonClick}
					disabled={page >= Math.ceil(count / rowsPerPage) - 1}
					aria-label="last page"
				>
					{theme.direction === 'rtl' ? <FirstPageIcon /> : <LastPageIcon />}
				</IconButton>
			</div>
		);
	}

	TablePaginationActions.propTypes = {
		count: PropTypes.number.isRequired,
		onChangePage: PropTypes.func.isRequired,
		page: PropTypes.number.isRequired,
		rowsPerPage: PropTypes.number.isRequired,
	};

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
				<Button onClick={openAttendancePopup} variant="light" className="pull-right" style={{marginRight: "15px"}}>Take Attendance</Button>
				<Button onClick={openRosterPopup} variant="light" className="pull-right" style={{marginRight: "15px"}}>View Roster</Button>
			</div>
			<h4 style={{marginLeft: '25px'}}>Hello {StateManager.getProf().name}</h4>
		</div>
		{ (noClasses === false) &&
		<div>
			<div className="main">
				<Fragment>{layout}</Fragment>
				<Legend/>
			</div>
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
        style={{width: '400px', height: 'auto'}}
        defaultValue=""
        InputProps={{
          readOnly: true,
        }}
        />
		{
			(linkShown) &&
			<Button className="pull-right" onClick={copyLink} varient="light">Copy Link</Button>
		}
			</div>
		</div>
		}
		{
			(noClasses === true ) &&
			<div key="root" className="root">
				<h1 style= {{textAlign: 'center', padding: '50px' }}> There are no classes to display </h1>
			</div> 
		}
		{attendancePopup &&
			<Dialog fullScreen open={attendancePopup} onClose={closeAttendancePopup} TransitionComponent={Transition}>
				<AppBar style={{position: 'relative', color: '#cd5c5c'}}>
					<Toolbar>
						<IconButton className="AttendanceTitle" edge="start" onClick={closeAttendancePopup} aria-label="close">
						<CloseIcon />
						</IconButton>
						<Typography className="AttendanceTitle" variant="h3" style={{marginLeft: '15px', flex: 1}}>
							Attendance
						</Typography>
						<ButtonMatUI className="AttendanceTitle" autoFocus onClick={saveAttendanceAndClose}>
							save
						</ButtonMatUI>
					</Toolbar>
				</AppBar>
					<MuiPickersUtilsProvider utils={DateFnsUtils}>
						<DateTimePicker value={selectedDate} onChange={handleDateChange} />
					</MuiPickersUtilsProvider>
				{ (noClasses === false) &&
					<div className="Attendance">
						{StateManager.setY(0)}
						<Fragment>{layout}</Fragment>
						<Legend/>
					</div>
				}
			</Dialog>
		}
		{rosterPopup &&
			<Dialog fullScreen open={rosterPopup} onClose={closeRosterPopup} TransitionComponent={Transition}>
				<AppBar style={{ position: 'relative', color: '#cd5c5c' }}>
					<Toolbar>
						<IconButton className="RosterTitle" edge="start" onClick={closeRosterPopup} aria-label="close">
							<CloseIcon />
						</IconButton>
						<Typography className="RosterTitle" variant="h3" style={{ marginLeft: '15px', flex: 1 }}>
							Roster
								</Typography>
					</Toolbar>
				</AppBar>
				<TableContainer component={Paper}>
					<Table>
						<TableHead>
							<TableRow>
								<StyledTableCell style={{ fontSize: 30, color: 'black' }} align="left"><b>Student Name</b></StyledTableCell>
								<StyledTableCell style={{ fontSize: 30, color: 'black' }} align="left"><b>Date of Absence</b></StyledTableCell>
							</TableRow>
						</TableHead>
						<TableBody>
							{(rowsPerPage > 0
								? rosterStudents.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
								: rosterStudents
							).map((row) => (
								<TableRow key={row.studentName}>
									<StyledTableCell style={{ fontSize: 30, color: 'black' }} align="left">{row.studentName}</StyledTableCell>
									<StyledTableCell style={{ fontSize: 30, color: 'black' }} align="left">{translate(row.daysMissed)}</StyledTableCell>
								</TableRow>
							))}
						</TableBody>
						<TableFooter>
							<TableRow>
								<TablePagination
									rowsPerPageOptions={[5, 10, 25, { label: 'All', value: -1 }]}
									colSpan={2}
									count={rosterStudents.length}
									rowsPerPage={rowsPerPage}
									page={page}
									SelectProps={{
										inputProps: { 'aria-label': 'rows per page' },
										native: true,
									}}
									onChangePage={handleChangePage}
									onChangeRowsPerPage={handleChangeRowsPerPage}
									ActionsComponent={TablePaginationActions}
								/>
							</TableRow>
						</TableFooter>
					</Table>
				</TableContainer>
			</Dialog>
		}
	</div>
);
}
