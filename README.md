# SWE4103_Project

Presented to you by TEAM 2!

CircleCI will run on each merge request to ensure the build works correctly. 

## Running the project ##
### Linux ###
* Install dotnet as well as aspnet-runtime. 
* Run these commands:
>`export ASPNETCORE_Environment=Development && dotnet build`

>`cd frontend && npm ci && cd ..`

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
I'm not sure yet ¯\\_(ツ)_/¯.

To find out how to build and run the front end see [this link](./frontend)
If you have any questions running the frontend please ask for help from someone on the team! 

# Happy developing! #
