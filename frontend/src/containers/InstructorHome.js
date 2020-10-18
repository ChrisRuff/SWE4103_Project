import React, { Fragment, useState } from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Button, Dropdown, DropdownButton } from "react-bootstrap";
import Paper from "@material-ui/core/Paper";
import Grid from "@material-ui/core/Grid";
import "./InstructorHome.css";
import { Grow } from "@material-ui/core";

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
    const rows = [];
    const cols = [];

    for (var i = 0; i < numRows; i++) {
      rows.push(
          <div className="seat">
          <Paper className={classes.paper}>Seat</Paper>
          </div>
      );
    }
    for (var j = 0; j < numCols; j++) {
      cols.push(
        <Grid item className="row" col={j} xs={12} spacing={3}>
          {rows}
        </Grid>
      );
    }
    layout.push(
      <div className="root">
        <div className="outerLayout">
        <Grid container spacing={3}>
          {cols}
        </Grid>
        </div>
      </div>
    );
    return layout;
  };

  const layout = createLayout(25, 25);

  return (
    <div>
      <div className="layoutHeader">
        <DropdownButton
          id="dropdown-basic-button"
          title="Dropdown button"
        ></DropdownButton>
        <Button variant="light">Add</Button>
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
