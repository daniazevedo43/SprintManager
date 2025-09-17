# Sprint Manager

An api focused on organizing and tracking tasks. It provides a solid foundation for managing projects, sprints, work items and team members. Ideal for software development teams looking for a solution for their agile workflow.

## Architecture

* **Domain-Driven Design (DDD):** The project adopts a DDD approach, focusing on a domain model that encapsulates business logic and rules.
* **Command Query Responsibility Segregation (CQRS):** Separates operations that change state (commands) from operations that read state (queries).
* **MediatR:** Used for implementing the CQRS pattern, enabling clear separation between commands and queries, and promoting single responsibility for handlers.
* **Entity Framework Core:** Provides a way to interact with the database.
* **AutoMapper:** Facilitates object-to-object mapping, simplifying the transformation between domain entities and Data Transfer Objects (DTOs).
* **Custom Exception Handling:** Utilizes custom exceptions to provide specific error responses to API consumers.

## Core Domain Entities

The API revolves around the following key domain entities:

* **User:** Represents the users who access and manage tasks.
* **Project:** Represents the different projects where tasks will be organized.
* **ProjectMember:** Defines which users belong to which projects and their roles.
* **Sprint:** Organizes tasks into work cycles.
* **WorkItem:** Represents a functionality that needs to be implemented in a project.
* **Comment:** Represents comments left by users in work items.
* **Image:** Represents images attached by users in work items.
* **RefreshToken:** Represents the refresh tokens created for the authenticated users.

## Endpoints

The following API functionalities are already live:

#### Auth (`api/Auth`)

* `POST /api/Auth/register`: Where a user can register in the API.
* `POST /api/Auth/login`: Where a user can login in the API.
* `POST /api/Auth/logout`: Where a user can logout.
* `POST /api/Auth/refresh`: Where a user can refresh the access token when it expires.
* `GET /api/Auth/confirm-email`: Confirms a user's email after registration.
* `POST /api/Auth/resend-confirmation-email`: Sends a new confirmation email. Users can use this endpoint in case they don't receive the email the first time.
* `POST /api/Auth/forgot-password`: Sends the necessary details to the console so a user can reset it's password. On "development" environment, it sends an email.
* `POST /api/Auth/reset-password`: Resets a user's password.
* `DELETE /api/Auth/delete-account`: Where a user can delete his account.

#### Users (`/api/Users`)

* `GET /api/Users`: Returns a list of all users.
* `GET /api/Users/{id}`: Returns a user.

#### Projects (`/api/Projects`)

* `GET /api/Projects`: Returns a list of all projects.
* `GET /api/Projects/{id}`: Returns a project.
* `POST /api/Projects`: Creates a new project.
* `PUT /api/Projects/{id}`: Updates an existing project.
* `DELETE /api/Projects/{id}`: Removes an existing project.

#### ProjectMembers (`/api/ProjectMembers`)

* `GET /api/ProjectMembers`: Returns a list of all projects that have users working on them.
* `GET /api/ProjectMembers/{projectId}`: Returns a project that has users working on them.
* `POST /api/ProjectMembers`: Associates a user with a project.
* `PUT /api/ProjectMembers/{id}`: Changes the role of a user in a project.
* `DELETE /api/ProjectMembers/{id}`: Removes a user from a project.

#### Sprints (`/api/Sprints`)

* `GET /api/Sprints`: Returns a list of all sprints from all projects.
* `GET /api/Sprints/{id}`: Returns a sprint.
* `POST /api/Sprints`: Create a new sprint for a project.
* `PUT /api/Sprints/{id}`: Updates a sprint.
* `DELETE /api/Sprints/{id}`: Removes a sprint from a project.

#### WorkItems (`/api/Workitems`)

* `GET /api/Workitems`: Returns a list of all work items in all projects.
* `GET /api/Workitems/{id}`: Returns a project's work item.
* `POST /api/Workitems`: Creates a new work item for a project.
* `PUT /api/Workitems/{id}`: Updates a work item.
* `DELETE /api/Workitems/{id}`: Removes a work item from a project.

#### Comments (`/api/Comments`)

* `GET /api/Comments`: Returns a list of all comments in all work items.
* `GET /api/Comments/{id}`: Returns a comment.
* `POST /api/Comments`: Creates a new comment in a work item.
* `PUT /api/Comments/{id}`: Updates a comment.
* `DELETE /api/Comments/{id}`: Removes a comment.

#### Images (`/api/Images`)

* `GET /api/Images`: Returns a list of all images in all work items.
* `GET /api/Images/{id}`: Returns an image.
* `POST /api/Images`: Adds a new image in a work item.
* `DELETE /api/Images/{id}`: Removes an image from a work item.

## Environments

The API is configured to run in the following environments:

* **Local:** Used for daily development. Uses logs to simulate sending emails and displays the details in the console.
* **Development:** Used for testing integrations with SendGrid. For security reasons, the SendGrid key is not included in the repository.

## Database setup

Install SQL Server (if you haven't installed it yet).

### Connection String configuration

1. Open `appsettings.json` file in `SprintManager.API` project's folder.
2. Change `ConnectionStrings` so that it's configured to your SQL Server instance.

### Apply Migrations

Navigate to `SprintManager.API` project in your command line and run the command down below. This will apply all pending migrations and create the database if it doesn't exist:

```shell
dotnet ef database update
```

## Authentication

JWT is used for authentication and authorization. All endpoints, except for registration and login, require a valid access token.

#### How to get an access token

1. Send a `POST` request to register in the API.
    * **Endpoint**: `/api/Auth/register`
    * **Request Body**:

    ```json
    {
        "name": "your_name",
        "email": "your_email@domain.com",
        "password": "your_password"
    }
    ```

2. Click on the link shown in the console. The API will use the `/api/Auth/confirm-email` endpoint to automatically confirm the user's email. In "development" environment, a real email with a link is sent to the user.

3. Send a `POST` request to login in the API.
    * **Endpoint**: `/api/Auth/login`
    * **Request Body**:

    ```json
    {
        "email": "your_email@domain.com",
        "password": "your_password"
    }
    ```

4. If the login credentials are correct, the API will respond with an access token and a response token.

#### How to use the access token

After obtaining the access token, you must include it in the authorization header of all your requests to the protected endpoints.

* **Header**: `Authorization`
* **Value**: `Bearer {your-access-token}`

In case you're using Swagger, you can follow these steps to use the access token:

1. Click the **"Authorize"** button
2. Enter your access token
3. Click **"Authorize"**

Once authorized, Swagger will automatically add the authorization header to all requests you make to the protected endpoints.
