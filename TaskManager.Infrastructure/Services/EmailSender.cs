namespace TaskManager.Infrastructure.Services;

public class EmailSender : IEmailSender
{
    private readonly EmailConfigurationSettings _emailConfigurationSettings;
    private readonly IIssueRepository _issueRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IIssueDetailRepository _issueDetailRepository;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(
        IOptionsMonitor<EmailConfigurationSettings> optionsMonitor,
        IIssueRepository issueRepository,
        UserManager<AppUser> userManager,
        IIssueDetailRepository issueDetailRepository,
        ILogger<EmailSender> logger
        )
    {
        _emailConfigurationSettings = optionsMonitor.CurrentValue;
        _issueRepository = issueRepository;
        _userManager = userManager;
        _issueDetailRepository = issueDetailRepository;
        _logger = logger;
    }

    #region Private methods
    private async Task SendAsync(MimeMessage mailMessage)
    {
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_emailConfigurationSettings.SmtpServer, _emailConfigurationSettings.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(_emailConfigurationSettings.UserName, _emailConfigurationSettings.Password);

            await client.SendAsync(mailMessage);
        }
        catch
        {
            //log an error message or throw an exception, or both.
            _logger.LogError("--> Send Email not success");
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true);
            client.Dispose();
        }
    }

    private MimeMessage CreateEmailMessage(EmailMessageDto message, TextFormat textFormat)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(message.From, _emailConfigurationSettings.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;

        emailMessage.Body = new TextPart(textFormat)
        {
            Text = message.Content
        };

        return emailMessage;
    }

    private async Task<IReadOnlyCollection<string>> GetEmailsByNotificationConfig(Guid issueId, NotificationEventViewModel notificationEventViewModel)
    {
        if (notificationEventViewModel is null)
        {
            return new List<string>();
        }

        var userIds = new List<Guid>();
        if (notificationEventViewModel.AllWatcher)
        {
            var watcherIds = await _issueRepository.GetAllWatcherOfIssue(issueId);
            userIds.AddRange(watcherIds!);
        }
        if (notificationEventViewModel.CurrentAssignee)
        {
            var currentAssigneeId = await _issueDetailRepository.GetCurrentAssignee(issueId);
            if (currentAssigneeId is Guid id)
            {
                userIds.Add(id);
            }

        }
        if (notificationEventViewModel.Reporter)
        {
            var reporterId = await _issueDetailRepository.GetReporter(issueId);
            userIds.Add(reporterId);
        }
        if (notificationEventViewModel.ProjectLead)
        {
            var projectLeadId = await _issueRepository.GetProjectLeadIdOfIssue(issueId);
            userIds.Add(projectLeadId);
        }

        userIds = userIds.Distinct().ToList();
        var emails = await _userManager.Users.Where(u => userIds.Contains(u.Id)).Select(u => u.Email!).ToListAsync();

        return emails;
    }
    #endregion

    public async Task SendEmailAsync(EmailMessageDto message, TextFormat textFormat = TextFormat.Text)
    {
        var mailMessage = CreateEmailMessage(message, textFormat);

        await SendAsync(mailMessage);
    }

    public async Task SendEmailWhenUpdateIssue(Guid issueId, string subjectOfEmail, Guid from, BuidEmailTemplateBaseDto buidEmailTemplateBase, NotificationEventViewModel notificationEventViewModel)
    {
        var emails = await GetEmailsByNotificationConfig(issueId, notificationEventViewModel);

        if (emails.Any())
        {
            var senderName = await _userManager.Users.Where(u => u.Id == from).Select(u => u.Name).FirstOrDefaultAsync();

            string content = BuildEmailTemplateConstants.BuildEmailTemplate(buidEmailTemplateBase);

            var emailMessageDto = new EmailMessageDto(emails, subjectOfEmail, content, senderName!);
            await SendEmailAsync(emailMessageDto, TextFormat.Html);
        }
    }

    public async Task SendEmailWhenCreatedIssue(Guid issueId, string subjectOfEmail, Guid from, BuidEmailTemplateBaseDto buidEmailTemplateBase, NotificationEventViewModel notificationEventViewModel)
    {
        var emails = await GetEmailsByNotificationConfig(issueId, notificationEventViewModel);

        if (emails.Any())
        {
            var senderName = await _userManager.Users.Where(u => u.Id == from).Select(u => u.Name).FirstOrDefaultAsync();

            string content = BuildEmailTemplateConstants.BuildEmailTemplate(buidEmailTemplateBase);

            var emailMessageDto = new EmailMessageDto(emails, subjectOfEmail, content, senderName!);
            await SendEmailAsync(emailMessageDto, TextFormat.Html);
        }
    }
}