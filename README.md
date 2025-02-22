# Agrigate

Agrigate is a cross-platform application that helps growers collect, manage, and analyze agricultural data and run a 
more efficient, profitable farm or garden.

## Project Structure

Agrigate is a mono-repo that contains all code needed to build and run the  application. The project consists of the 
following items:

- **Agrigate.App**: The client application, built with Blazor Server and Electron.Net
- **Docs**: A Docusaurus app that hosts documentation for Agrigate via GitHub Pages

## Getting Started

### Prerequisites

Before development can begin, the following items must be installed:

- [.Net 8 SDK](https://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu-install?tabs=dotnet9&pivots=os-linux-ubuntu-2410)
- Electron.NET CLI `dotnet tool install ElectronNET.CLI -g`
- EF Core CLI `dotnet tool install --global dotnet-ef`

### Font Awesome

Agrigate uses FontAwesome Pro icons, which are excluded from the repo via `.gitignore`. To include icons locally, 
[download the pro fonts](https://fontawesome.com/account/general), then add the `css` and `webfonts` directories to 
`Agrigate.App/wwwroot/FontAwesome`

### Electron Debugging

To debug the application via electron, navigate to the Aggrigate.App directory using the terminal, and run 
`electronize start`. Then, from within Rider, click "Debug" -> "Attach to Process"

## Documentation

Full documentation for agrigate can be found on
[GitHub Pages](https://demetrioz.github.io/Agrigate/).
