using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_mvc.Models;

namespace dotnet_mvc.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(Message message);

    }
}