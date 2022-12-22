using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System;

namespace bagant.Form._2002122020
{
    class ComboBox_Item_47 : B1Item
    {
        public ComboBox_Item_47()
        {
            FormType = "2002122020";
            ItemUID = "Item_47";
        }

        [B1Listener(BoEventTypes.et_COMBO_SELECT, false)]
        public virtual void OnAfterComboSelect(ItemEvent pVal)
        {
            try
            {
                Form = new B1Forms(pVal.FormUID);
                var distancia = (ComboBox)Form.Items.Item(pVal.ItemUID).Specific;

                if (distancia.Selected != null)
                {
                    var comboSucursal = (ComboBox)Form.Items.Item("Item_94").Specific;
                    var sucursal = comboSucursal.Selected == null ? "" : comboSucursal.Selected.Value;

                    new CommonTaskSapUI().SetFilterItemsTable(Form, sucursal, distancia.Value);
                }
            }
            catch (Exception ex)
            {
                B1Connections.SboApp.SetStatusBarMessage("Excepcion [OnAfterItemPressed] ->> " + ex.Message);
            }
        }

    }
}
