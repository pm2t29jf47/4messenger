using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Data.SqlClient;
using System.Data;
using ServerSideExceptionHandler;

namespace DataSourceLayer
{
    public class RecipientGateway : Gateway
    {
        public void InsertRecipient(Recipient recipient)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("insert_recipient", sqlConnection))
                {
                    PrepareIR(cmd, recipient);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                new ExceptionHandler().HandleExcepion(ex);
            }
        }

        private void PrepareIR(SqlCommand cmd, Recipient recipient)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            CreateIRParameters(cmd);
            SetIRParameters(cmd, recipient);
        }

        private void CreateIRParameters(SqlCommand cmd)
        {
            cmd.Parameters.Add(new SqlParameter("@employeeId", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@messageId", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@deleteByRecipient", SqlDbType.Bit));
        }

        private void SetIRParameters(SqlCommand cmd, Recipient recipient)
        {
            cmd.Parameters["@employeeId"].Value = recipient.EmployeeId;
            cmd.Parameters["@messageId"].Value = recipient.MessageId;
            cmd.Parameters["@deleteByRecipient"].Value = recipient.DeleteByRecipient;
        }        
    }
}
