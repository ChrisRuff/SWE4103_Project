import React, { Fragment, useState } from "react";
import Seat from "../components/Seat.js";
import Grid from "@material-ui/core/Grid";
import TextField from "@material-ui/core/TextField";
import { Button } from "react-bootstrap";
import { useHistory } from "react-router-dom";
import { AspNetConnector } from "../AspNetConnector.js";
import { StateManager } from "../StateManager.js";
import "./EditSeatPlan.css";

export default function EditSeatPlan() {
  // function taken from InstructorHome.js to create seating plan layout
  const createLayout = (numRows, numCols) => {
		StateManager.wipeSeats();
		StateManager.setRows(numRows);
		StateManager.setCols(numCols);

    let editingLayout = [];
    let rows = [];

    for (var j = 0; j < numRows; j++) {
      let cols = [];

      for (var i = 0; i < numCols; i++) {
        cols.push(
          <div key={i} className="seat">
            <Seat key={i} x={i} y={j}/>
          </div>
        );
      }
      
      rows.push(
        <Grid item key={j} className="row" col={j} xs={12}>
          {cols}
        </Grid>
      );
    }

    editingLayout.push(
      <div key="root" className="root">
        <Grid container spacing={3}>
          {rows}
        </Grid>
      </div>
    );

    return editingLayout;
	};

  const classSelected = StateManager.getSelectedClass();
  const history = useHistory();
  
  var rowNum = StateManager.getRows();
  var colNum = StateManager.getCols();
  
  const [myLayouts, setLayouts] = useState(createLayout(rowNum, colNum));

  /*
   anything entered in width and height textfields get saved into rowNum and colNum
   that are going to be used later when the user hits the confirm button
   */
  const handleRowTextFieldChange = x => {
    rowNum = x.target.value;
  }
  const handleColTextFieldChange = x => {
    colNum = x.target.value;
  }

  // clones existing class layout with new width and height
  function handleConfirmButtonClick() {

		var classToDelete = [{"className": classSelected}]
		var newClass = [{"className": classSelected, "width": parseInt(colNum,10), "height": parseInt(rowNum,10)}]
    AspNetConnector.removeClass(classToDelete);
    AspNetConnector.makeClass(newClass);
		//StateManager.setSelectedClass(classSelected);
    //StateManager.setClassLayout(createLayout(rowNum, colNum));
    history.push("/InstructorHome");
  }
  function handleApplyButtonClick() {
    StateManager.wipeSeats()
    setLayouts(myLayouts => [createLayout(rowNum, colNum)]);
  }
  function handleCancelButtonClick() {
    history.push("/InstructorHome");
  }

  return (
      <div>
          <div className="editSeatPlan">
              <form autoComplete="off">
                  <div className="rowTextField">
                      <TextField label="Enter number of rows" variant="outlined" onChange={handleRowTextFieldChange} />
                      <TextField label="Enter number of columns" variant="outlined" onChange={handleColTextFieldChange} />
                  </div>
              </form>
          </div>
          <div className="actionButtons">
              <Button className="confirmButton" onClick={handleConfirmButtonClick} variant="light">Confirm</Button>
              <Button className="ApplyButton" onClick={handleApplyButtonClick} variant="light">Apply</Button>
              <Button className="cancelButton" onClick={handleCancelButtonClick} variant="light">Cancel</Button>
          </div>
          <div className="preview">
              <label className="previewLabel">Preview</label>
              <Fragment>{myLayouts.map(item => <ul key={item}>{item}</ul>)}</Fragment>
          </div>
      </div>
  );
}
