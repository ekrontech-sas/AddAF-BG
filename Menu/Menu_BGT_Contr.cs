using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System;

namespace bagant.Menu
{
    class Menu_BGT_Contr : B1XmlFormMenu
    {
        public Menu_BGT_Contr()
        {
            MenuUID = "BGT_Contr";
        }

        [B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        public virtual void OnBeforeMenuClick(MenuEvent pVal)
        {
            try
            {
                var uid = Guid.NewGuid().ToString().Substring(0, 6);
                var xml = B1Util.Formato(GetEmbeddedResource("bagant.Form._2002122020.srf.form_contrato.srf"), uid);

                _init(xml, uid);
            }
            catch(Exception ex)
            {
                Log.Error("Exception (OnBeforeMenuClick) => "+ex.Message);
            }
        }

        private void _init(string xml, string uid)
        {
            B1Connections.SboApp.LoadBatchActions(ref xml);

            Form = new B1Forms(uid);

            new CommonTaskSapUI().SetContractInitialValues(Form);

            Form.Items.Item("dummy").Click();
            Form.Visible = true;
        }

    }
}
