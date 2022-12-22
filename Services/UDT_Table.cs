using B1Framework.B1Frame;
using System;

namespace bagant.Services
{
    class UDT_Table
    {
        public static void AddRecordUDT(string tabla, int docnum, string[] field, object[] value)
        {
            try
            {
                var userDefinedTable = B1Connections.DiCompany.UserTables.Item(tabla);
                    userDefinedTable.Name = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    userDefinedTable.UserFields.Fields.Item("U_BGT_DocDate").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    userDefinedTable.UserFields.Fields.Item("U_BGT_DocNum").Value = docnum;

                for ( int i=0; i<field.Length; i++ )
                    userDefinedTable.UserFields.Fields.Item(field[i]).Value = value[i];

                var respCod = userDefinedTable.Add();
            }
            catch (Exception ex)
            {
                Log.Error("Exception (AddFieldUDT) => " + ex.Message);
            }
        }

        public static void RemoveRecordUDT(string tabla, int docnum)
        {
            try
            {
                var userDefinedTable = B1Connections.DiCompany.UserTables.Item(tabla);
                if (userDefinedTable.GetByKey(docnum.ToString()))
                {
                    var respCod = userDefinedTable.Remove();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Exception (AddFieldUDT) => " + ex.Message);
            }
        }

        public static void AddRecordUDT(string tabla, string[] field, object[] value)
        {
            try
            {
                var userDefinedTable = B1Connections.DiCompany.UserTables.Item(tabla);
                    userDefinedTable.Name = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                for (int i = 0; i < field.Length; i++)
                    userDefinedTable.UserFields.Fields.Item(field[i]).Value = value[i];

                var respCod = userDefinedTable.Add();
            }
            catch (Exception ex)
            {
                Log.Error("Exception (AddFieldUDT) => " + ex.Message);
            }
        }

    }
}
