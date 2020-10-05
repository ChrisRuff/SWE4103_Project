import React, { useState } from "react";
import { Link, useHistory } from "react-router-dom";
import { LinkContainer } from "react-router-bootstrap";
import { Nav, Navbar, NavItem } from "react-bootstrap";
import "./App.css";
import { AspNetConnector } from "./AspNetConnector.js";
import { StateManager } from "./StateManager.js";
import Routes from "./Routes";
import { AppContext } from "./libs/contextLib";

function App() {
  const [isAuthenticated, userHasAuthenticated] = useState(false);
  const history = useHistory();
	
	// Stephen's Notes
  // ###########################################################
  // How to call an enpoint and print it to the web console
  // Press Ctrl-Shift-C then click console on Windows/Linux
  // or Setings -> More Tools -> Developer Tools then click console

  // Asynchronous call using fetch
  AspNetConnector.callExampleEndpointFetch().then((data) => {
    console.log(data[0].message);
  });

  /* Synchronous call using XMLHttpRequest
	var res = AspNetConnector.callExampleEndpointXML();
	console.log(res[0].message);
	*/

  // Storing data in the StateManager is an easy way to pass data from
  // one react component to another
  StateManager.setExampleData("Data to be used in another component.");

  var exData = StateManager.getExampleData();
  console.log(exData);


	var students = [{
		"studentName": "cruff",
		"classNames": ["SWE4103"],
		"email": "cruffy_test@unb.net",
		"response": false
	}]

	// Add an array of student objects to the db
	students = AspNetConnector.addStudents(students);
  // ###########################################################

  function handleLogout() {
    // TODO: logout user using AspNetConnector
    userHasAuthenticated(false);
    history.push("/login");
  }

  return (
    <div className="App container">
      <Navbar fluid collapseOnSelect>
        <Navbar.Header>
          <Navbar.Brand>
            <Link to="/">Attendance Tracker</Link>
          </Navbar.Brand>
          <Navbar.Toggle />
        </Navbar.Header>
        <Navbar.Collapse>
          <Nav pullRight>
            {isAuthenticated ? (
              <NavItem onClick={handleLogout}>Logout</NavItem>
            ) : (
              <>
                <LinkContainer to="/signup">
                  <NavItem>Signup</NavItem>
                </LinkContainer>
                <LinkContainer to="/login">
                  <NavItem>Login</NavItem>
                </LinkContainer>
              </>
            )}
          </Nav>
        </Navbar.Collapse>
      </Navbar>
      <AppContext.Provider value={{ isAuthenticated, userHasAuthenticated }}>
        <Routes />
      </AppContext.Provider>
    </div>
  );
}

export default App;
