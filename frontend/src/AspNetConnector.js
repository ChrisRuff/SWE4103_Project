export var AspNetConnector = {
	// Asynchronous example
	callExampleEndpointFetch: function() {
			return fetch('api/apiexample/get').then(res => res.json()).then((data) => {
						return data;
			}).catch(function(error) {
						console.log('Request Failed', error);
			})
	},

	//Synchronous example, use only if needed and don't run on the main thread
	callExampleEndpointXML: function() {
			var request = new XMLHttpRequest();
			// `false` makes the request synchronous
			// if you leave false out feel free to use XMLHttp
			request.open('GET', 'api/apiexample/get', false);
			request.send();
			if (request.status === 200) { // That's HTTP for 'ok'
						return JSON.parse(request.response);
			}
	},

	/* Sample call
	 *
	 * var students = [{
   * 	"studentName": "cruff",
   * 	"classes":[{"className": "SWE4103", "seat":{"x":1,"y":1}}], 
   * 	"email": "cruffy_test@unb.net",
	 * 	"password": "password",
   * 	"response": false
   * }]
	 *
	 * var request = DatabaseConnector.Connector.AddStudent(student(s));
	 *
	 *	request.onload = function() {
	 *		console.log(JSON.parse(request.response));
	 *	}
	 */
	// Adds a student to the DB
	addStudents: function(students) {
			
		var request = new XMLHttpRequest();
        
		request.open('POST', 'api/student/add', true);
		request.setRequestHeader('Content-type', 'application/json');
		request.send(JSON.stringify(students));
		
		return request;
			
	},

	/* 
	* var newSeat = [{
	* 	"studentName": "zlewis",
	* 	"classes": [{"className": "CS1073", "seat": {"x": 4,"y": 7}}],
	* 	"email": "zlewis_test@unb.net",
	* 	"password": "password1",
	* 	"response": false
	* }]
	*
	* var request = AspNetConnector.addSeats(newSeat);
	* 
	* request.onload = function() {
	* 	JSON.parse(request.response)
	* }
	*/
	// Adds a seat to the DB
	addSeat: function(students) {
			
		var request = new XMLHttpRequest();
        
		request.open('POST', 'api/student/seat/add', true);
		request.setRequestHeader('Content-type', 'application/json');
		request.send(JSON.stringify(students));
		
		return request;
	},

/* 
	 var students = [{
    	"studentName": "cruff",
    	"classes":[{"className": "SWE4103", "seat":{"x":1,"y":1}}], 
    	"email": "cruffy_test@unb.net",
	  	"pass": "password",
    	"response": false
    }]
	 
	  //AspNetConnector.addStudents(students);
	 
	  var request = AspNetConnector.addSeat(students);
	  var request = AspNetConnector.getSeat(students);
	 
	 	request.onload = function() {
	 		console.log(JSON.parse(request.response));
	 	}
	*/
	// Gets a seat from the DB
	getSeat: function(students) {
			
		var request = new XMLHttpRequest();
        
		request.open('POST', 'api/student/seat/get', true);
		request.setRequestHeader('Content-type', 'application/json');
		request.send(JSON.stringify(students));
		
		return request;
	},
		
	/* Sample call
	 var student = [{
    	"studentName": "cruff",
    	"classes":[{"className": "SWE4103", "seat":{"x":1,"y":1}}], 
    	"email": "cruffy_test@unb.net",
	  	"password": "password",
    	"response": false
    }]
	 
	  var request = AspNetConnector.getStudents(student);
	 
	 	request.onload = function() {
	 		console.log(JSON.parse(request.response));
	 	}
	 */
	// get student
	getStudents: function(students) {
			
		var request = new XMLHttpRequest();
        
		request.open('POST', 'api/student/get', true);
		request.setRequestHeader('Content-type', 'application/json');
		request.send(JSON.stringify(students));
		
		return request;
			
	},

	/* 
	 var newClass = [{"className": "CS1073", "height": 25, "width": 50}]
	
	 var request = AspNetConnector.makeClass(newClass);
	 
	 request.onload = function() {
	 	JSON.parse(request.response)
	 }
	*/
	// makes class
	makeClass: function(students) {
			
		var request = new XMLHttpRequest();
        
		request.open('POST', 'api/class/add', true);
		request.setRequestHeader('Content-type', 'application/json');
		request.send(JSON.stringify(students));
		
		return request;
	},

	/* 
	 var newClass = [{"className": "SWE4103", "seat":{"x":1,"y":1}}]
	
	 var request = AspNetConnector.disableSeat(newClass);
	 
	 request.onload = function() {
	 	JSON.parse(request.response)
	 }
	*/
	// Disable a seat
	disableSeat: function(students) {
			
		var request = new XMLHttpRequest();
        
		request.open('POST', 'api/student/seat/disable', true);
		request.setRequestHeader('Content-type', 'application/json');
		request.send(JSON.stringify(students));
		
		return request;
	},

	/* 
   var newClass = [{"className": "CS1073"}]
	
	 var request = AspNetConnector.removeClass(newClass);
	 
	 request.onload = function() {
	 	JSON.parse(request.response)
	 }
	*/
	// remove a class
	removeClass: function(students) {
			
		var request = new XMLHttpRequest();
        
		request.open('POST', 'api/student/class/remove', true);
		request.setRequestHeader('Content-type', 'application/json');
		request.send(JSON.stringify(students));
		
		return request;
	},

	/* 
	 var newClass = [{"className": "SWE4103", "seat":{"x":1,"y":1}}]
	
	 var request = AspNetConnector.reserveSeat(newClass);
	 
	 request.onload = function() {
	 	JSON.parse(request.response)
	 }
	*/
	// reserve a seat
	reserveSeat: function(students) {
			
		var request = new XMLHttpRequest();
        
		request.open('POST', 'api/student/seat/reserve', true);
		request.setRequestHeader('Content-type', 'application/json');
		request.send(JSON.stringify(students));
		
		return request;
	}
}