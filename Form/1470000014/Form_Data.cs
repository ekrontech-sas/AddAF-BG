using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;

namespace bagant.Form._1470000014
{
    class Form_Data : B1Form
    {
        public Form_Data()
        {
            FormType = "1470000014";
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_ADD, false)]
        public virtual void OnAfterFormDataAdd(BusinessObjectInfo pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (pVal.ActionSuccess)
            {
                var docEntry = Form.DataSources.DBDataSources.Item("ORTI").GetValue("DocEntry", 0);
                new SAPServices().AddMovementAssetData(docEntry, "ORTI", "RTI1", "1470000094");
            }
        }
    }
}
