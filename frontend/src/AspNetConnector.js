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
	addSeats: function(students) {
			
		var request = new XMLHttpRequest();
        
		request.open('POST', 'api/student/seat/add', true);
		request.setRequestHeader('Content-type', 'application/json');
		request.send(JSON.stringify(students));
		
		return request;
	}	
}
