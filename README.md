# Currency Converter

A currency converter web application built on .NET Framework 4.6.1, utilizing the European Central Bank's XML exchange rate data.

## Overview

This web application facilitates currency rate retrieval for a specified time interval and conversion of a specified amount from one currency to another on a specific date.

## Features

- **Dynamic Currency Rate Retrieval:** Retrieve historical currency rates within a specified time range based on user input.
- **Currency Conversion:** Convert a specified amount from one currency to another on a given date.

## Getting Started

### Prerequisites

- Visual Studio
- .NET Framework 4.6.1
- SQL Server

### Installation

1. Clone the repository: `git clone https://github.com/Regres404/CurrencyConverter.git`
2. Open the solution in Visual Studio.
3. Update the connection string in `Web.config` to point to your SQL Server instance.
4. Run the application.

## Database

The application uses Entity Framework Code-First. The database will be automatically created and seeded with initial data on the first run.

## Logging

Log entries are generated using log4net. The log file path and configuration can be modified in the `Web.config` file.
