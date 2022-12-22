using B1Framework.B1Frame;
using SAPbouiCOM;

namespace bagant.Form._2004012021
{
    class Button_Item_2 : B1Item
    {
        public Button_Item_2()
        {
            FormType = "2004012021";
            ItemUID = "Item_2";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnAfterItemPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (Form.PaneLevel-1 == 1) 
                Form.Items.Item(ItemUID).Enabled = false;

            Form.Items.Item("Item_33").Enabled = true;
            Form.PaneLevel -= 1;
        }
    }
}
