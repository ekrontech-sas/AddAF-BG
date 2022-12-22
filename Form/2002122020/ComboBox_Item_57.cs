using B1Framework.B1Frame;
using B1Framework.RecordSet;
using SAPbouiCOM;
using System;

namespace bagant.Form._2002122020
{
    class ComboBox_Item_57 : B1Item
    {
        public ComboBox_Item_57()
        {
            FormType = "2002122020";
            ItemUID = "Item_57";
        }

        [B1Listener(BoEventTypes.et_COMBO_SELECT, false)]
        public virtual void OnAfterItemPressed(ItemEvent pVal)
        {
            try
            {
                Form = new B1Forms(pVal.FormUID);
                var serie = (SAPbouiCOM.ComboBox) Form.Items.Item(pVal.ItemUID).Specific;

                if( serie.Selected != null )
                {
                    var seriesSelected = serie.Selected.Value;
                    var queryNext = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetNextDocumentNumber.sql"), seriesSelected);
                    var resp = Record.Instance.Query(queryNext).Execute().First();

                    Form.DataSources.DBDataSources.Item("@BGT_OCTR").SetValue("DocNum", 0, resp["NextNumber"]);
                }
            }
            catch (Exception ex)
            {
                B1Connections.SboApp.SetStatusBarMessage("Excepcion [OnAfterItemPressed] ->> " + ex.Message);
            }
        }

    }
}
