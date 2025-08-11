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

* **Users:** Represents the users who access and manage tasks.
* **Projects:** Represents the different projects where tasks will be organized.
* **ProjectMembers:** Defines which users belong to which projects and their roles.
* **Sprints:** Organizes tasks into work cycles.
* **WorkItems:** Represents a functionality that needs to be implemented in a project.
* **Comments:** Represents comments left by users in work items.
* **Images:** Represents images attached by users in work items.

## Endpoints

The following API functionalities are already live:

#### Users (`/api/Users`)

* `GET /api/Users`: Returns a list of all users.
* `GET /api/Users/{id}`: Returns a user.
* `POST /api/Users`: Creates a new user.
* `PUT /api/Users/{id}`: Updates an existing user.
* `DELETE /api/Users/{id}`: Removes an existing user.

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
* `GET /api/Sprints/{id}`: Returns a sprint of a project.
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
* `GET /api/Comments/{id}`: Returns a comment from a work item.
* `POST /api/Comments`: Creates a new comment in a work item.
* `PUT /api/Comments/{id}`: Updates a comment.
* `DELETE /api/Comments/{id}`: Removes a comment.

#### Images (`/api/Images`)

* `POST /api/Images`: Adds a new image in a work item.

## Database setup

Install SQL Server (if you haven't installed it yet).

### Connection String configuration

1. Open `appsettings.json` file in `SprintManager.API` project's folder.
2. Change `ConnectionStrings` so that it's configured to your SQL Server instance.


Navigate to `SprintManager.API` project in your command line and run the command down below. This will apply all pending migrations and create the database if it doesn't exist:

```shell
dotnet ef database update
```