using B1Framework.B1Frame;
using SAPbouiCOM;

namespace bagant.Form._1473000075
{
    class TAB_Item_0 : B1Item
    {
        public TAB_Item_0()
        {
            FormType = "1473000075";
            ItemUID = "Item_0";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnAfterItemPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            Form.PaneLevel = 50;
        }
    }
}
