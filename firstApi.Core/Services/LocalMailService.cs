using firstApi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace firstApi.Core.Services
{
    public class LocalMailService : IMailService
    {
        private string _mailTo = "admin@example.com";
        private string _mailFrom = "noreply@example.com";

        public void Send(string subject, string message)
        {
            Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with LocalMailService.");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }
}
