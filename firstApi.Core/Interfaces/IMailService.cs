using System;
using System.Collections.Generic;
using System.Text;

namespace firstApi.Core.Interfaces
{
    public interface IMailService
    {
        void Send(string subject, string message);
    }
}
