# Getting Started

## Local Setup

To run projects individually, outside of docker, complete the following steps:

## Local Setup (Docker)

This is currently a work in progress; However, to run and / or test docker compose locally, complete the following 
steps:

1. Clone the repository to your local machine   
   `git clone git@github.com:Demetrioz/Agrigate.git`

2. Create a .env file in the `/src` directory using `.env-example` as a base
3. Download and install NGrok so the API and web app can communicate properly with Keycloak
4. Update your NGrok configuration to run multiple tunnels with a single worker
    ```
    version: "3"
    agent:
      authtoken: xyz
         
      tunnels:
        agrigatae_keycloak:
          addr: 8080
          proto: http
        agrigate_api:
          addr: 5000
          proto: http
        agrigate_web:
          addr: 5001
          proto: http
    ```

5. Start Ngrok  
   `ngrok start --all`

6. Update the `Authority` and `MetadataAddress` values in `docker-compose.yml`
   ```
   # api service
   Authentication__Authority=https://ngrok-keycloak-id.ngrok-free.app/auth/realms/Agrigate
   Authentication__MetadataAddress=https://ngrok-keycloak-id.ngrok-free.app/auth/realms/Agrigate/.well-known/openid-configuration
   
   # web service
   Authentication__Authority=http://ngrok-keycloak-id.ngrok-free.app/auth/realms/Agrigate
   ```

7. Run docker compose from the `/src` directory  
   `docker compose up --build`

8. Log in to Keycloak using the local address, not the NGrok address using `admin` for the username and password
9. Switch to the `Agrigate` realm, then update the `Agrigate.Api` and `Agrigate.Web` clients to have the appropriate
    redirect URIs and web origins. These should be the NGrok addresses forwarding to 5000 and 5001, respectively
10. At this point, you should be able to navigate to the NGrok address forwarding to 5000 to utilize the API, and the 
    address forwarding to 5001 to use the web app. To log in via the API's swagger page, use Agrigate.Api for the 
    `client_id`, `admin@agrigate.instance` for the user, and `password` for the password.

> [!IMPORTANT]
> When logging in, you'll receive a message about the information not being secure. Continue anyway, and this will be 
> addressed before running in production

## Authentication With Docker Local

When running `docker compose up`, services will be started locally, but authentication on the API will fail to validate
the issuer if using `http://localhost:9011` as the authority. To resolve this, start ngrok with 
`ngrok http --url=MyStaticNGrokUrl 9011` and set `ENV Authentication__Authority=https://MyStaticNGrokUrl` in 
DockerfileApi.

Then, you can use `https://MyStaticNGrokUrl/api/login` to request a token and authenticate successfully with the API