using B1Framework.B1Frame;
using SAPbouiCOM;

namespace bagant.Form._2010012021
{
    class Matrix_Item_6 : B1Item
    {
        public Matrix_Item_6()
        {
            FormType = "2010012021";
            ItemUID = "Item_6";
        }

        [B1Listener(BoEventTypes.et_CHOOSE_FROM_LIST, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (((IChooseFromListEvent)pVal).SelectedObjects == null) return;

            var matrix = (SAPbouiCOM.Matrix)Form.Items.Item(ItemUID).Specific;
            if (pVal.ColUID == "Col_5")
            {
                try
                {
                    var empId = ListChoiceListener(pVal, "empID")[0].ToString();
                    var lastName = ListChoiceListener(pVal, "lastName")[0].ToString();
                    var firstName = ListChoiceListener(pVal, "firstName")[0].ToString();

                    ((EditText)matrix.GetCellSpecific("Col_9", pVal.Row)).Value = empId;
                    ((EditText)matrix.GetCellSpecific(pVal.ColUID, pVal.Row)).Value = lastName;
                }
                catch
                {

                }
            }
            else if (pVal.ColUID == "Col_7")
            {
                var codeTransportista = ListChoiceListener(pVal, "Code")[0].ToString();
                ((EditText)matrix.GetCellSpecific(pVal.ColUID, pVal.Row)).Value = codeTransportista;
            }
            else if (pVal.ColUID == "Col_8")
            {
                var codeTransporte = ListChoiceListener(pVal, "Code")[0].ToString();
                ((EditText)matrix.GetCellSpecific(pVal.ColUID, pVal.Row)).Value = codeTransporte;
            }

            matrix.FlushToDataSource();
        }

        [B1Listener(BoEventTypes.et_VALIDATE, false)]
        public virtual void OnAfterValidate(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var matrix = (SAPbouiCOM.Matrix)Form.Items.Item(pVal.ItemUID).Specific;

            if (pVal.ColUID == "Col_6")
            {
                EditText iNumeroGuia = null;
                iNumeroGuia = ((EditText)matrix.GetCellSpecific("Col_6", pVal.Row));
                
                if (iNumeroGuia.Value.Length > 9)
                {
                    B1Connections.SboApp.SetStatusBarMessage("Numero de caracteres permitidos 9", BoMessageTime.bmt_Medium, true);
                    iNumeroGuia.Value = "0";
                }
                else if (iNumeroGuia.Value.Length > 0 && iNumeroGuia.Value.Length < 9)
                {
                    iNumeroGuia.Value = iNumeroGuia.Value.ToString().PadLeft(9, '0');
                }
            }
        }
    }
}
