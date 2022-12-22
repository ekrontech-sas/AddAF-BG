using B1Framework.B1Frame;
using SAPbouiCOM;

namespace bagant.Form._2002122020
{
    class EditText_Item_95 : B1Item
    {
        public EditText_Item_95()
        {
            FormType = "2002122020";
            ItemUID = "Item_95";
        }

        [B1Listener(BoEventTypes.et_CHOOSE_FROM_LIST, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            if (((IChooseFromListEvent)pVal).SelectedObjects == null) return;

            Form = new B1Forms(pVal.FormUID);

            SetFieldDb(ListChoiceListener(pVal, "Code")[0].ToString(), "U_BGT_CodComis", "@BGT_OCTR");
            SetFieldDb(ListChoiceListener(pVal, "U_CMS_COD")[0].ToString(), "U_BGT_NomComis", "@BGT_OCTR");
        }
    }
}
