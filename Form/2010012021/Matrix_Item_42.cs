using B1Framework.B1Frame;
using SAPbouiCOM;

namespace bagant.Form._2010012021
{
    class Matrix_Item_42 : B1Item
    {
        public Matrix_Item_42()
        {
            FormType = "2010012021";
            ItemUID = "Item_42";
        }

        [B1Listener(BoEventTypes.et_CHOOSE_FROM_LIST, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (((SAPbouiCOM.IChooseFromListEvent)pVal).SelectedObjects == null) return;

            var matrix = (SAPbouiCOM.Matrix)Form.Items.Item(ItemUID).Specific;
            if (pVal.ColUID == "Col_1")
            {
                matrix.GetLineData(pVal.Row);
                    Form.DataSources.UserDataSources.Item("UD_Art").Value = ListChoiceListener(pVal, "ItemCode")[0].ToString();
                    Form.DataSources.UserDataSources.Item("UD_ArtN").Value = ListChoiceListener(pVal, "ItemName")[0].ToString();
                matrix.SetLineData(pVal.Row);
            }
            else if (pVal.ColUID == "Col_0")
            {
                matrix.GetLineData(pVal.Row);
                    Form.DataSources.UserDataSources.Item("UD_Cont").Value = ListChoiceListener(pVal, "DocNum")[0].ToString();
                matrix.SetLineData(pVal.Row);
            }
        }
    }
}
