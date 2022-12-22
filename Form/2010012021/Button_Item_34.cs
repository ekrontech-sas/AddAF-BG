using B1Framework.B1Frame;
using SAPbouiCOM;

namespace bagant.Form._2010012021
{
    class Button_Item_34 : B1Item
    {
        public Button_Item_34()
        {
            FormType = "2010012021";
            ItemUID = "Item_34";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnAfterItemPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            Form.Close();
        }
    }
}
