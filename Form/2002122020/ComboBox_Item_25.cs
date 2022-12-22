using B1Framework.B1Frame;
using B1Framework.RecordSet;
using SAPbouiCOM;
using System;

namespace bagant.Form._2002122020
{
    class ComboBox_Item_25 : B1Item
    {
        public ComboBox_Item_25()
        {
            FormType = "2002122020";
            ItemUID = "Item_25";
        }

        [B1Listener(BoEventTypes.et_COMBO_SELECT, false)]
        public virtual void OnAfterComboSelect(ItemEvent pVal)
        {
            try
            {
                Form = new B1Forms(pVal.FormUID);
                var comboObra = (ComboBox)Form.Items.Item(pVal.ItemUID).Specific;

                if (comboObra.Selected != null)
                {
                    var cardCode = Form.DataSources.DBDataSources.Item("@BGT_OCTR").GetValue("U_BGT_CardCode", 0).Trim();
                    var selObra = comboObra.Selected.Value;
                    var queryObra = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetObraSelect.sql"), cardCode, selObra);
                    var respObra = Record.Instance.Query(queryObra).Execute().First();

                    Form.DataSources.DBDataSources.Item("@BGT_OCTR").SetValue("U_BGT_Project", 0, respObra["Address2"]);
                    Form.DataSources.DBDataSources.Item("@BGT_OCTR").SetValue("U_BGT_ProjectName", 0, respObra["PrjName"]);
                }                
            }
            catch (Exception ex)
            {
                B1Connections.SboApp.SetStatusBarMessage("Excepcion [OnAfterItemPressed] ->> " + ex.Message);
            }
        }

    }
}
