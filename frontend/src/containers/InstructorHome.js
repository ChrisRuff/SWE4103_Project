import React, { Fragment } from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Button, Dropdown, DropdownButton } from "react-bootstrap";
import Paper from "@material-ui/core/Paper";
import Grid from "@material-ui/core/Grid";
import "./InstructorHome.css";
import { Grow } from "@material-ui/core";
import Seat from "../components/Seat.js"

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
    const layout = [];
    const cols = [];

    for (var j = 0; j < numCols; j++) {

      const rows = [];
      for (var i = 0; i < numRows; i++) {
        rows.push(
          <div key={i} className="seat">
            <Seat key={i} x={i} y={j} />
          </div>
        );
      }
      
      cols.push(
        <Grid item key={j} className="row" col={j} xs={12}>
          {rows}
        </Grid>
      );
    }

    layout.push(
      <div key="root" className="root">
        <Grid container spacing={3}>
          {cols}
        </Grid>
      </div>
    );
    return layout;
  };

  const layout = createLayout(5, 5);

  return (
    <div>
      <div className="layoutHeader">
        <DropdownButton
          id="dropdown-basic-button"
          title="Dropdown button"
        ></DropdownButton>
        <Button width='min-content' height='min-content' variant="light">Add</Button>
        <Button variant="light">Submit</Button>
      </div>
      <Fragment>{layout}</Fragment>
      <div className="layoutFooter">
        <Button variant="light">Edit Seat Plan</Button>
        <Button variant="light">More Options...</Button>
      </div>
    </div>
  );
}
