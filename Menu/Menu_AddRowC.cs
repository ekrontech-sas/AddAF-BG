using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System.Linq;

namespace bagant.Menu
{
    class Menu_AddRowC : B1XmlFormMenu
    {
        public Menu_AddRowC()
        {
            MenuUID = "AddRowC";
        }

        [B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        public virtual void OnBeforeMenuClick(MenuEvent pVal)
        {
            if (!(new[] { "2002122020" }).Contains(B1Connections.SboApp.Forms.ActiveForm.TypeEx)) return;

            Form = new B1Forms(B1Connections.SboApp.Forms.ActiveForm.UniqueID);

            var itemUID = "Item_62";
            var bdlevel = 1;
            var table = (SAPbouiCOM.Matrix)Form.Items.Item(itemUID).Specific;

            new CommonTaskSapUI().AddRowToMatrix(Form, table, bdlevel);
        }
    }
}
