import React from 'react';
import { render } from '@testing-library/react';
import App from './App';
import { BrowserRouter as Router } from "react-router-dom";

test('renders learn react link', () => {
  const { getByText } = render(getApp());
  const linkElement = getByText(/Login/i);
  expect(linkElement).toBeInTheDocument();
});

function getApp()
{
	return (
		<div> 
			<Router> 
				<App /> 
			</Router> 
		</div>);
}
