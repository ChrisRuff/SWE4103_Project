
export var StateManager = {
	accountState: null,
	exampleData: null,
	classLayout: null,
	selectedClass: "--",
	selectedSeat: null,
	trackingMode: false,
	submitted: false,
	absentSeats: [],
	prof: null,
	student: null,
	x: 0,
	y: 0,
	numRows: 0,
	numCols: 0,
	seats: [],
	mandatoryStatus: null,
	isEditing: null,
	wipe() {
		this.classLayout = null;
		this.accountState = null;
		this.selectedClass = "--";
		this.prof = null;
		this.submitted = false;
		this.student = null;
		this.numRows = 0;
		this.numCols = 0;
		this.trackingMode = false;
		this.wipeSeats();
	},
	setSubmitted(s) {
		this.submitted = s;
	},
	addAbsentSeat(seat)
	{
		this.absentSeats.push(seat.state.name);
	},
	getAbsentSeats()
	{
		return this.absentSeats;
	},
	removeAbsentSeat(seat)
	{
		for(let i = 0; i < this.absentSeats.length; ++i)
		{
			if(this.absentSeats[i].name == seat.name)
			{
				this.absentSeats.splice(i, 1);
			}
		}
	},
	isSubmitted() {
		return this.submitted;
	},
	setTrackingMode(m) {
		this.trackingMode = m;
	},
	getTrackingMode() {
		return this.trackingMode;
	},
	setExampleData(data) {
		this.exampleData = data;
	},
	getExampleData() {
		return this.exampleData;
	},
	getProf() {
		return this.prof;
	},
	setProf(prof) {
		this.prof = prof;
	},
	getStudent() {
		return this.student;
	},
	setStudent(student) {
		this.student = student;
	},
	setAccountState(state) {
		this.accountState = state;
	},
	getAccountState() {
		return this.accountState;
	},
	setSelectedClass(data){
		this.selectedClass = data;
	},
	getSelectedClass(){
		return this.selectedClass;
	},
	setClassLayout(layout){
		this.classLayout = layout;
	},
	getClassLayout(){
		return this.classLayout
	},
	addSeat(x, y, seatType, seat) {
		this.seats.push({"seatType": seatType, "x": x, "y": y, "seat": seat});


		if(this.seats.length > this.numCols * this.numRows)
			this.seats.pop(this.numCols * this.numRows);
	},
	changeSeatType(x, y, seatType) {
		var seatLoc = (this.numCols * y) + x; 
		this.seats[seatLoc].seatType = seatType;
	},
	getSeat(x,y) {
		console.log("Seats");
		console.log(this.seats);
		for (var i=0; i<=this.seats.length; i++) {
			if (this.seats[i].x === x && this.seats[i].y === y)
				return this.seats[i].seat
		}
	},
	wipeSeats()
	{
		this.seats = []
		this.x = 0;
		this.y = 0;
	},
	getX() {
		return this.x;
	},
	incX() {
		if(this.x === this.numCols - 1) {
			this.x = 0;
			this.y += 1;
		}
		else 
			this.x += 1;
	},
	getY() {
		return this.y;
	},
	setY(num) {
		this.y = num;
	},
	setRows(numRows) {
		this.numRows = numRows;
	},
	getRows(){
		return this.numRows;
	},
	setCols(numCols) {
		this.numCols = numCols;
	},
	getCols(){
		return this.numCols;
	},
	getSeats() {
		return this.seats;
	},
	setMandatoryStatus(mandatoryStatus) {
		this.mandatoryStatus = mandatoryStatus
	},
	getMandatoryStatus() {
		return this.mandatoryStatus
	},
	setSelectedSeat(seat) {
		this.selectedSeat = seat;
	},
	getSelectedSeat() {
		return this.selectedSeat;
	},
	getIsEditing(){
		return this.isEditing;
	},
	setIsEditing(isEditing){
		this.isEditing = isEditing; 
	}

}
