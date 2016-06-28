using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NAGGLE.DAL;
using NAGGLE.DAL.Entities;
using NAGGLE.DAL.Entities.IddentityDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Service.Authentication
{
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, int, ApplicationUserLogin,
        ApplicationUserRole, ApplicationUserClaim>, IUserStore<ApplicationUser, int>, IDisposable
    {
        public ApplicationUserStore(IdentityDBContext context)
            : base(context)
        {

        }

    }
}
