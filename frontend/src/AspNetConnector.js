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
		
	/*	Students datastructure see /Controllers/Models/StudentDTO.cs
	*
	*	students = [{
	*		studentName: "wow",
	*		classNames: ["cool"],
	*		email: "wowzers",
	*		response: false
	*	}]
	*/
	addStudents: function(students) {
			
		var request = new XMLHttpRequest();
        
		request.open('POST', 'api/student/add', true);
		request.setRequestHeader('Content-type', 'application/json');
		request.send(JSON.stringify(students));
			
		request.onload = function() {
			if (request.status === 200) { // That's HTTP for 'ok'
      				console.log(JSON.parse(request.response));
			}
		}
	}
}
