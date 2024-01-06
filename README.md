# FaceAnalyzer - Backend Project

This repository contains the FaceAnalyzer backend solution. The project is written in .NetCore version 7.0
> The Api code is inside FaceAnalyzer.Api project which is an ASP.NET Core Web API application
## Folder Structure
- **/Data**: Contains database entities, migrations, and database context.
    - **/Entities**: Database entity classes.
    - **/Migrations**: Database migration scripts.
- **/Service**: Includes the service layer, controllers, middlewares, and filters.
    - **/Controllers**: API controllers.
    - **/Middlewares**: Custom middleware components.
    - **/Profiles**: Mapping profiles for service layer
    - **/Swagger**: Swagger Documents customization
- **/Business**: Houses business models, DTOs, and mappers.
    - **/UseCases**: Business use cases
    - **/Commands**: Business use case commands
    - **/Queries**: Business use case queries
    - **/Contracts**: Describes the communication between the business and other layers. (dtos for example)
    - **/Profiles**: Mapping profile logic for business layer.
- **/Shared**: Stores enums, constants, and shared resources.
    - **/Enum**: Enumeration types.
    - **/Security**: Authentication and Authorization
    - **/Exceptions**: Shared exceptions thrown by the application. 

## Getting Started

To run the FaceAnalyzer project, follow these steps:

1. Install .Net7 from [here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
2. Clone the repository:
 ```bash
  git clone https://github.com/FaceAnalyzer/backend.git
 ```
2. Open the project in your preferred IDE.
3. Configure your database connection. (Put the connection string in the appropriate file. See next section! )
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
    "AppDatabase": "server=localhost;user=<db user>;password=<password>;database=face_analyzer",
    "DbVersion": "8.0.34"
  },
  "JwtConfig": {
    "Secret": "secret key of 16 bytes long",
    "Expiry": "expiry period in minutes"
  }
}
```

## Deployment

### Architecure

FaceAnalyzer backend is built as a cloud-native app. It is deployed as 2 containers. One container runs `mysql:8.0` image, while the other one runs our ASP.NET backend app. Both of these containers are deployed together to a [Kubernetes cluster](https://github.com/FaceAnalyzer/aks-cluster).

The deployment manifest is located in `FaceAnalyzer.Api`. `manifest-production.yaml` is for production environment, while `manifest-staging.yaml` is for staging environment. At this time the only difference is the domain for backend container, configuration is the same for both environments.

Apart from having Deployment, backend also has Service and Ingress. This exposes backend to the Internet. Ingress resource is picked up by Ingress-nginx, that should already be deployed into the cluster. MySQL has Depoyment, Service and PVC. Service makes MySQL Deployment exposed to other Deployments in the cluster, but not outside the cluster. PVC is linked to a persistant volume. This means MySQL will not lose data if it's restarted.

### CI/CD

Backend repo is supported by CI/CD. We use GitHub Actions that are built-in to GitHub.
The deployment pipeline is straightforward. It has two jobs.
The first job builds a Docker image using the Dockerfile in the repo, and then pushes the Docker image to a Docker registry, in our case DockerHub.
The second job deploys a  Kubernetes YAML manifest to the Kubernetes cluster. Based on this manifest, Kubernetes cluster pulls the right image from the DockerHub. After applying the manifests, database migrations are applied. The job creates ASP.NET migration bundle locally, then copies it to the backend Deployment, and finally executes it.

For CI/CD to work properly, a working Kubernetes cluster is required, including access to the cluster. Learn more on [aks-cluster](https://github.com/FaceAnalyzer/aks-cluster) repo.

Additionaly, there are a few test jobs in the pipeline.
[CodeQL](https://codeql.github.com/) checks for code quality. It's a static linter that scans for common issues and vulnerabilities.
There is also `Build and Test` job that runs tests written by us.

### Makefile

You can also deploy this project manually, without CI/CD. A Makefile is provided in `FaceAnalyzer.Api` directory.

Run `make deploy VERSION=<version>` to build the Docker image, push the image, and apply the Kubernetes manifest.
You must set a version.

### Docker Compose

For easier local deployment and testing, `docker-compose.yml` is provided.

1. Run `docker compose up` to start backend and MySQL server inside the Docker network.
2. Run `make migrate-local` to apply migrations. This will create ASP.NET migration bundle and copy it to the backend container.

If you want to keep data persistant, add a [volume](https://tecadmin.net/docker-compose-persistent-mysql-data/) to MySQL service in `docker-compose.yml`.

## Project Dependencies

- ASP.NET Core: [Link to ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet)
- Entity Framework Core: [Link to EF Core](https://docs.microsoft.com/en-us/ef/core/)
- MySql version 8.0.34
- MongoDB server
- .NET7.0

## Git Workflow

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
