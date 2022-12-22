using B1Framework.B1Frame;
using SAPbouiCOM;

namespace bagant.Form._150
{
    class TAB_Item_3 : B1Item
    {
        public TAB_Item_3()
        {
            FormType = "150";
            ItemUID = "Item_3";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnAfterItemPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            Form.PaneLevel = 51;
        }
    }
}
