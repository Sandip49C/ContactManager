ContactManager
A simple ASP.NET Core web app to manage contacts (name, phone, email) with CRUD functionality, duplicate prevention, search, and notifications. Uses MySQL for storage and Entity Framework Core for database operations.

Features
Add, view, edit, and delete contacts.
Prevents duplicate phone numbers and emails.
Search contacts by name, phone, or email.
Displays notifications for actions.
"Visit Contact Manager" button on the home page to access contacts.

Setup
Clone the repo:https://github.com/Sandip49C/ContactManager 
Set up MySQL:
 Create a database: CREATE DATABASE ContactManagerDB;
 Update appsettings.json with your MySQL connection string.
Apply migrations: dotnet ef database update
Run the app: dotnet run
Open https://localhost:7245/ in your browser.

Requirements
.NET SDK 7.0
MySQL Server
Visual Studio or VS Code
