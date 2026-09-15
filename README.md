# Contract Claim Data Manager

## About the Project

Contract Claim Data Manager is a C# .NET 8 console application for managing lecturer contract claims. The application uses SQLite to save claim information.

The application can:

* Add new claims
* View existing claims
* Update claim status
* Delete claims
* Export claims to a text file
* Run a direct ADO.NET query to count claims

## Data Access

**ORM:** An Object-Relational Mapper (ORM) allows C# objects to work with data stored in a database. In this project, Entity Framework Core is used as the ORM.

**Entity:** The `Claim` class represents a claim record in the database.

**DbContext:** `ClaimContext` manages the connection between the application and the database.

**DbSet:** `DbSet<Claim>` represents the collection of Claim records in the database.

**Provider:** The SQLite provider allows Entity Framework Core to communicate with the SQLite database.

## Code First and Database First

**Code First** means the database structure is created from the C# classes and their configuration. This project uses Code First.

**Database First** starts with an existing database and creates the application classes from that database.

## Database

The application uses a SQLite database called:

`claims.db`

The database is created automatically when the application starts.

## Reports

The exported report is saved in:

`Reports/claim_summary.txt`

The report is also displayed in the console after it has been created.

## Requirements

* .NET 8
* Visual Studio
* Microsoft.EntityFrameworkCore.Sqlite
* Microsoft.Data.Sqlite
