using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System;

namespace bagant.Menu
{
    class Menu_ExpandRow : B1XmlFormMenu
    {
        public Menu_ExpandRow()
        {
            MenuUID = "ExpandRow";
        }

        [B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        public virtual void OnBeforeMenuClick(MenuEvent pVal)
        {
            try
            {
                Form = new B1Forms(B1Connections.SboApp.Forms.ActiveForm.UniqueID);
                var grid = (Grid)Form.Items.Item("Item_5").Specific;
                    grid.Rows.ExpandAll();
            }
            catch (Exception ex)
            {
                Log.Error("Exception (OnBeforeMenuClick) => " + ex.Message);
            }
        }

    }
}
