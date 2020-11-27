import React from "react";
import { Route, Switch } from "react-router-dom";
import UnauthenticatedHome from "./containers/UnauthenticatedHome";
import NotFound from "./containers/NotFound";
import Login from "./containers/Login";
import Signup from "./containers/Signup";
import StudentHome from "./containers/StudentHome";
import InstructorHome from "./containers/InstructorHome";
import Roster from "./containers/Roster";

// These routes point to different pages within our website
// ex. To look at the 'Login' page (Login.js), go to https://localhost:5001/login

export default function Routes() {
  return (
    <Switch>
      <Route exact path="/">
        <UnauthenticatedHome />
      </Route>
      <Route exact path="/StudentHome">
        <StudentHome />
      </Route>
      <Route exact path="/InstructorHome">
        <InstructorHome />
      </Route>
      <Route exact path="/Roster">
        <Roster />
      </Route>
      <Route exact path="/login">
        <Login />
      </Route>
      <Route exact path="/signup">
        <Signup />
      </Route>
      {/* Finally, catch all unmatched routes */}
      <Route>
        <NotFound />
      </Route>
    </Switch>
  );
}
