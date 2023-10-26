# FaceAnalyzer - Backend Project

This repository contains the FaceAnalyzer backend project, an ASP.NET Core Web API.

## Folder Structure
This list is not complete ! 
Some folders does not exist yet. 
- **/Data**: Contains database entities, migrations, and database context.
    - **/Entities**: Database entity classes.
    - **/Migrations**: Database migration scripts.
    - ...
- **/Service**: Includes the service layer, controllers, middlewares, and filters.
    - **/Controllers**: API controllers.
    - **/Middlewares**: Custom middleware components.
    - ...
- **/Business**: Houses business models, DTOs, and mappers.
    - **/Models**: Business models.
    - **/Contracts**: Describes the communication between the business and other layers. (dtos for example)
    - **/Mappers**: Mapping logic.
- **/Shared**: Stores enums, constants, and shared resources.
    - **/Enum**: Enumeration types.
    - **/Constants**: Shared constants.
    - **/Utils**: Shared utility functions.
    - **/Exceptions**: Shared exceptions thrown by the application. 
  

## Getting Started

To run the FaceAnalyzer project, follow these steps:

1. Install .Net7 from [here](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
2. Clone the repository:
 ```bash
  git clone https://github.com/FaceAnalyzer/backend.git
 ```
2. Open the project in your preferred IDE.
3. Configure your database connection. (Put the connection string in the appropriate file. see next section! )
4. Run migrations to create the database: 
```bash 
dotnet ef database update
```
5. Start the application:
```bash
dotnet run
```
## Environment Setup

ASP.NET Core's Configuration Manager supports multiple configuration sources with a specific priority order. You can learn more about the priority order [here](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration).

The primary configuration sources for development are:

- **appsettings.json**: This serves as the foundational configuration file, containing common settings for the application.

- **appsettings.Development.json**: Designed for the development environment, this file is used to override configurations from the base `appsettings.json`.

- Notably, the `appsettings.Development.json` file is excluded from the Git repository. You must manually add it to your project and use it to specify configurations that should be overridden, such as your **local database connection string**.
Here is an example of appsettings.Development.json
```json
{
  "ConnectionStrings": {
    "AppDatabase": "server=localhost;user=<db user>;password=<password>;database=<datatabase name>",
    "DbVersion": "<mysql version>",
    "RawData": "<mongodb connection string>",
    "NoSqlDatabaseName": "<mongodb database>"
  },
  "JwtConfig": {
    "Secret": "secret key of 16 bytes long",
    "Expiry": "expiry period in minutes"
  }
}
```
## Deployment
...
## Project Dependencies

- ASP.NET Core: [Link to ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet)
- Entity Framework Core: [Link to EF Core](https://docs.microsoft.com/en-us/ef/core/)
- MySql version 8.0.34
- MongoDB server
- .NET7.0


## Git Workflow
(this is just a placeholder we can change it later if we agreed on something else )

Our project follows a Git workflow to manage development, feature integration, and hotfixes.

### Branches

- **main**: The main branch represents the production-ready code. Deployments are made from this branch.
- **develop**: The develop branch is the integration branch for feature development. New features are developed and tested here before merging into the main branch.
- **feature branches**: Feature branches are created from the develop branch for new feature development. Once a feature is complete and tested, it's merged back into the develop branch.
- **hotfix branches**: Hotfix branches are created from the main branch to address critical issues in the production code. Once the hotfix is complete, it's merged into both main and develop branches.

### Workflow Steps

1. **Feature Development**:
  - Create a new feature branch from the develop branch.
  - Develop and test your feature.
  - Commit changes to your feature branch.

2. **Feature Integration**:
  - Create a pull request (PR) to merge your feature branch into the develop branch.
  - Collaborate with team members for code review.
  - Once approved, merge your feature branch into the develop branch.

3. **Deployment**:
  - Periodically, the develop branch is merged into the main branch for deployment to production.

4. **Hotfixes**:
  - If critical issues arise in the main branch (production), create a hotfix branch.
  - Develop and test the hotfix.
  - Create a PR to merge the hotfix into both main and develop branches.

### Commit Messages

Please follow meaningful commit message conventions for clear version tracking.

Example Commit Message:
- feat: Add user authentication feature
- fix: Correct API response format
- docs: Update README with Git workflow


This Git workflow ensures a structured approach to feature development, integration, and hotfix management while maintaining a stable production environment.
