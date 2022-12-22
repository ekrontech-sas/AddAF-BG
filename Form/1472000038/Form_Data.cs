using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;

namespace bagant.Form._1472000038
{
    class Form_Data : B1Form
    {
        public Form_Data()
        {
            FormType = "1472000038";
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_ADD, false)]
        public virtual void OnAfterFormDataAdd(BusinessObjectInfo pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (pVal.ActionSuccess)
            {
                var docEntry = Form.DataSources.DBDataSources.Item("OFAR").GetValue("DocEntry", 0);
                new SAPServices().AddMovementAssetData(docEntry, "OFAR", "FAR1", "9472000038");
            }
        }

    }
}
