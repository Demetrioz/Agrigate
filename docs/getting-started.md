# Getting Started

## Authentication With Docker Local

When running `docker compose up`, services will be started locally, but authentication on the API will fail to validate
the issuer if using `http://localhost:9011` as the authority. To resolve this, start ngrok with 
`ngrok http --url=MyStaticNGrokUrl 9011` and set `ENV Authentication__Authority=https://MyStaticNGrokUrl` in 
DockerfileApi.

Then, you can use `https://MyStaticNGrokUrl/api/login` to request a token and authenticate successfully with the API