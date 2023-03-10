using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;

namespace bagant.Form._1470000012
{
    class Form_Data : B1Form
    {
        public Form_Data()
        {
            FormType = "1470000012";
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_ADD, false)]
        public virtual void OnAfterFormDataAdd(BusinessObjectInfo pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (pVal.ActionSuccess)
            {
                var docEntry = Form.DataSources.DBDataSources.Item("OMDP").GetValue("DocEntry", 0);
                new SAPServices().AddMovementAssetData(docEntry, "OMDP", "MDP1", "");
            }
        }

    }
}
