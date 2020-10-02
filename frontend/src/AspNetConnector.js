
export var AspNetConnector = {
		
		callExampleEndpointFetch: function() {
				
				return fetch('api/apiexample/get')
				.then(res => res.json())
				.then((data) =>{
					return data;
				})
        .catch(function(error) {
					console.log('Request Failed', error);
				})
		}
		

		//Synchronous example, use only if needed and don't run on the main thread
		/*
		callExampleEndpointXML: function() {
			var request = new XMLHttpRequest();
			request.open('GET', 'api/apiexample/get', false);  // `false` makes the request synchronous
			request.send();
 
			if (request.status === 200) {// That's HTTP for 'ok'
  			return JSON.parse(request.response);
			}	
		}
		*/

}

