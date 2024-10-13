# Client

The client application is built with Flutter and can run on multiple platforms
including Linux, Mac, Windows, web, Android, and iOS. Currently, only android
Snap (Linux) versions are available pre-built.

## Linux

Coming Soon!

## Android

To run the android version of Agrigate, you can side-load an APK on your device.

:::info

Since Google's policy for adding a new app to the play store isn't feasable for
me at the moment, the app will be available via APKs on the GitHub
[releases page](https://github.com/Demetrioz/Agrigate/releases)

:::

### Download the APK

On your android device, download the appropriate APK from the
[releases page](https://github.com/Demetrioz/Agrigate/releases)

### Install the APK

Once downloaded, you can install the device by tapping the downloaded APK and
allowing any needed permissions

## Web

:::info

Once login functionality is added to the client, the web-based version will be
included in the docker-compose file by default, and these steps will no
longer be nescessary

:::

To run a web-based version of Agrigate, you can use the included Dockerfile to
run a containerized application.

### Clone The Repo

Clone the agrigate repo to your local machine

```
git clone https://github.com/Demetrioz/Agrigate.git
```

### Build the Docker Image

Next, navigate to the agrigate driectory and build the image using the included
Dockerfile

```
cd ./agrigate
docker build -t agrigate-web .
```

### Start the Container

Once the docker build has completed, you can then start the container

```
docker run -d -p 8080:80 --name agrigate agrigate-web
```
