using NAGGLE.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.DAL.Entities
{
    // Overrided the save changes method
    public partial class NaggleEntities
    {
        private string[] excludeProperties = new string[] { "CreatedDate", "CreatedBy" };
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                #region Set Log Info Details
                //Set the log info details
                if (entry.Entity is ILogInfo)
                {
                    if (entry.State == EntityState.Added)
                    {
                        ((ILogInfo)entry.Entity).CreatedDate = DateTime.UtcNow;
                        ((ILogInfo)entry.Entity).UpdatedDate = null;
                        ((ILogInfo)entry.Entity).UpdatedBy = null;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        foreach (string property in excludeProperties)
                        {
                            entry.Property(property).IsModified = false;
                        }
                        ((ILogInfo)entry.Entity).UpdatedDate = DateTime.UtcNow;
                    }
                }
                #endregion
            }
            return base.SaveChanges();
        }
    }

    public partial class Account : IFlagRemove, ILogInfo
    {
    }

    public partial class Company : IFlagRemove, ILogInfo
    {
    }
    public partial class User : IFlagRemove, ILogInfo
    {
    }
    public partial class Attribute : IFlagRemove, ILogInfo
    {
    }
    public partial class Product : IFlagRemove, ILogInfo
    {
    }
    public partial class ProductAccountPrice :  ILogInfo
    {
    }
    public partial class ProductPricing :  ILogInfo
    {
    }


}
