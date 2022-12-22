using B1Framework.B1Frame;
using SAPbouiCOM;

namespace bagant.Form._2002122020
{
    class EditText_Item_53 : B1Item
    {
        public EditText_Item_53()
        {
            FormType = "2002122020";
            ItemUID = "Item_53";
        }

        [B1Listener(BoEventTypes.et_CHOOSE_FROM_LIST, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            if (((IChooseFromListEvent)pVal).SelectedObjects == null) return;

            Form = new B1Forms(pVal.FormUID);

            SetFieldDb(ListChoiceListener(pVal, "PrjCode")[0].ToString(), "U_BGT_Project", "@BGT_OCTR");
            SetFieldDb(ListChoiceListener(pVal, "PrjName")[0].ToString(), "U_BGT_ProjectName", "@BGT_OCTR");
        }

    }
}
