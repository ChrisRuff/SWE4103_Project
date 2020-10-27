import React, { useState } from "react";
import { useHistory } from "react-router-dom";
import {
  HelpBlock,
  FormGroup,
  FormControl,
  ControlLabel,
} from "react-bootstrap";
import LoaderButton from "../components/LoaderButton";
import { useAppContext } from "../libs/contextLib";
import { useFormFields } from "../libs/hooksLib";
import { onError } from "../libs/errorLib";
import "./Signup.css";
import { Radio, RadioGroup, FormControlLabel } from "@material-ui/core";
import { sha512 } from "js-sha512";
import { StateManager } from "../StateManager";
import { AspNetConnector } from "../AspNetConnector.js" 

export default function Signup() {

  const history = useHistory();
  const [newUser, setNewUser] = useState(null);
  const [accountState, setValue] = useState(null);
  const { userHasAuthenticated } = useAppContext();
  const [isLoading, setIsLoading] = useState(false);
  const [fields, handleFieldChange] = useFormFields({
    name: "",
    email: "",
    password: "",
    confirmPassword: "",
    //confirmationCode: "", 
    account: "",    
  });

  const handleChange = (event) => {
    fields.account = event.target.value;  
    StateManager.setAccountState(event.target.value);
    setValue(event.target.value);
    if (fields.password !== fields.confirmPassword){
      onError("Passwords do not match");
    }
  };

  function validateForm() {
    return (
      fields.name.length>0 &&
      fields.email.length > 0 &&
      fields.password.length > 0 &&
      fields.password === fields.confirmPassword &&
      fields.account !== ""     
    );
  } 

  async function handleSubmit(event) {
    let hash = sha512(fields.password);
    event.preventDefault();
    setIsLoading(true);
    try {
      if(fields.account==="student"){
        const newUser = await AspNetConnector.addStudents([{
          "studentName": fields.name,
          "email": fields.email,
          "pass": hash,
        }]);
      }
      else{
        const newUser = await AspNetConnector.addProf([{
          "profName": fields.name,
          "email": fields.email,
          "pass": hash,
        }]);
      }
      setNewUser(newUser);
      userHasAuthenticated(true);
      history.push("/");
    } catch (e) {
      onError(e);
      setIsLoading(false);
    }
  }

  /* This will only be utilized if we want to send new users
     an emailed confirmation code when signing up
     async function handleConfirmationSubmit(event) {
      event.preventDefault();

      setIsLoading(true);
    }

  function renderConfirmationForm() {
    return (
      <form onSubmit={handleConfirmationSubmit}>
        <FormGroup controlId="confirmationCode" bsSize="large">
          <ControlLabel>Confirmation Code</ControlLabel>
          <FormControl
            autoFocus
            type="tel"
            onChange={handleFieldChange}
            value={fields.confirmationCode}
          />
          <HelpBlock>Please check your email for the code.</HelpBlock>
        </FormGroup>
        <LoaderButton
          block
          type="submit"
          bsSize="large"
          isLoading={isLoading}
          disabled={!validateConfirmationForm()}
        >
          Verify
        </LoaderButton>
      </form>
    );
  }*/

  function renderForm() {
    return (
      <form onSubmit={handleSubmit}>
        <FormGroup controlId="name" bsSize="large">
          <ControlLabel>Name</ControlLabel>
          <FormControl
            autoFocus
            type="name"
            value={fields.name}
            onChange={handleFieldChange}
          />
        </FormGroup>
        <FormGroup controlId="email" bsSize="large">
          <ControlLabel>Email</ControlLabel>
          <FormControl
            type="email"
            value={fields.email}
            onChange={handleFieldChange}
          />
        </FormGroup>
        <FormGroup controlId="password" bsSize="large">
          <ControlLabel>Password</ControlLabel>
          <FormControl
            type="password"
            value={fields.password}
            onChange={handleFieldChange}
          />
        </FormGroup>
        <FormGroup controlId="confirmPassword" bsSize="large">
          <ControlLabel>Confirm Password</ControlLabel>
          <FormControl
            type="password"
            onChange={handleFieldChange}
            value={fields.confirmPassword}
          />
        </FormGroup>
        <RadioGroup aria-label="Account"  value={accountState} onChange={handleChange}>
          <FormControlLabel value = "student"
            control={<Radio />}
            label={<span style={{ fontSize: '14px' }}>Student</span>}/>
          <FormControlLabel value = "professor"
            control={<Radio />}
            label={<span style={{ fontSize: '14px' }}>Professor</span>}/>
        </RadioGroup>
        <LoaderButton
          block
          type="submit"
          bsSize="large"
          isLoading={isLoading}
          disabled={!validateForm()}
        >
          Signup
        </LoaderButton>
      </form>
    );
  }

  return (
    <div className="Signup">
      {renderForm()}
    </div>
  );
}
