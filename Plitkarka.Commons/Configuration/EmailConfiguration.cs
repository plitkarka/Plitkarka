﻿namespace Plitkarka.Commons.Configuration;

public record EmailConfiguration
{
    public bool ShouldSendEmails { get; set; } = false;
    public string DisplayName { get; set; }
    public string From { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
}
