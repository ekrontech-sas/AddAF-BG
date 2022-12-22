using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System;

namespace bagant.Form._2002122020
{
    class ComboBox_Item_94 : B1Item
    {
        public ComboBox_Item_94()
        {
            FormType = "2002122020";
            ItemUID = "Item_94";
        }

        [B1Listener(BoEventTypes.et_COMBO_SELECT, false)]
        public virtual void OnAfterComboSelect(ItemEvent pVal)
        {
            try
            {
                Form = new B1Forms(pVal.FormUID);
                var comboDistancia = (ComboBox)Form.Items.Item("Item_47").Specific;
                var comboSucursal = (ComboBox)Form.Items.Item("Item_94").Specific;

                if (comboDistancia.Selected != null)
                {
                    var distancia = comboDistancia.Selected == null ? "" : comboDistancia.Selected.Value;
                    var sucursal = comboSucursal.Selected == null ? "" : comboSucursal.Selected.Value;

                    new CommonTaskSapUI().SetFilterItemsTable(Form, sucursal, distancia);
                }
            }
            catch (Exception ex)
            {
                B1Connections.SboApp.SetStatusBarMessage("Excepcion [OnAfterItemPressed] ->> " + ex.Message);
            }
        }
    }
}
