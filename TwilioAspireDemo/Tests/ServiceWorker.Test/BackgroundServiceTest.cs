using BuildingBlocks.Contracts.DTO;
using BuildingBlocks.Contracts.Notification.Contracts;
using BuildingBlocks.Utility;
using Microsoft.Extensions.Logging;
using Moq;
using Notification.Domain.MessageLogs.Enums;
using Notification.Worker.Factories;
using Notification.Worker.Interfaces;
using Notification.Worker.Processors;

namespace ServiceWorker.Test
{
    public class BackgroundServiceTest
    {
        [Fact]
        public async Task ProcessAsync_Should_Use_Correct_Handler()
        {
            // Arrange
            var handlerMock = new Mock<INotificationHandler>();

            var factoryMock = new Mock<INotificationHandlerFactory>();

            factoryMock.Setup(x =>
                x.GetHandler(MessageChannel.Sms))
                .Returns(handlerMock.Object);

            var loggerMock = new Mock<ILogger<NotificationProcessor>>();

            var processor = new NotificationProcessor(factoryMock.Object, loggerMock.Object);

            var envelope = new NotificationEnvelope
            {
                Payload = new NotificationDto
                {
                    Channel = "Sms"
                }
            };

            // Act
            await processor.ProcessAsync(envelope);

            // Assert
            handlerMock.Verify(x =>
                x.HandleAsync(It.IsAny<NotificationDto>()),
                Times.Once);
        }

        [Fact]
        public async Task ProcessAsync_Should_Send_Email_Handler()
        {
            // Arrange
            var handlerMock = new Mock<INotificationHandler>();

            var factoryMock = new Mock<INotificationHandlerFactory>();

            factoryMock.Setup(x =>
                x.GetHandler(MessageChannel.Email))
                .Returns(handlerMock.Object);


            var loggerMock = new Mock<ILogger<NotificationProcessor>>();

            var processor = new NotificationProcessor(factoryMock.Object, loggerMock.Object);

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
            handlerMock.Verify(x =>
                x.HandleAsync(It.Is<NotificationDto>(cmd =>
                    cmd.Recipient == "test@test.com" &&
                    cmd.Content == "hello"&&
                    cmd.Channel == "Email")),
                Times.Once);
        }

        [Fact]
        public async Task ProcessAsync_Should_Call_Sms_Handler()
        {
            // Arrange
            var handlerMock = new Mock<INotificationHandler>();

            var factoryMock = new Mock<INotificationHandlerFactory>();

            factoryMock.Setup(x =>
                x.GetHandler(MessageChannel.Sms))
                .Returns(handlerMock.Object);

            var loggerMock = new Mock<ILogger<NotificationProcessor>>();

            var processor = new NotificationProcessor(factoryMock.Object, loggerMock.Object);

            var envelope = new NotificationEnvelope
            {
                Payload = new NotificationDto
                {
                    Recipient = "09123456789",
                    Content = "code",
                    Channel = "Sms"
                }
            };

            // Act
            await processor.ProcessAsync(envelope);

            // Assert
            handlerMock.Verify(x =>
                x.HandleAsync(It.Is<NotificationDto>(d =>
                    d.Recipient == "09123456789" &&
                    d.Content == "code")),
                Times.Once);
        }
        [Fact]
        public async Task ProcessAsync_Should_Send_Otp_Command()
        {
            var handlerMock = new Mock<INotificationHandler>();

            var factoryMock = new Mock<INotificationHandlerFactory>();

            factoryMock.Setup(x =>
                x.GetHandler(MessageChannel.Otp))
                .Returns(handlerMock.Object);


            var loggerMock = new Mock<ILogger<NotificationProcessor>>();

            var processor = new NotificationProcessor(factoryMock.Object, loggerMock.Object);

            var envelope = new NotificationEnvelope
            {
                Payload = new NotificationDto
                {
                    Recipient = "09123456789",
                    Channel = "Otp"
                }
            };

            await processor.ProcessAsync(envelope);

            handlerMock.Verify(x =>
                x.HandleAsync(It.Is<NotificationDto>(cmd =>
                    cmd.Recipient == "09123456789" &&
                    cmd.Channel == "Otp")),
                Times.Once);
        }

        [Fact]
        public async Task ProcessAsync_Should_Throw_When_Channel_Invalid()
        {
            // Arrange
            var handlerMock = new Mock<INotificationHandler>();

            var factoryMock = new Mock<INotificationHandlerFactory>();

            factoryMock.Setup(x =>
                x.GetHandler(MessageChannel.Sms))
                .Returns(handlerMock.Object);

            var loggerMock = new Mock<ILogger<NotificationProcessor>>();

            var processor = new NotificationProcessor(factoryMock.Object, loggerMock.Object);

            var envelope = new NotificationEnvelope
            {
                Payload = new NotificationDto
                {
                    Recipient = "test",
                    Content = "hello",
                    Channel = "Invalid"
                }
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                processor.ProcessAsync(envelope));

            handlerMock.Verify(x =>
                x.HandleAsync(It.IsAny<NotificationDto>()),
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
