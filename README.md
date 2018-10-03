# SeqrUs
A small ASP.NET Core MVC site to demonstrate some OWASP vulnerabilities and how to fix them.

The security settings of the app can be altered in runtime and allows users to investigate how the different security flaws can be tested/experienced depending on the current settings.

## How to build and run

0. Ensure you have dotnet Core 2.0 SDK
0. Clone the repo
0. `dotnet run --project Seqrus.Web`
0. While running the app, you can browse to `/Config` to change the security settings. Note that if you do, you'll need to start the server process again.

## Latest build
The latest (master) build is available at https://seqrus.azurewebsites.net
