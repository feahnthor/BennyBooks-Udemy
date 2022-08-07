using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennyBooks.Utility
{
    // Created to help resolve the issue that pops up
    // InvalidOperationException: Unable to resolve service for type 'Microsoft.AspNetCore.Identity.UI.Services.IEmailSender' while attempting to activate 'BennyBooksWeb.Areas.Identity.Pages.Account.RegisterModel'
    public class EmailSender : IEmailSender // Fake Implementation of EmailSender to resolve default issue in \Identity\Pages\Account\register.cshtml.cs,
                                            // needs to be used in Program.cs
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}
