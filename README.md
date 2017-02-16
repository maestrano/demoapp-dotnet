# Maestrano .NET DemoApp

This application shows examples of how to use the Maestrano .NET API.
That may be tested from our sandbox environment: https://sandbox.maestrano.com

Please follow the documentation described here: https://maestrano.atlassian.net/wiki/display/DEV/Integrate+your+app+on+partner%27s+marketplaces

## Running the application locally

### What you'll need
- Visual Studio Express for Web (see https://maestrano.atlassian.net/wiki/display/DEV/.NET+Development+Setup)

### Guide

Go to https://developer.maestrano.com

Create a new Application, and a new Environment. Make sure your environment is associated to the Marketplace: Maestrano Singapore.

Retrieve your API Key and Secret

Set up the App endpoints:
- Host: http://localhost:8080
- SSO Init Path: /maestrano/init/?marketplace=%{marketplace}
- SSO Consume Path: /maestrano/consume/?marketplace=%{marketplace}

Set these environment variables, you may use [Rapid Environment Editor](https://www.rapidee.com)
```
MNO_DEVPL_HOST=https://developer.maestrano.com
MNO_DEVPL_API_PATH=/api/config/v1/marketplaces
MNO_DEVPL_ENV_NAME=<your environment nid>
MNO_DEVPL_ENV_KEY=<your environment key>
MNO_DEVPL_ENV_SECRET=<your environment secret>
```

Open Visual Studio Express for Web (type "VS Express" in search bar)
Click on File > Open Project (or File > Recent Projects and Solutions if you have already opened it before)
Open MnoDemoApp.sln (VS Solution file)
You can click on the Play icon at the top to launch the web application and open it in the default browser

Go to the platform sandbox:
https://sandbox.maestrano.com

Add your App from the marketplace

Start the application
