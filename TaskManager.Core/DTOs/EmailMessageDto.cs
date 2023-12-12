using MimeKit;

namespace TaskManager.Core.DTOs;

public class EmailMessageDto
{
    public List<MailboxAddress> To { get; set; }
    public string? Subject { get; set; }
    public string? Content { get; set; }
    public string From { get; set; }
    public EmailMessageDto(IEnumerable<string> to, string subject, string content, string from)
    {
        To = new List<MailboxAddress>();
        To.AddRange(to.Select(x => new MailboxAddress(x, x)));
        Subject = subject;
        Content = content;
        From = from;
    }
}

public class EmailModel
{
    public string? To { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
}
