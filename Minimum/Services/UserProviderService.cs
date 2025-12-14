using Minimum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.Services
{
    public class UserProviderService
    {
        public UserProviderService() 
        {
            CurrentUser = new User();
        }
        public User CurrentUser { get; set; }
    }
}
