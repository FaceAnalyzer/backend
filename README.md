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
    - **/Contracts**: Describes the communication between the service and other layers. (dtos for example)
    - **/Exceptions**: Shared exceptions thrown by the application. 

## Getting Started

To run the FaceAnalyzer project, follow these steps:

1. Clone the repository: `git clone <repository-url>`
2. Open the project in your preferred IDE.
3. Configure your database connection in `appsettings.json`. (this will probably change later !!)
4. Run migrations to create the database: `dotnet ef database update`
5. Start the application: `dotnet run`

## Deployment
...
## Project Dependencies

- ASP.NET Core: [Link to ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet)
- Entity Framework Core: [Link to EF Core](https://docs.microsoft.com/en-us/ef/core/)
- Other dependencies...


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
