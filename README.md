# RaceDay PoE — Event Management System

## Project Overview

RaceDay is a road running, walking and cycling event management system designed to support the management of sporting events and their participants. The system provides functionality for managing events, participant enrolments, race categories, results, routes and weather information.

This repository contains the different parts of the RaceDay Portfolio of Evidence (PoE), including database development, SQL scripts, an ASP.NET Core Web API, automated tests and continuous integration.

## Repository Structure

```text
RaceDay-POE-Parts/
│
├── Part 1/
│   ├── docs/
│   ├── SQL_Queries/
│   ├── Screenshots/
│   └── README.md
│
├── Part 2/
│   ├── RaceDay.API/
│   ├── RaceDay.API.Tests/
│   ├── SQL_Queries/
│   ├── Screenshots/
│   └── README.md
│
├── .github/
│   └── workflows/
│       └── ci.yml
│
└── README.md
```

*Note: The structure above shows the intended organisation of the repository. The actual folders and files should be checked against the uploaded project.*

## Part 1 — Database Design and SQL

Part 1 focuses on the database foundation of the RaceDay system. It includes SQL scripts, database documentation and supporting design evidence.

The database covers the following main entities:

- RaceDayUsers
- Events
- Categories
- Enrolments
- Results
- Routes
- Weather

The SQL scripts are used to create, populate, inspect and verify the database, depending on the individual script.

Supporting documentation and database design evidence are stored in `Part 1/docs/`.

## Part 2 — ASP.NET Core Web API

Part 2 develops the RaceDay application further through an ASP.NET Core Web API using C# and .NET 8.

The API is designed to provide access to RaceDay data through endpoints for functionality such as:

- User authentication and session management
- Event management
- Category management
- Participant enrolments
- Race results
- Route information
- Weather information

Entity Framework Core is used to connect the API to SQL Server. Access to protected functionality is controlled according to the application's user roles.

The API source code is stored in `Part 2/RaceDay.API/`.

## Automated Testing

The test project is stored in `Part 2/RaceDay.API.Tests/`.

Automated tests help verify the application's behaviour and identify defects during development. The tests can be executed locally using the .NET CLI:

```bash
dotnet test "Part 2/RaceDay.API.Tests/RaceDay.API.Tests.csproj" --configuration Release
```

Run this command from the repository root after confirming that the project paths and references are correct.

## Continuous Integration — GitHub Actions

The repository includes a GitHub Actions workflow at `.github/workflows/ci.yml`.

The workflow is intended to run automatically when changes are pushed to the configured branch or when a pull request targets that branch.

The workflow checks that Part 1 contains SQL scripts and restores, builds and tests the Part 2 .NET application.

A successful workflow run provides evidence that the configured checks passed for that revision. The SQL-file checks do not execute the scripts against SQL Server.

## Screenshots and Supporting Evidence

Screenshots and other supporting evidence are organised in the relevant part's `Screenshots` folders.

This evidence can include database query results, API demonstrations, automated test results and GitHub Actions runs. Only evidence that has actually been captured and verified should be included in the final submission.

## Demonstration Video

**YouTube demonstration:** [Add the final YouTube video link here]

The demonstration video will present the RaceDay project, explain the main functionality, show relevant database and API evidence, and demonstrate the testing and continuous integration process.

## Technologies Used

- C#
- ASP.NET Core Web API
- .NET 8
- Entity Framework Core
- Microsoft SQL Server
- SQL
- Git and GitHub
- GitHub Actions
- Automated testing

## Project Status

This repository brings the RaceDay PoE components together in one organised location. The final submission should be checked to ensure that all required source files, SQL scripts, documentation, screenshots, test results and demonstration links are present and that the CI workflow runs successfully.

## Author

**Student:** Oviweyo Mdunyelwa 
**Project:** RaceDay PoE — Event Management System
