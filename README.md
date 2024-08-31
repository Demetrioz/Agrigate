# Agrigate

Agrigate is a platform that collects, manages, and analyzes all of your
agricultural data and helps you run a more efficient, profitable farm or garden.

## Project Structure

Agrigate is a mono-repo that contains all code needed to build and run the
application. The project consists of the following items:

### Client Applications

The client application is what you use to interact with the Agrigate platform
and lives in the `agrigate` directory. It is built using flutter and can
run on Android, iOS, Mac, Windows, Linux, or Web.

### Server Application

The `server` directory contains a dockercompose file that will create a
container with all of the server related code required to run an instance of
Agrigate. This includes a database, api, and other various services.

### Devices

The `devices` directory contains different IoT devices that can be used on their
own or in conjunction with Agrigate. The majority of devices are designed using
some version of a Raspberry Pi.

### Docs

The `docs` directory contains the code for the documentation website.

## Documentation

Full documentation for agrigate can be found at
[Coming Soon!](https://github.com/Demetrioz/Agrigate).
