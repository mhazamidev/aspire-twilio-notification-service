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

#endregion

#region Databases
var sql = builder.AddIdentityDatabase(password: sqlPassword);
#endregion

#region Services

builder.AddNotificationService(sql, accountSid, authToken, phoneNumber, sendGridApiKey, verifyServiceSid);


#endregion


builder.Build().Run();
