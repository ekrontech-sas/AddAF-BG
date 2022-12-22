using B1Framework.B1Frame;
using SAPbouiCOM;

namespace bagant.Form._2010012021
{
    class EditText_Item_26 : B1Item
    {
        public EditText_Item_26()
        {
            FormType = "2010012021";
            ItemUID = "Item_26";
        }

        [B1Listener(BoEventTypes.et_CHOOSE_FROM_LIST, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            if (((IChooseFromListEvent)pVal).SelectedObjects == null) return;

            Form = new B1Forms(pVal.FormUID);
            Form.DataSources.UserDataSources.Item("UD_ITH").Value = ListChoiceListener(pVal, "ItemCode")[0].ToString();
        }
    }
}
