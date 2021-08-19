using Dapper;
using MAD.Integration.Common;
using MAD.Procore.StudioHoursUpload.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MAD.Procore.StudioHoursUpload.Services
{
    public class StudioHourClient
    {
        private readonly NamelyDbConnectionFactory namelyDbConnectionFactory;
        private readonly ProcoreConfig procoreConfig;

        public StudioHourClient(NamelyDbConnectionFactory namelyDbConnectionFactory, ProcoreConfig procoreConfig)
        {
            this.namelyDbConnectionFactory = namelyDbConnectionFactory;
            this.procoreConfig = procoreConfig;
        }

        public async Task<IEnumerable<vwStudioHoursByRegionAndCountry>> GetStudioHours()
        {
            var querySqlPath = Path.Combine(Globals.BaseDirectory, "Queries\\NamelyStudioHourQuery.sql");
            var querySql = await File.ReadAllTextAsync(querySqlPath);

            using var namelyDbConnection = this.namelyDbConnectionFactory.Create();
            namelyDbConnection.Open();

            return await namelyDbConnection.QueryAsync<vwStudioHoursByRegionAndCountry>(querySql, new { Region = procoreConfig.Name });
        }
    }
}
