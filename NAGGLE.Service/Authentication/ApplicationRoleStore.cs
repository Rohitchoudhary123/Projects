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
    public class ApplicationRoleStore : RoleStore<ApplicationRole, int, ApplicationUserRole>, IQueryableRoleStore<ApplicationRole, int>, IRoleStore<ApplicationRole, int>, IDisposable
    {
        public ApplicationRoleStore(IdentityDBContext context)
            : base(context)
        {
        }
    }
}
