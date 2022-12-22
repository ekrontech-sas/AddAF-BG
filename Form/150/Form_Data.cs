using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System.Xml;

namespace bagant.Form._150
{
    class Form_Data : B1Form
    {
        public Form_Data()
        {
            FormType = "150";
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_ADD, false)]
        public virtual void OnAfterFormDataAdd(BusinessObjectInfo pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var asset = ((CheckBox)Form.Items.Item("Item_1").Specific).Checked;

            if  ( pVal.ActionSuccess && asset )
            {
                var xml = new XmlDocument();
                    xml.LoadXml(pVal.ObjectKey);

                new SAPServices().CreateFixedAsset(xml.InnerText);
            }
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_LOAD, false)]
        public virtual void OnAfterFormDataLoad(BusinessObjectInfo pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            
            var xml = new XmlDocument();
                xml.LoadXml(pVal.ObjectKey);

            var queryMovement = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetMovementItem.sql", GetType().Assembly), xml.InnerText);
            Log.Info("queryMovement "+ queryMovement);

            var grid = Form.Items.Item("Item_4");
            Form.DataSources.DataTables.Item("DT_Mov").ExecuteQuery(queryMovement);

            if( string.IsNullOrEmpty(Form.DataSources.DataTables.Item("DT_Mov").GetValue(0, 0)))
            {
                Form.DataSources.DataTables.Item("DT_Mov").ExecuteQuery("SELECT 'No existen registros coincidentes' AS \"Mensaje\" FROM DUMMY");
                grid.Enabled = false;
            }
            else
            {
                grid.Enabled = true;
                ((Grid)grid.Specific).Columns.Item(0).Editable = false;
                ((Grid)grid.Specific).Columns.Item(1).Editable = false;
                ((Grid)grid.Specific).Columns.Item(2).Editable = false;
                ((Grid)grid.Specific).Columns.Item(3).Editable = false;
                ((Grid)grid.Specific).Columns.Item(4).Editable = false;
                ((Grid)grid.Specific).Columns.Item(5).Editable = false;
                ((Grid)grid.Specific).Columns.Item(8).Editable = false;
                ((Grid)grid.Specific).Columns.Item(9).Editable = false;
            }

            return;

        }


    }
}
