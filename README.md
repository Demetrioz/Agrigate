# Agrigate

Agrigate is a platform that collects, manages, and analyzes all of your agricultural data, helping you to run a more 
efficient and profitable farm or garden.

## Project Structure

Agrigate is a mono-repo that contains all code needed to build and run the application. The project consists of the 
following items:

**Agrigate.App**: A Blazor server application that uses Identity Server for authentication.
**Agrigate.Core**: An Akka.Net console application that contains all business logic and acts as the "OS" for the farm.
**Agrigate.Domain**: A class library containing the data model for Agrigate.
**Agrigate.Domain.Auth**: A class library containing the data model for authentication.
**docs**: A docusaurus site containing full documentation for Agrigate. 

## Getting Started

### Installation

1. Clone (or download) the repository to your local machine

```
git clone https://github.com/Demetrioz/Agrigate.git
```

2. Navigate to the project's root directory and run docker compose to start the application

```
docker compose up --build
```

### Development

1. Ensure the following dependencies are installed

- .Net 8
- Docker

## Documentation

Full documentation for Agrigate can be found on [GitHub Pages](https://demetrioz.github.io/Agrigate/).
