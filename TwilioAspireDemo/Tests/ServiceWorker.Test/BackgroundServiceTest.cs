using BuildingBlocks.Contracts.DTO;
using BuildingBlocks.Contracts.Notification.Contracts;
using BuildingBlocks.Utility;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Notification.Application.Features.Email;
using Notification.Application.Features.SentOtp;
using Notification.Application.Features.Sms;
using Notification.Domain.MessageLogs.Enums;
using Notification.Worker.Processors;

namespace ServiceWorker.Test
{
    public class BackgroundServiceTest
    {
        [Fact]
        public async Task ProcessAsync_Should_Send_Email_Command()
        {
            // Arrange
            var senderMock = new Mock<ISender>();
            var loggerMock = new Mock<ILogger<NotificationProcessor>>();

            var processor = new NotificationProcessor(senderMock.Object, loggerMock.Object);

            var envelope = new NotificationEnvelope
            {
                Payload = new NotificationDto
                {
                    Recipient = "test@test.com",
                    Content = "hello",
                    Channel = "Email"
                }
            };

            // Act
            await processor.ProcessAsync(envelope);

            // Assert
            senderMock.Verify(x =>
                x.Send(It.Is<SendEmailCommand>(cmd =>
                    cmd.Email == "test@test.com" &&
                    cmd.Message == "hello")),
                Times.Once);
        }

        [Fact]
        public async Task ProcessAsync_Should_Send_Sms_Command()
        {
            var senderMock = new Mock<ISender>();

            var loggerMock = new Mock<ILogger<NotificationProcessor>>();

            var processor = new NotificationProcessor(senderMock.Object, loggerMock.Object);

            var envelope = new NotificationEnvelope
            {
                Payload = new NotificationDto
                {
                    Recipient = "09123456789",
                    Content = "code",
                    Channel = "Sms"
                }
            };

            await processor.ProcessAsync(envelope);

            senderMock.Verify(x =>
                x.Send(It.Is<SendSMSCommand>(cmd =>
                    cmd.Phone == "09123456789" &&
                    cmd.Message == "code")),
                Times.Once);
        }

        [Fact]
        public async Task ProcessAsync_Should_Send_Otp_Command()
        {
            var senderMock = new Mock<ISender>();
            var loggerMock = new Mock<ILogger<NotificationProcessor>>();

            var processor = new NotificationProcessor(senderMock.Object, loggerMock.Object);

            var envelope = new NotificationEnvelope
            {
                Payload = new NotificationDto
                {
                    Recipient = "09123456789",
                    Channel = "Otp"
                }
            };

            await processor.ProcessAsync(envelope);

            senderMock.Verify(x =>
                x.Send(It.Is<SendOtpCommand>(cmd =>
                    cmd.Recipient == "09123456789")),
                Times.Once);
        }

        [Fact]
        public async Task ProcessAsync_Should_Not_Send_When_Channel_Invalid()
        {
            var senderMock = new Mock<ISender>();

            var loggerMock = new Mock<ILogger<NotificationProcessor>>();

            var processor = new NotificationProcessor(senderMock.Object, loggerMock.Object);

            var envelope = new NotificationEnvelope
            {
                Payload = new NotificationDto
                {
                    Recipient = "test",
                    Content = "hello",
                    Channel = "Invalid"
                }
            };

            await processor.ProcessAsync(envelope);

            senderMock.Verify(x =>
                x.Send(It.IsAny<object>()),
                Times.Never);
        }


        [Theory]
        [InlineData("Email", MessageChannel.Email)]
        [InlineData("email", MessageChannel.Email)]
        [InlineData("EMAIL", MessageChannel.Email)]
        [InlineData("sMs", MessageChannel.Sms)]
        public void GetEnumValue_Should_Return_Enum_When_Valid(string input, MessageChannel expected)
        {
            var result = input.GetEnumValue<MessageChannel>();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetEnumValue_Should_Throw_When_Invalid()
        {
            var input = "invalid";

            Assert.Throws<ArgumentException>(() =>
                input.GetEnumValue<MessageChannel>());
        }

        [Fact]
        public void GetEnumValue_Should_Throw_When_Null()
        {
            string input = null;

            Assert.Throws<ArgumentException>(() =>
                input.GetEnumValue<MessageChannel>());
        }
    }
}
