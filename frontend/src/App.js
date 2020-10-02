import React from 'react';
import logo from './logo.svg';
import './App.css';
import { AspNetConnector } from './AspNetConnector.js';
import { StateManager } from './StateManager.js'; 

function App() {

	// ###########################################################
	// How to call an enpoint and print it to the web console
	// Press Ctrl-Shift-C then click console on Windows/Linux
	// or Setings -> More Tools -> Developer Tools then click console
	
	// Asynchronous call using fetch
	AspNetConnector.callExampleEndpointFetch().then(data => { console.log(data[0].message) });
	
	/* Synchronous call using XMLHttpRequest
	var res = AspNetConnector.callExampleEndpointXML();
	console.log(res[0].message);
	*/
	
	// Storing data in the StateManager is an easy way to pass data from
	// one react component to another
	StateManager.setExampleData("Data to be used in another component.");
	
	var exData = StateManager.getExampleData();
	console.log(exData);
	// ###########################################################

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.js</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
    </div>
  );
}

export default App;
