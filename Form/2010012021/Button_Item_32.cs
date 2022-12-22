using B1Framework.B1Frame;
using SAPbouiCOM;
using System;

namespace bagant.Form._2010012021
{
    class Button_Item_32 : B1Item
    {
        public Button_Item_32()
        {
            FormType = "2010012021";
            ItemUID = "Item_32";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnAfterItemPressed(ItemEvent pVal)
        {
            try
            {
                Form = new B1Forms(pVal.FormUID);
                ((Grid)Form.Items.Item("Item_30").Specific).Rows.CollapseAll(); ;
            }
            catch (Exception ex)
            {
                B1Connections.SboApp.SetStatusBarMessage("Excepcion [OnAfterItemPressed] ->> " + ex.Message);
            }
        }
    }
}
