using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MAD.Procore.RecurringStudioHoursUpload.Services
{
    public class NamelyDbConnectionFactory
    {
        private readonly AppConfig appConfig;

        public NamelyDbConnectionFactory(AppConfig appConfig)
        {
            this.appConfig = appConfig;
        }

        public IDbConnection Create()
        {
            return new SqlConnection(this.appConfig.NamelyConnectionString);
        }
    }
}
