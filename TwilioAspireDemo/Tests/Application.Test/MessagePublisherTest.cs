using BuildingBlocks.Contracts.DTO;
using BuildingBlocks.Messaging;
using Moq;
using Notification.Application.Services;
using Notification.Infrastructure.Persistence.Interfaces;

namespace Application.Test
{
    public class MessagePublisherTest
    {
        [Fact]
        public async Task SendEmailAsync_Should_Call_Publisher_With_Correct_Data()
        {
            // Arrange
            var publisherMock = new Mock<INotificationPublisher>();

            var service = new NotificationService(publisherMock.Object);

            var to = "test@test.com";
            var content = "hello";

            // Act
            await service.SendEmailAsync(to, content);

            // Assert
            publisherMock.Verify(x =>
                x.PublishAsync(
                    It.Is<NotificationDto>(d =>
                        d.Recipient == to &&
                        d.Content == content &&
                        d.Channel == "email"),
                    RoutingKeys.Email),
                Times.Once);
        }
    }
}
