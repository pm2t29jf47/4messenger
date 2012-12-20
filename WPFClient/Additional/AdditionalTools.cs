using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using Entities.Additional;
using System.Threading;
using System.Collections;
using DBService;
using System.ComponentModel;
using System.ServiceModel;
using ServiceInterface;
using System.Data.SqlTypes;

namespace WPFClient.Additional
{
    /// <summary>
    /// im watching u!
    /// </summary>
    class AdditionalTools
    {
        public static byte[] GetMaxTimestamp(List<Entity> Entities)
        {
            byte[] currentMax = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            SqlBinary sqlCurrentMax = new SqlBinary(currentMax);
            SqlBinary sqlCurrent;
            foreach (Entity item in Entities)
            {
                sqlCurrent = new SqlBinary(item.Version);
                if (sqlCurrentMax.CompareTo(sqlCurrent) < 0)
                    sqlCurrentMax = sqlCurrent;
            }
            return sqlCurrentMax.Value;
        }
    }
}