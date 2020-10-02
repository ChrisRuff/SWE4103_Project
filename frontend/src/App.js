import React from 'react';
import logo from './logo.svg';
import './App.css';
import { AspNetConnector } from './AspNetConnector.js';

function App() {

	// ###########################################################
	// TODO: Create StateManager
	//				- A place to set data that may be useful in other 
	//					 components
	
	// How to call an enpoint and print it to the web console
	// Press Ctrl-Shift-C then click console on Windows/Linux
	// or Setings -> More Tools -> Developer Tools then click console
	
	// Asynchronous call using fetch
	AspNetConnector.callExampleEndpointFetch().then(data => { console.log(data[0].message) });
	
	// Synchronous call using XMLHttpRequest
	var res = AspNetConnector.callExampleEndpointXML();
	console.log(res[0].message);
	
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
