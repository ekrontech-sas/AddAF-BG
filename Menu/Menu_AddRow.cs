using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System.Linq;

namespace bagant.Menu
{
    class Menu_AddRow : B1XmlFormMenu
    {
        public Menu_AddRow()
        {
            MenuUID = "AddRow";
        }

        [B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        public virtual void OnBeforeMenuClick(MenuEvent pVal)
        {
            if (!(new[] { "2003112020" }).Contains(B1Connections.SboApp.Forms.ActiveForm.TypeEx)) return;

            Form = new B1Forms(B1Connections.SboApp.Forms.ActiveForm.UniqueID);

            var itemUID = GetItemUID(Form.PaneLevel);
            var bdlevel = GetBdLevel(Form.PaneLevel);
            var table = (SAPbouiCOM.Matrix)Form.Items.Item(itemUID).Specific;

            new CommonTaskSapUI().AddRowToMatrix(Form, table, bdlevel);
        }

        private string GetItemUID(int paneLevel)
        {
            switch(paneLevel)
            {
                case 2:
                    return "Item_7";
                case 3:
                    return "Item_2";
                case 4:
                    return "Item_17";
                case 6:
                    return "Item_49";
                case 7:
                    return "Item_50";
                case 8:
                    return "Item_55";
                default:
                    return "";
            }
        }

        private int GetBdLevel(int paneLevel)
        {
            switch (paneLevel)
            {
                case 2:
                    return 1;
                case 3:
                    return 2;
                case 4:
                    return 3;
                case 6:
                    return 4;
                case 7:
                    return 5;
                case 8:
                    return 6;
                default:
                    return -1;
            }
        }


    }
}
