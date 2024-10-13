# Server

The Agrigate server consists of several components:

- An API for interacting with the platform
- An event service for dealing with events such as notifications and telemetry
- A database for storing all of the data

The easiest way to configure everything is by using the included docker-compose
file.

## Local Installation

### Docker

#### Install Pre-requisites

To run the Agrigate server locally, first install the following pre-requisites

- [Git](https://git-scm.com/)
- [Docker](https://www.docker.com/)

#### Clone The Repo

Next, clone the Agrigate repo to your local machine

```
git clone https://github.com/Demetrioz/Agrigate.git
```

#### Create an MQTT Broker

Before starting Agrigate, we need to create an MQTT broker for the event service
to be able to receive telemetry events. You can do this by signing up for a free
account at [EMQX](https://www.emqx.com/en) and choosing a serverless broker /
deployment.

:::info

Agrigate currently utilizes an EMQX broker in order to work with the
[Hyperion](https://demetrioz.github.io/Hyperion/) application and receive
notifications even outside your home network. Eventually, there will be an
option to utilize a self-hosted broker instead of using a 3rd party cloud
service.

:::

#### Update Docker-Compose

Once the repo has been cloned locally and we have an MQTT broker available,
we can update the `server/docker-compose.yml` file. Replace \{VALUE\} with
appropriate values for your use case

```
version: "3"

services:
  database:
    ...
    environment:
      - "POSTGRES_PASSWORD={VALUE}"               # Choose a strong password

  api:
    ...
    environment:
      - "Database__Password={VALUE}"              # Should be the same as the database password
      - "Api__ApiKey={VALUE}"                     # A secret key required to authentication with the API
      - "Notifications__ClientId={VALUE}"         # The clientId that will show up in the MQTT broker
      - "Notifications__Host={VALUE}"             # The MQTT broker's host name
      - "Notifications__Port={VALUE}"             # The MQTT broker's port
      - "Notifications__Username={VALUE}"         # The username for connecting to the MQTT broker
      - "Notifications__Password={VALUE}"         # The password for connecting to the MQTT broker
      - "Notifications__SecureConnection={VALUE}" # Whether the broker connection is using TLS

  eventservice:
    ...
    environment:
      - "Telemetry__ClientId={VALUE}"             # The clientId that will show up in the MQTT broker
      - "Telemetry__Host={VALUE}"                 # The MQTT broker's host name
      - "Telemetry__Port={VALUE}"                 # The MQTT broker's port
      - "Telemetry__Topic={VALUE}"                # The topic where you want telemetry to be sent
      - "Telemetry__Username={VALUE}"             # The username for connecting to the MQTT broker
      - "Telemetry__Password={VALUE}"             # The password for connecting to the MQTT broker
      - "Telemetry__SecureConnection={VALUE}"     # Whether the broker connection is using TLS
      - "Database__Password={VALUE}"              # Should be the same as the api's passsword value
      - "Notifications__ClientId={VALUE}"         # The clientId that will show up in the MQTT broker
      - "Notifications__Host={VALUE}"             # The MQTT broker's host name
      - "Notifications__Port={VALUE}"             # The MQTT broker's port
      - "Notifications__Username={VALUE}"         # The username for connecting to the MQTT broker
      - "Notifications__Password={VALUE}"         # The password for connecting to the MQTT broker
      - "Notifications__SecureConnection={VALUE}" # Whether the broker connection is using TLS
    expose:
      - {VALUE}                                   # Should match the Telemetry__Port value

```

:::danger

Make sure that the `Api__ApiKey` value is difficult to guess or remember,
especially if exposing your local Agrigate API to the internet.

Someone with this key would be able to make API requests to read and modify data
within your Agrigate instance.

Occasionally changing this value is recommended, as is using a password
generator like Proton Pass or 1Password.

:::

#### Start Docker-Compose

Now we can start docker with the following commands

```
cd /server
docker compose up
```

#### Verification

At this point, docker should be running a `server` container with the 3
following sub-containers

- database
- api
- eventservice

:::info

The postgres database is available at `localhost:8080`, and the
api is available at `http://localhost:8081`. The swagger docs are at
`http://localhost:8081/swagger/index`

:::
