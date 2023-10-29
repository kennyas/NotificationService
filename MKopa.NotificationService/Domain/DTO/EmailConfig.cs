using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKopaMessageBox.Domain.DTOs
{
    public class EmailConfig
    {
        public EmailConfig() { }

        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string SmtpService { get; set; }
        public string SmtpEmailId { get; set; }
        public string SmtpPassword { get; set; }
        public int SmtpPort { get; set; }
        public int SmtpTransactionalPort { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpFailoverPort { get; set; }
        public int SmtpFailoverTransactionalPort { get; set; }
        public string SmtpFailoverHost { get; set; }
        public bool IsDevelopment { get; set; }
    }
}
