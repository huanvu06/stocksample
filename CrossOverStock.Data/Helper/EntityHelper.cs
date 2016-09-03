using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossOverStock.Data.Helper
{
    public static class EntityHelper
    {
        public static string ConnectionStringBuilder(string DalPathName, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["StockContext"].ConnectionString;
            HttpHelper.WriteLogError(connectionString);
            var ecsbuilder = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder();
            ecsbuilder.Provider = "System.Data.SqlClient";
            //ecsbuilder.Provider = "System.Data.EntityClient";
            ecsbuilder.ProviderConnectionString = connectionString;

            // Becare full the folder structure: General.Data or Data only in project folder tree.
            ecsbuilder.Metadata = @"res://*/" + DalPathName + ".csdl|res://*/" + DalPathName + ".ssdl|res://*/" + DalPathName + ".msl";
            string cs = ecsbuilder.ToString();
            HttpHelper.WriteLogError("Built connectionstring: " + cs);
            return cs;
        }
    }
}
