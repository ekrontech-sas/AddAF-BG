using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;

namespace bagant.Menu
{
    class Menu_1282 : B1XmlFormMenu
    {
        public Menu_1282()
        {
            MenuUID = "1282";
        }

        [B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        public virtual void OnAfterMenuClick(MenuEvent pVal)
        {
            Form = new B1Forms(B1Connections.SboApp.Forms.ActiveForm.UniqueID);
            if (Form.TypeEx == "150")
            {
                ((CheckBox)Form.Items.Item("Item_1").Specific).Checked = true;
            }
            else if (Form.TypeEx == "1473000075")
            {
                ((CheckBox)Form.Items.Item("Item_2").Specific).Checked = true;
            }
            else if (Form.TypeEx == "2002122020")
            {
                new CommonTaskSapUI().SetSearchMode(Form, false, false);
                new CommonTaskSapUI().SetContractInitialValues(new B1Forms(B1Connections.SboApp.Forms.ActiveForm.UniqueID));
                new CommonTaskSapUI().EnableFields(Form);
            }
        }
    }
}
