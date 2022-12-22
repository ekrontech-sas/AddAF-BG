using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;

namespace bagant.Menu
{
    class Menu_CancelDoc : B1XmlFormMenu
    {
        public Menu_CancelDoc()
        {
            MenuUID = "CancelDoc";
        }

        [B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        public virtual void OnBeforeMenuClick(MenuEvent pVal)
        {
            Form = new B1Forms(B1Connections.SboApp.Forms.ActiveForm.UniqueID);

            var docEntry = Form.DataSources.DBDataSources.Item("@BGT_OCTR").GetValue("DocEntry", 0);
            var udo = new UDOServices("AGT_CTR");
                udo.LoadDocumentHeader("DocEntry", docEntry);
                udo.UpdateHeaderDocument("C", "U_BGT_Status");
                udo.Update();

            B1Connections.SboApp.SetStatusBarMessage("Documento cancelado", BoMessageTime.bmt_Medium, false);
        }

    }
}
