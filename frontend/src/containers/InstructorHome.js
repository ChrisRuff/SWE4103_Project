import React, { Fragment } from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Button, Dropdown, DropdownButton } from "react-bootstrap";
import Grid from "@material-ui/core/Grid";
import "./InstructorHome.css";
import Seat from "../components/Seat.js";
import {AspNetConnector} from "../AspNetConnector.js";
import {StateManager} from "../StateManager.js";

export default function InstructorHome() {
  const useStyles = makeStyles((theme) => ({
    paper: {
      padding: theme.spacing(1),
      textAlign: "center",
      color: theme.palette.text.secondary,
    },
  }));

  const classes = useStyles();

  const createLayout = (numRows, numCols) => {
		StateManager.setRows(numRows);
		StateManager.setCols(numCols);

    const layout = [];
    const rows = [];

    for (var j = 0; j < numRows; j++) {

      const cols = [];
      for (var i = 0; i < numCols; i++) {
        cols.push(
          <div key={i} className="seat">
            <Seat key={i} x={i} y={j} />
          </div>
        );
      }
      
      rows.push(
        <Grid item key={j} className="row" col={j} xs={12}>
          {cols}
        </Grid>
      );
    }

    layout.push(
      <div key="root" className="root">
        <Grid container spacing={3}>
          {rows}
        </Grid>
      </div>
    );
    return layout;
  };

  const layout = createLayout(5, 6);

	const makeClass = () => {
		deleteClass()

		var cols = layout[0].props.children.props.children[0].props.children.length;
		var rows = layout[0].props.children.props.children.length;
		var className = "Test";
		var newClass = [{"className": className, "height": cols, "width": rows}]
		AspNetConnector.makeClass(newClass);	
	}
	
	const deleteClass = () => {
		var className = "Test";
		var newClass = [{"className": className}]
		AspNetConnector.removeClass(newClass);
		console.log(StateManager.getSeat(0,0));
	}

		

  return (
    <div>
      <div className="layoutHeader">
        <DropdownButton
          id="dropdown-basic-button"
          title="Dropdown button"
        ></DropdownButton>
        <Button width='min-content' height='min-content' variant="light">Add</Button>
        <Button onClick={makeClass} variant="light">Submit</Button>
      </div>
      <Fragment>{layout}</Fragment>
      <div className="layoutFooter">
        <Button variant="light">Edit Seat Plan</Button>
        <Button variant="light">More Options...</Button>
      </div>
    </div>
  );
}
