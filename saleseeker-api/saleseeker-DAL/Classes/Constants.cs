using saleseeker_DAL.Modals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saleseeker_DAL.Classes
{
    public class Constants
    {
        public class SiteSeed 
        {
            //TODO: Build seed data classes to populate the tables with init details
            public static readonly ReadOnlyCollection<Site> SiteList =
                new ReadOnlyCollection<Site>(new[]
               {
                    new Site {
                        Id = 1,
                        Name = "",
                        LogoUrl = "",
                        Url=""
                    },
                    new Site {
                        Id = 2,
                        Name = "",
                        LogoUrl = "",
                        Url=""
                    }
               });
        }
    }
}
