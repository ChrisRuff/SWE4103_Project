import React from 'react';
import logo from './logo.svg';
import './App.css';

function App() {

	// ###########################################################
	// TODO: Create StateManager
	//				- A place to set data that may be useful in other 
	//					 parts of the program
	//        - A place to get data from other parts of the program

	// TODO: Create AspNetConnector
	//				- A universal connector to the backend containing
	//					 functions to get data from all endpoints
	
	// How to call an enpoint and print it to the web console
	// Press Ctrl-Shift-C then click console on Windows/Linux
	// or Setings -> More Tools -> Developer Tools then click console
	fetch('apiexample/get')
        .then(res => res.json())
        .then((data) => {
					console.log(data) 
				})
        .catch()

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
