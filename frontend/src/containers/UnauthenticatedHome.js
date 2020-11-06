import React from "react";
import "./UnauthenticatedHome.css";
import { useHistory } from "react-router-dom";

export default function UnauthenticatedHome() {
	const history = useHistory();
	if(localStorage.getItem("user") != null)
	{
		history.push("/InstructorHome"); 
	}
  return (
    <div className="UnauthenticatedHome">
      <div className="lander">
        <h1>Attendance Tracker</h1>
        <p>An app for SWE4103</p>
      </div>
    </div>
  );
}
