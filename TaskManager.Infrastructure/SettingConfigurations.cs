﻿namespace TaskManager.Infrastructure;
public class JwtSettings
{
    /// <summary>
    /// Audience jwt
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Issuer jwt
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Khóa bảo mật jwt
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Thời gian hết hạn của token
    /// </summary>
    public string Subject { get; set; } = string.Empty;
}

public class CloudinarySettings
{
    public string? CloudName { get; set; }
    public string? ApiKey { get; set; }
    public string? ApiSecret { get; set; }
}

public class SftpServerSettings
{
    public string Localhost { get; set; } = string.Empty;
    public int Port { get; set; } = 2222;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class ElasticConfigurationSettings
{
    public string Uri { get; set; } = string.Empty;
}

public class EmailConfigurationSettings
{
    public string From { get; set; } = string.Empty;
    public string SmtpServer { get; set; } = string.Empty;
    public int Port { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class FileShareSettings
{
    public string FileShareName { get; set; } = string.Empty;
    public string ConnectionStrings { get; set; } = string.Empty;
}


public class BlobContainerSettings
{
    public string ConnectionStrings { get; set; } = string.Empty;
}

public class TextToImageAISettings
{
    public string APIUrl { get; set; } = string.Empty;
    public string APIKey { get; set; } = string.Empty;
}