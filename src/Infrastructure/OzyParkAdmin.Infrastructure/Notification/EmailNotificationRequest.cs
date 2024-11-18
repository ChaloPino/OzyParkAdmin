using System.Collections.Immutable;

namespace OzyParkAdmin.Infrastructure.Notification;

internal sealed record NotificationRequest(ImmutableArray<EmailNotificationRequest> Emails, ImmutableArray<WhatsappNotificationRequest> Whatsapps);
internal sealed record EmailNotificationRequest(
    string Template,
    ImmutableArray<MailboxAddress> ToAddresses,
    ImmutableArray<MailboxAddress> CcAddresses,
    ImmutableArray<MailboxAddress> BccAddresses,
    string? Language,
    IDictionary<string, DataItem> Data,
    ImmutableArray<AttachmentData> Attachments);

internal sealed record WhatsappNotificationRequest(
    string Template,
    string ToPhoneNumber,
    string? Language,
    IDictionary<string, DataItem> Data,
    ImmutableArray<AttachmentData> Attachments);

internal sealed record MailboxAddress(string Address, string DisplayName);

internal sealed record DataItem(object Value, string Type);

internal sealed record AttachmentData(string Name, byte[] Content, string ContentType);