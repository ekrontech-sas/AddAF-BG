using B1Framework.B1Frame;
using SAPbouiCOM;

namespace bagant.Form._2002122020
{
    class Tab_Item_100 : B1Item
    {
        public Tab_Item_100()
        {
            FormType = "2002122020";
            ItemUID = "Item_100";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            var form = B1Connections.SboApp.Forms.Item(pVal.FormUID);
            form.Settings.MatrixUID = "Item_102";
        }
    }
}
