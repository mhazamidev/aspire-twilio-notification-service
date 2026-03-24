using TwilioAspireDemo.AppHost.Infrastructure.Databases;
using TwilioAspireDemo.AppHost.Services;

var builder = DistributedApplication.CreateBuilder(args);

#region parameters

var sqlPassword = builder.AddParameter("SqlPassword", secret: true);

var accountSid = builder.AddParameter("AccountSid", secret: true);
var authToken = builder.AddParameter("AuthToken", secret: true);
var phoneNumber = builder.AddParameter("PhoneNumber", secret: true);
var sendGridApiKey = builder.AddParameter("SendGridApiKey", secret: true);
var verifyServiceSid = builder.AddParameter("VerifyServiceSid", secret: true);
var mqUsername = builder.AddParameter("RabbitMQUsername", secret: true);
var mqPassword = builder.AddParameter("RabbitMQPassword", secret: true);

#endregion

#region Databases
var sql = builder.AddIdentityDatabase(password: sqlPassword);
#endregion

#region Services
var rabbitmq = builder.AddRabbitMQService(mqUsername, mqPassword);

builder.AddNotificationService(sql, rabbitmq, accountSid, authToken, phoneNumber, sendGridApiKey, verifyServiceSid);

builder.AddNotificationWorkerService(sql, rabbitmq);

#endregion


builder.Build().Run();
