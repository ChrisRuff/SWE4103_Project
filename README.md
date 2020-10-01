# SWE4103_Project

Presented to you by TEAM 2!

CircleCI will run on each merge request to ensure the build works correctly. 

## Running the project ##
### Linux ###
* Install dotnet as well as aspnet-runtime. 
* Run these commands:
>`cd frontend && npm ci && cd ..`

>`export ASPNETCORE_Environment=Development && dotnet build`

>`dotnet run`

You can now connect to the development server by typing localhost:5000 into your web browser 

### MacOS ###
* Install dotnet. You can do this easily by installing [Homebrew package manager](https://brew.sh/) and running the following command:
>`brew cask install dotnet`

* Now you must install a .Net Core SDK found [here](https://aka.ms/dotnet-download).

* Then run these commands:
>`export ASPNETCORE_Environment=Development && dotnet build`

>`cd frontend && npm ci && cd ..`

>`dotnet run`

You can now connect to the development server by typing localhost:5001 into your web browser

### Windows ###
Two different ways:
1. Using Ubuntu (a Linux command prompt application easily available for Windows)
* Download Ubuntu from the Microsoft store
* Follow the instructions from the Linux section above
* You may need to update your npm using before running dotnet run
>`npm install -g npm`

NOTE: With Ubuntu in order to get to your Documents directory, for example, you have to first do (with your own Username)
>`cd /mnt/c/Users/Maddy/Documents/`

2. On Windows
* Download .NET - https://dotnet.microsoft.com/download (.NET Core 3.1 downloads)
* Download Nodejs - https://nodejs.org/en/ (14.13.0 Current)
* In command prompt do 
>`cd frontend`

>`npm ci`

>`cd ..`

>`dotnet run`

You can now connect to the development server by typing localhost:5000 into your web browser 

NOTE: The easiest code editor to use with C# will likely be Visual Studio Code, so download that as well. 

To find out how to build and run the front end see [this link](./frontend)
If you have any questions running the frontend please ask for help from someone on the team! 


### Running tests ###
#### Frontend tests ####
To run tests for frontend execute:
>`cd frontend && npm test`

This will take you to a new window that allows you to run tests.

#### Backend tests ####
To run tests for backend execute:
>`dotnet test test/`

This will run all the unit tests contained within the test folder.

# Happy developing! #
