using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;

namespace bagant.Menu
{
    class Menu_1281 : B1XmlFormMenu
    {
        public Menu_1281()
        {
            MenuUID = "1281";
        }

        [B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        public virtual void OnAfterMenuClick(MenuEvent pVal)
        {
            Form = new B1Forms(B1Connections.SboApp.Forms.ActiveForm.UniqueID);
            if (Form.TypeEx == "2002122020") 
                new CommonTaskSapUI().SetSearchMode(Form, true, true);
        }

    }
}
