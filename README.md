# Zindagi - Demo

Blood donation management app using Redis JSON, Redis Search and C# Blazor

# Technologies

* .NET Blazor - Server Side
* Telerik - Blazor Components, using trail version.
* Redis Modules Used:
    * Redis JSON - Storing and retrieving requests, user information
    * Redis Search - Searching requests
    * Redis Pub/Sub - Sending notifications in background

# Design Patterns

* Mediator
* Domain Driven Design (partially)  

# Images

<a href="https://github.com/BhanuKorthiwada/RedisZindagi/blob/main/docs/images/001.png?raw=true"><img src="https://github.com/BhanuKorthiwada/RedisZindagi/blob/main/docs/images/001.png?raw=true" width="100%" height="auto"></a>

<a href="https://github.com/BhanuKorthiwada/RedisZindagi/blob/main/docs/images/002.png?raw=true"><img src="https://github.com/BhanuKorthiwada/RedisZindagi/blob/main/docs/images/002.png?raw=true" width="50%" height="auto"></a>

<a href="https://github.com/BhanuKorthiwada/RedisZindagi/blob/main/docs/images/003.png?raw=true"><img src="https://github.com/BhanuKorthiwada/RedisZindagi/blob/main/docs/images/003.png?raw=true" width="50%" height="auto"></a>

<a href="https://github.com/BhanuKorthiwada/RedisZindagi/blob/main/docs/images/004.png?raw=true"><img src="https://github.com/BhanuKorthiwada/RedisZindagi/blob/main/docs/images/004.png?raw=true" width="50%" height="auto"></a>

<a href="https://github.com/BhanuKorthiwada/RedisZindagi/blob/main/docs/images/005.png?raw=true"><img src="https://github.com/BhanuKorthiwada/RedisZindagi/blob/main/docs/images/005.png?raw=true" width="50%" height="auto"></a>

## How it works
### How the data is stored:

* The request data is stored in various keys and various data types.
    * For each of request:
        * ID : `Guid` as a string
        * Blood Group, Donation Type, Priority, Status: C# `ENUM`

* Redis JSON
    * User Profile Key:
        prefix: `USER_PROFILE`
        postfix: Auth0 name identifier

    * Request Key:
        prefix: `BLOOD_REQUEST`
        postfix: Guid string

* Redis Publish:
    * Requests: any new blood request will publish request id as message to topic `URN:BLOODREQUESTS:NEW`

### How the data is accessed:

* C# Repository pattern is used, every call will create an instance using Connection Multiplexer

## Hot to run it locally?

### Prerequisites

- .NET Core - v5.0.x (latest patch version)
- Visual Studio 2019 16.9 or Visual Studio Code 1.55
- Docker - v19.03.13 (optional)
- Auth0:
  - Domain
  - Client ID
  - Client Secret

### Local installation

Clone the project  
Go to `/src/Zindagi` folder, update `appsettings.json` with below details
    - [Auth0 (refer for detailed steps)](https://auth0.com/blog/what-is-blazor-tutorial-on-building-webapp-with-authentication/)
    - Redis Connection String
    - SMTP details (optional)

run `dotnet run`

visit application using [localhost with SSL](https://localhost:5001) or [localhost](http://localhost:5000)

Application will be up and running

### Local installation using docker

Clone the project

Update `.env` file with **Auth0** and **SMTP** details

From terminal/command prompt run `docker compose up -d`

Application can be accessing using [localhost](http://localhost:80)
