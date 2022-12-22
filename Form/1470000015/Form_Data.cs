using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;

namespace bagant.Form._1470000015
{
    class Form_Data : B1Form
    {
        public Form_Data()
        {
            FormType = "1470000015";
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_ADD, false)]
        public virtual void OnAfterFormDataAdd(BusinessObjectInfo pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (pVal.ActionSuccess)
            {
                var docEntry = Form.DataSources.DBDataSources.Item("OACD").GetValue("DocEntry", 0);
                var sapServ = new SAPServices();
                    sapServ.AddMovementAssetData(docEntry, "OACD", "ACD1", "1470000060");
            }
        }

    }
}
