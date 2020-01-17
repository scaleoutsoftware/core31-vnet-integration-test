# .NET Core TcpClient Connection Test

This repository contains projects for troubleshooting TCP connectivity
issues we have experienced when running an ASP.NET Core 3.1 web
application with VNet integration for accessing Azure VMs in our cloud
infrastructure.

We have had no problems with .NET Core 2.x applications, but .NET Core
3.1 apps throw an exception from TcpClient.Connect() stating "An
attempt was made to access a socket in a way forbidden by its access
permissions."

The solution contains the following projects:

- EchoServer: A simple TCP echo server (.NET Framework 4.6.2),
  intended to be run on an Azure virtual machine.
- EchoClient: A .NET Standard 2.0 library with a Test() method that
  connects to the echo server, sends "Hello world!" over the network,
  and then closes the connection.
- Two ASP.NET Core web applications (a .NET Core 2.1 version and a
  .NET Core 3.1 version), intended to be run in as Azure App Services
  with VNET integration to connect to the EchoServer VM. These two web
  apps are boilerplate projects, except their Home Controllers try to
  use the EchoClient library to connect to the EchoServer service.
