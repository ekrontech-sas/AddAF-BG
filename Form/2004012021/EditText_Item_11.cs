using B1Framework.B1Frame;
using SAPbouiCOM;

namespace bagant.Form._2004012021
{
    class EditText_Item_11 : B1Item
    {
        public EditText_Item_11()
        {
            FormType = "2004012021";
            ItemUID = "Item_11";
        }

        [B1Listener(BoEventTypes.et_CHOOSE_FROM_LIST, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            if (((IChooseFromListEvent)pVal).SelectedObjects == null) return;

            Form = new B1Forms(pVal.FormUID);
            Form.DataSources.UserDataSources.Item("UD_OPD").Value = ListChoiceListener(pVal, "OpprId")[0].ToString();
        }
    }
}
