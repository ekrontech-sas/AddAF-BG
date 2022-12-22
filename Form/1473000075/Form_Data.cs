using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System;
using System.Xml;

namespace bagant.Form._1473000075
{
    class Form_Data : B1Form
    {
        public Form_Data()
        {
            FormType = "1473000075";
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_ADD, false)]
        public virtual void OnAfterFormDataAdd(BusinessObjectInfo pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var asset = ((CheckBox)Form.Items.Item("Item_2").Specific).Checked;

            if (pVal.ActionSuccess && asset)
            {
                var xml = new XmlDocument();
                    xml.LoadXml(pVal.ObjectKey);


                new SAPServices().CreateInventoryItem(xml.InnerText);
            }
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_LOAD, false)]
        public virtual void OnAfterFormDataLoad(BusinessObjectInfo pVal)
        {
            try
            {
                Form = new B1Forms(pVal.FormUID);

                var xml = new XmlDocument();
                xml.LoadXml(pVal.ObjectKey);
                var queryMovement = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetMovementAssetItem.sql", GetType().Assembly), xml.InnerText);

                Log.Info("queryAssetMovement " + queryMovement);

                var grid = Form.Items.Item("Item_3");
                Log.Info("queryAssetMovement (43)");
                Form.DataSources.DataTables.Item("DT_Mov").ExecuteQuery(queryMovement);
                Log.Info("queryAssetMovement (44)");

                if (string.IsNullOrEmpty(Form.DataSources.DataTables.Item("DT_Mov").GetValue(0, 0).ToString()))
                {
                    Log.Info("queryAssetMovement (45)");
                    Form.DataSources.DataTables.Item("DT_Mov").ExecuteQuery("SELECT 'No existen registros coincidentes' AS \"Mensaje\" FROM DUMMY");
                    Log.Info("queryAssetMovement (46)");
                    grid.Enabled = false;
                }
                else
                {
                    Log.Info("queryAssetMovement (47)");
                    grid.Enabled = true;

                    ((Grid)grid.Specific).Columns.Item(0).Editable = false;
                    ((Grid)grid.Specific).Columns.Item(1).Editable = false;
                    ((Grid)grid.Specific).Columns.Item(2).Editable = false;
                    ((Grid)grid.Specific).Columns.Item(2).Visible = false;
                    ((Grid)grid.Specific).Columns.Item(3).Editable = false;
                    ((Grid)grid.Specific).Columns.Item(4).Editable = false;
                    ((Grid)grid.Specific).Columns.Item(5).Editable = false;
                    ((Grid)grid.Specific).Columns.Item(6).Editable = false;
                }
            }
            catch(Exception ex)
            {
                Log.Error("Exception (OnAfterFormDataLoad) => " + ex.Message);
            }
        }

    }
}
