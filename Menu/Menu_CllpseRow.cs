using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System;

namespace bagant.Menu
{
    class Menu_CllpseRow : B1XmlFormMenu
    {
        public Menu_CllpseRow()
        {
            MenuUID = "CllpseRow";
        }

        [B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        public virtual void OnBeforeMenuClick(MenuEvent pVal)
        {
            try
            {
                Form = new B1Forms(B1Connections.SboApp.Forms.ActiveForm.UniqueID);
                var grid = (Grid)Form.Items.Item("Item_5").Specific;
                    grid.Rows.CollapseAll();
            }
            catch (Exception ex)
            {
                Log.Error("Exception (OnBeforeMenuClick) => " + ex.Message);
            }
        }
    }
}
