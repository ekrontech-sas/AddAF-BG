using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;

namespace bagant.Form._2002122020
{
    class EditText_Item_39 : B1Item
    {
        public EditText_Item_39()
        {
            FormType = "2002122020";
            ItemUID = "Item_39";
        }

        [B1Listener(BoEventTypes.et_CHOOSE_FROM_LIST, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            if (((IChooseFromListEvent)pVal).SelectedObjects == null) return;

            Form = new B1Forms(pVal.FormUID);
            Form.Freeze(true);

            try
            {
                var ofDocEntry = ListChoiceListener(pVal, "DocEntry")[0].ToString();
                var ofDocNum = ListChoiceListener(pVal, "DocEntry")[0].ToString();
                var category = ListChoiceListener(pVal, "U_BG_CATEGORIA")[0].ToString();

                SetFieldDb(ofDocNum, "U_BGT_DocNumOF", "@BGT_OCTR");
                SetFieldDb(ofDocEntry, "U_BGT_DocEntryOF", "@BGT_OCTR");

                new CommonTaskSapUI().SetTableData(Form, ofDocEntry);
                new CommonTaskSapUI().SetOpportunities(Form, ofDocEntry);
                new CommonTaskSapUI().SetCategory(Form, category);
            } 
            finally
            {
                Form.Freeze(false);
            }
        }

    }
}
