using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System;

namespace bagant.Form._2003112020
{
    class Button_Item_10 : B1Item
    {
        public Button_Item_10()
        {
            FormType = "2003112020";
            ItemUID = "Item_10";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnAfterItemPressed(ItemEvent pVal)
        {
            try
            {
                var path = FileDialog.OpenFolderDialog();
                Form.DataSources.DBDataSources.Item(0).SetValue("U_BGT_PATHLOG", 0, path);
            }
            catch (Exception ex)
            {
                B1Connections.SboApp.SetStatusBarMessage("Excepcion [OnAfterItemPressed] ->> " + ex.Message);
            }
        }

    }
}
