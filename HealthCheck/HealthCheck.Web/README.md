# Enver Gökmen Demo App

It schedules health cheks for web urls. All logs are in ErrorLogs.json Serilog is used to track logs, hangfire framework is used to schedule Cron like tasks.

## Consider chancing connection string, Hangfire and App uses the same database

```json
{
	"ConnectionStrings":"2030-10-10",
	"key":"YourKeyHere",
	"value":"Your Value Here"
}
```

## I've created a few test for the app 

HealthCheck.Tests
