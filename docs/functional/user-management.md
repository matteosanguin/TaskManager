# User Management

This document describes the user management features of the TaskManager application.

## User Registration

Users can register for a new account using the registration page. The following information is required:

- First Name
- Last Name
- Username
- Email
- Password

## User Login

Registered users can log in using their username and password. Upon successful login, they are redirected to the home page.

## User Profile

Users can view and update their profile information. The following information can be updated:

- First Name
- Last Name
- Email

## Roles

The application uses ASP.NET Core Identity roles to manage user permissions. The following roles are planned:

- **Admin**: Can manage all aspects of the application.
- **User**: Can create and manage their own projects and tasks.

Role management is not yet implemented in the UI.
