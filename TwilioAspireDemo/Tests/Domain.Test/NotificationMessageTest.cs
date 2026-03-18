using Domain.SeedWork.Exceptions;
using FluentAssertions;
using Notification.Domain.MessageLogs.Entities;
using Notification.Domain.MessageLogs.Enums;

namespace Domain.Test
{
    public class NotificationMessageTest
    {
        [Fact]
        public void Create_Should_Throw_When_Recipient_Is_Empty()
        {
            Action action = () => NotificationMessage.Create("", "test", MessageChannel.Email);

            action.Should()
                .Throw<DomainException>()
                .WithMessage("Recipient cannot be null or empty.");
        }

        [Fact]
        public void Create_Should_Throw_When_Content_Is_Empty()
        {
            Action action = () => NotificationMessage.Create("test@test.com", "", MessageChannel.Email);

            action.Should()
                .Throw<DomainException>()
                .WithMessage("Content cannot be null or empty.");
        }
    }
}
