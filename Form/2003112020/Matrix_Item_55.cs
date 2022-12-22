using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System;

namespace bagant.Form._2003112020
{
    class Matrix_Item_55 : B1Item
    {
        public Matrix_Item_55()
        {
            FormType = "2003112020";
            ItemUID = "Item_55";
        }

        [B1Listener(BoEventTypes.et_CHOOSE_FROM_LIST, false)]
        public virtual void OnBeforeChooseFromList(ItemEvent pVal)
        {
            if (((IChooseFromListEvent)pVal).SelectedObjects == null) return;

            Form = new B1Forms(pVal.FormUID);
            if (pVal.ColUID == "Col_0")
            {
                var signature = ListChoiceListener(pVal, "USERID")[0].ToString();
                var codigo = ListChoiceListener(pVal, "USER_CODE")[0].ToString();
                var nombre = ListChoiceListener(pVal, "U_NAME")[0].ToString();
                var matrix = (SAPbouiCOM.Matrix)Form.Items.Item(ItemUID).Specific;

                try
                {
                    matrix.SetCellWithoutValidation(pVal.Row, "Col_0", codigo);
                    matrix.SetCellWithoutValidation(pVal.Row, "Col_2", signature);
                    matrix.SetCellWithoutValidation(pVal.Row, "Col_1", nombre);
                    matrix.AutoResizeColumns();

                    Form.Mode = BoFormMode.fm_UPDATE_MODE;
                }
                catch (Exception ex)
                {
                    Log.Info("Exception => " + ex.Message);
                }
            }
        }

    }
}
