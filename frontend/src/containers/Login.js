import React, { useState } from "react";
import { useHistory } from "react-router-dom";
import { FormGroup, FormControl, ControlLabel } from "react-bootstrap";
import LoaderButton from "../components/LoaderButton";
import { useAppContext } from "../libs/contextLib";
import { useFormFields } from "../libs/hooksLib";
import { onError } from "../libs/errorLib";
import { sha512 } from "js-sha512";
import "./Login.css";
import { Radio, RadioGroup, FormControlLabel } from "@material-ui/core";
import { AspNetConnector } from "../AspNetConnector.js" 
import { StateManager } from "../StateManager";

export default function Login() {
  const history = useHistory();
  const { userHasAuthenticated } = useAppContext();
  const [accountState, setValue] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [fields, handleFieldChange] = useFormFields({
    email: "",
    password: "",
    account: "",
  });

  const handleChange = (event) => {
    fields.account = event.target.value;  
    StateManager.setAccountState(event.target.value);
    setValue(event.target.value);
  };

  function validateForm() {
    return (
    fields.email.length > 0 && 
    fields.password.length > 0 &&
    fields.account !== ""
    );
  }

  // handleSubmit is called once the 'login' button is clicked
  async function handleSubmit(event) {
    event.preventDefault();
    setIsLoading(true);

    try {
      let hash = sha512(fields.password);
      if ((fields.account==="student")){
        let request = await AspNetConnector.loginStudent([{
          "email": fields.email,
          "pass": hash,
        }]);
        request.onload = function() {
          let obj = (JSON.parse(request.response));
          let value = (obj[0].response);
          if (value === true){
            userHasAuthenticated(true);
            history.push("/InstructorHome"); 
          }
          else{
            onError("Invalid password or account selected");
            userHasAuthenticated(false);
            history.push("/")
          }
        }
      }
      else {
        let request = await AspNetConnector.loginProf([{
          "email": fields.email,
          "pass": hash,
        }]);
        request.onload = function() {
          let obj = (JSON.parse(request.response));
          let value = (obj[0].response);
          if (value === true){
            userHasAuthenticated(true);
            history.push("/InstructorHome"); 
          }
          else{
            onError("Invalid password");
            userHasAuthenticated(false);
            history.push("/")
          }
        }
      }
    } catch (e) {
      onError(e);
      setIsLoading(false); 
    }
  }

  return (
    <div className="Login">
      <form onSubmit={handleSubmit}>
        <FormGroup controlId="email" bsSize="large">
          <ControlLabel>Email</ControlLabel>
          <FormControl
            autoFocus
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
          Login
        </LoaderButton>
      </form>
    </div>
  );
}
