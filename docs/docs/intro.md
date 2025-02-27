# Introduction

Agrigate is a platform that collects, manages, and analyzes all of your agricultural data to help you run a more 
efficient, profitable farm or garden.

## How to use Agrigate

### Base Installation

Agrigate is designed to be simple to install and use - All you need to have installed is 
[Docker](https://www.docker.com/).

First, clone the repository to your local machine. If you don't have or know how to use [Git](https://git-scm.com/), 
you can also download the files instead.

```
git clone https://github.com/Demetrioz/Agrigate.git
```

Next, navigate to the directory where you cloned or downloaded the files, and run docker compose.

```
docker compose up --build
```

At this point, you should be able to navigate to `http://localhost:5000` and login using the following username and
password:

- **Username**: admin
- **Password**: Ag@dm1n!

:::danger

Be sure to change the default admin password, especially if you're exposing the application to the internet for remote 
access.

:::

### Remote Access

Since Agrigate is running on your machine, you can only access it when you're connected to your local network by 
default. If you would like to have access to the application from anywhere internet access is available, you can expose
`http://localhost:5000` to the web by using something like [NGrok](https://ngrok.com/) or 
[Cloudflare](https://www.cloudflare.com/).

:::info

Using a service like NGrok or Cloudflare will allow you to expose the web application to the internet, without 
exposing the database, background service, or the rest of your network.

:::

#### NGrok

Coming Soon!

#### Cloudflare Zero Trust

Coming Soon!

## Technical Details

Agrigate is an entire platform, containing a web application, background service, and database.

### Dependencies

- [.Net](https://dotnet.microsoft.com/en-us/)
- [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor)
- [Akka.Net](https://github.com/akkadotnet/akka.net)
- [Identity Server](https://duendesoftware.com/products/identityserver)
- [Petabridge.Cmd](https://cmd.petabridge.com/) (Optional - for remote management of the Akka.Net background service) 

### Versioning

All components within Agrigate utilize
[semantic versioning](https://semver.org/) and follow the \{MAJOR\}.\{MINOR\}.\{PATCH\} format. Release information can 
be found via the [Releases](releases/intro) page

## Contact Information

Agrigate is developed and maintained by Kevin Williams. For assistance, reach
out to via [email](mailto:kevin.williams@kevinwilliams.dev).

Have you encountered a bug? [Let me know!](https://github.com/Demetrioz/Agrigate/issues)
