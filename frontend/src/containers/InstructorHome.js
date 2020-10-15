import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Button, DropdownButton } from "react-bootstrap";
import Paper from "@material-ui/core/Paper";
import Grid from "@material-ui/core/Grid";
import "./InstructorHome.css";

export default function InstructorHome() {
  const useStyles = makeStyles((theme) => ({
    root: {
      flexGrow: 1,
    },
    paper: {
      padding: theme.spacing(1),
      textAlign: "center",
      color: theme.palette.text.secondary,
    },
  }));

  const classes = useStyles();
  function FormRow() {
    return (
      <React.Fragment>
        <Grid row={1} item xs={1}>
          <Paper className={classes.paper}>Seat</Paper>
        </Grid>
        <Grid row={2} item xs={1}>
          <Paper className={classes.paper}>Seat</Paper>
        </Grid>
        <Grid row={3} item xs={1}>
          <Paper className={classes.paper}>Seat</Paper>
        </Grid>
        <Grid row={4} item xs={1}>
          <Paper className={classes.paper}>Seat</Paper>
        </Grid>
        <Grid row={5} item xs={1}>
          <Paper className={classes.paper}>Seat</Paper>
        </Grid>
        <Grid row={6} item xs={1}>
          <Paper className={classes.paper}>Seat</Paper>
        </Grid>
        <Grid row={7} item xs={1}>
          <Paper className={classes.paper}>Seat</Paper>
        </Grid>
        <Grid row={8} item xs={1}>
          <Paper className={classes.paper}>Seat</Paper>
        </Grid>
        <Grid row={9} item xs={1}>
          <Paper className={classes.paper}>Seat</Paper>
        </Grid>
        <Grid row={10} item xs={1}>
          <Paper className={classes.paper}>Seat</Paper>
        </Grid>
      </React.Fragment>
    );
  }

  return (
    <div>
      <div className="layoutHeader">
        <DropdownButton id="dropdown-basic-button" title="Dropdown button">
          
        </DropdownButton>
        <Button variant="light">Add</Button>
        <Button variant="light">Submit</Button>
      </div>
      <Grid className="seats" container spacing={2}>
        <Grid className="seats" col={1} container item xs={12} spacing={3}>
          <FormRow />
        </Grid>
        <Grid className="seats" col={2} container item xs={12} spacing={3}>
          <FormRow />
        </Grid>
        <Grid className="seats" col={3} container item xs={12} spacing={3}>
          <FormRow />
        </Grid>
        <Grid className="seats" col={4} container item xs={12} spacing={3}>
          <FormRow />
        </Grid>
        <Grid className="seats" col={5} container item xs={12} spacing={3}>
          <FormRow />
        </Grid>
        <Grid className="seats" col={6} container item xs={12} spacing={3}>
          <FormRow />
        </Grid>
        <Grid className="seats" col={7} container item xs={12} spacing={3}>
          <FormRow />
        </Grid>
        <Grid className="seats" col={8} container item xs={12} spacing={3}>
          <FormRow />
        </Grid>
        <Grid className="seats" col={9} container item xs={12} spacing={3}>
          <FormRow />
        </Grid>
        <Grid className="seats" col={10} container item xs={12} spacing={3}>
          <FormRow />
        </Grid>
      </Grid>
      <div className = "layoutFooter">
        <Button variant="light">Edit Seat Plan</Button>
        <Button variant="light">More Options...</Button>
      </div>
    </div>
  );
}
