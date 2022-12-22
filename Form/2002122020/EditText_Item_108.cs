using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;

namespace bagant.Form._2002122020
{
    class EditText_Item_108 : B1Item
    {
        public EditText_Item_108()
        {
            FormType = "2002122020";
            ItemUID = "Item_108";
        }

        [B1Listener(BoEventTypes.et_CHOOSE_FROM_LIST, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            if (((IChooseFromListEvent)pVal).SelectedObjects == null) return;

            Form = new B1Forms(pVal.FormUID);
            Form.Freeze(true);

            try
            {
                SetFieldDb(ListChoiceListener(pVal, "Code")[0].ToString(), "U_BGT_CicloFact", "@BGT_OCTR");
            }
            finally
            {
                Form.Freeze(false);
            }
        }

    }
}