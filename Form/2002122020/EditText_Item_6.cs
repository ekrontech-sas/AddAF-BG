using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;

namespace bagant.Form._2002122020
{
    class EditText_Item_6 : B1Item
    {
        public EditText_Item_6()
        {
            FormType = "2002122020";
            ItemUID = "Item_6";
        }

        [B1Listener(BoEventTypes.et_CHOOSE_FROM_LIST, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            if (((IChooseFromListEvent)pVal).SelectedObjects == null) return;

            Form = new B1Forms(pVal.FormUID);

            var cardCode = ListChoiceListener(pVal, "CardCode")[0].ToString();
            var cardName = ListChoiceListener(pVal, "CardName")[0].ToString();
            var docEntry = ListChoiceListener(pVal, "DocEntry")[0].ToString();

            SetFieldDb(cardCode, "U_BGT_CardCode", "@BGT_OCTR");
            SetFieldDb(cardName, "U_BGT_CardName", "@BGT_OCTR");

            new CommonTaskSapUI().SetValuesFromSNChoosed(Form, cardCode);
            new CommonTaskSapUI().SetFilterProject(Form, cardCode);
        }

    }
}
