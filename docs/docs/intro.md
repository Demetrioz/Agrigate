# Introduction

Agrigate is a platform that collects, manages, and analyzes all of your
agricultural data and helps you run a more efficient, profitable farm.

## How to use Agrigate

Lets take a look at the three ways that you can utilize Agrigate:

1. Run everything locally on your device (Coming soon!)
2. Install and connect to a local, self-managed server (Coming soon!)
3. Use a paid hosting service (Coming soon!)

Regardless of which option you choose, you must first download the app from
the app store on Android, iOS, Mac, Windows, or Linux. You can also download
the binary from the GitHub release or build it from source.

### Local Only (Coming Soon!)

If you choose to do so, you can run agrigate exclusively on your local device.
This is the easiest, but most limited, way to get started. After downloading and
installing Agrigate, choose "Local Only" when starting the app for the first
time.

This will keep all data local to your device, but you loose certain network
functionality such as accessing your information from multiple devices and
capturing telemetry from IoT devices. Changing to a new device will require
exporting your data and importing it on the new device.

### Self-Managed (Coming Soon!)

The second option is installing and running an Agrigate server locally or via
a cloud provider. This will allow you to connect to and manage IoT devices,
run jobs, and be able to share data between multiple devices.

You'll install the server, then download the app as normal, choose "Server", and
enter your server's address.

### Hosted (Coming Soon!)

Using a hosted service is the same as running on self-managed server, except you
don't have to manage the server yourself. When downloading the app, select
"Hosted" and choose a payment plan.

## Technical Details

Agrigate is an entire platform with a client application, optional server, and
a collection of devices to help make growing your own food easier.

### Dependencies

Agrigate consists of several components. The client is is built with
[Flutter](https://flutter.dev/), while the server is built using
[.Net](https://dotnet.microsoft.com/en-us/) and physical devices utilize
[CircuitPython](https://circuitpython.org/).

### Versioning

All components within Agrigate utilize
[semantic versioning](https://semver.org/) and follow the
\{MAJOR\}.\{MINOR\}.\{PATCH\} format. Release information can be found via the
[Releases](https://demetrioz.github.io/Agrigate/releases/0.1.0) page

## Contact Information

Agrigate is developed and maintained by Kevin Williams. For assistance, reach
out to via [email](mailto:kevin@ktech.industries)

Have you encountered a bug?
[Let me know!](https://github.com/Demetrioz/Agrigate/issues)
