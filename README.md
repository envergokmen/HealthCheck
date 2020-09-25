# Enver Gökmen Demo App

It schedules health cheks for web urls. All logs are in ErrorLogs.json, Serilog is used to track logs, hangfire framework is used to schedule Cron like tasks.
An Auth filter is used to demonstratre auth, it supports multiple users. 
## Consider chancing connection string in appsettings, Hangfire and App uses the same database

```json
  "ConnectionStrings": {
    "HealthDbCon": "****"
  },
```

## I've created a few test for the app 
HealthCheck.Tests


## You can add bulk test items at the app starts, it will add 4 test urls for 1 min.
It will ask if you don't have any app only, yello button after login.
