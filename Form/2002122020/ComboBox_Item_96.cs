using B1Framework.B1Frame;
using SAPbouiCOM;
using System;

namespace bagant.Form._2002122020
{
    class ComboBox_Item_96 : B1Item
    {
        public ComboBox_Item_96()
        {
            FormType = "2002122020";
            ItemUID = "Item_96";
        }

        [B1Listener(BoEventTypes.et_COMBO_SELECT, false)]
        public virtual void OnAfterItemPressed(ItemEvent pVal)
        {
            try
            {
                Form = new B1Forms(pVal.FormUID);
                var combo = (ButtonCombo)Form.Items.Item("Item_96").Specific;
                var opt = combo.Selected.Value;

                if ( opt.Equals("23") )
                {
                    Form.Items.Item("Item_97").Click();
                    B1Connections.SboApp.SendKeys("{TAB}");
                }
            }
            catch (Exception ex)
            {
                B1Connections.SboApp.SetStatusBarMessage("Excepcion [OnAfterItemPressed] ->> " + ex.Message);
            }
        }

    }
}
