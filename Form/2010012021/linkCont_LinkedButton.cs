using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;

namespace bagant.Form._2010012021
{
    class linkCont_LinkedButton : B1Item
    {
        public linkCont_LinkedButton()
        {
            FormType = "2010012021";
            ItemUID = "linkCont";
        }

        [B1Listener(BoEventTypes.et_CLICK, true)]
        public virtual bool OnBeforeItemPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            var contrato = ((EditText)Form.Items.Item("U_BGT_ContAsoc").Specific).Value;
            Log.Info("contrato => " + contrato);
            B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_UserDefinedObject, "", contrato);

            return false;
        }
    }
}
