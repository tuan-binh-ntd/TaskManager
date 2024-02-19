namespace TaskManager.Infrastructure.Services;

public class EmailSender(
    IOptionsMonitor<EmailConfigurationSettings> optionsMonitor,
    IIssueRepository issueRepository,
    UserManager<AppUser> userManager,
    IIssueDetailRepository issueDetailRepository,
    ILogger<EmailSender> logger
        ) : IEmailSender
{
    private readonly EmailConfigurationSettings _emailConfigurationSettings = optionsMonitor.CurrentValue;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IIssueDetailRepository _issueDetailRepository = issueDetailRepository;
    private readonly ILogger<EmailSender> _logger = logger;

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
            var watcherIds = await _issueRepository.GetAllWatcherOfIssueAsync(issueId);
            userIds.AddRange(watcherIds!);
        }
        if (notificationEventViewModel.CurrentAssignee)
        {
            var currentAssigneeId = await _issueDetailRepository.GetCurrentAssigneeIdAsync(issueId);
            if (currentAssigneeId is Guid id)
            {
                userIds.Add(id);
            }

        }
        if (notificationEventViewModel.Reporter)
        {
            var reporterId = await _issueDetailRepository.GetReporterIdAsync(issueId);
            userIds.Add(reporterId);
        }
        if (notificationEventViewModel.ProjectLead)
        {
            var projectLeadId = await _issueRepository.GetProjectLeadIdOfIssueAsync(issueId);
            userIds.Add(projectLeadId);
        }

        userIds = userIds.Distinct().ToList();
        var emails = await _userManager.Users.Where(u => userIds.Contains(u.Id)).Select(u => u.Email!).ToListAsync();

        return emails;
    }
    #endregion

    private async Task SendEmailAsync(EmailMessageDto message, TextFormat textFormat = TextFormat.Text)
    {
        var mailMessage = CreateEmailMessage(message, textFormat);

        await SendAsync(mailMessage);
    }

    public async Task SendEmailWhenUpdateIssue(Guid issueId, string subjectOfEmail, Guid from, BuidEmailTemplateBaseDto buidEmailTemplateBase, NotificationEventViewModel notificationEventViewModel)
    {
        var emails = await GetEmailsByNotificationConfig(issueId, notificationEventViewModel);

        if (emails.Count != 0)
        {
            var senderName = await _userManager.Users.Where(u => u.Id == from).Select(u => u.Name).FirstOrDefaultAsync();

            string content = BuildEmailTemplateConstants.BuildEmailTemplate(buidEmailTemplateBase);

            var emailMessageDto = new EmailMessageDto(emails, subjectOfEmail, content, senderName!);
            await SendEmailAsync(emailMessageDto, TextFormat.Html);
        }
    }

    public async Task SendEmailWhenCreatedIssue(SendEmailModel sendEmailModel)
    {
        var issue = await _issueRepository.GetByIdAsync(sendEmailModel.IssueId) ?? throw new IssueNullException();
        var emails = await GetEmailsByNotificationConfig(sendEmailModel.IssueId, sendEmailModel.NotificationEventViewModel);

        var senderName = await _userManager.Users
            .Where(u => u.Id == sendEmailModel.FromUserId)
            .Select(u => u.Name)
            .FirstOrDefaultAsync() ?? IssueConstants.None_IssueHistoryContent;


        var projectName = await _issueRepository.GetProjectNameOfIssueAsync(sendEmailModel.IssueId);
        var projectCode = await _issueRepository.GetProjectCodeOfIssueAsync(sendEmailModel.IssueId);

        var avatarUrl = await _userManager.Users
            .Where(u => u.Id == sendEmailModel.FromUserId)
            .Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? TextToImageConstants.AnonymousAvatar;

        sendEmailModel.BuidEmailTemplate.SenderName = senderName;
        sendEmailModel.BuidEmailTemplate.ProjectName = projectName;
        sendEmailModel.BuidEmailTemplate.ProjectCode = projectCode;
        sendEmailModel.BuidEmailTemplate.IssueCode = issue.Code;
        sendEmailModel.BuidEmailTemplate.IssueName = issue.Name;

        if (emails.Count != 0)
        {
            string content = BuildEmailTemplateConstants.BuildEmailTemplate(sendEmailModel.BuidEmailTemplate);

            var emailMessageDto = new EmailMessageDto(emails, sendEmailModel.Subject, content, senderName);
            await SendEmailAsync(emailMessageDto, TextFormat.Html);
        }
    }
}
