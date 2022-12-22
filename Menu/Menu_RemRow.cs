using B1Framework.B1Frame;
using SAPbouiCOM;
using System.Linq;

namespace bagant.Menu
{
    class Menu_RemRow : B1XmlFormMenu
    {
        public Menu_RemRow()
        {
            MenuUID = "RemRow";
        }

        [B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        public virtual void OnBeforeMenuClick(MenuEvent pVal)
        {
            if (!(new[] { "2003112020","2002122020" }).Contains(B1Connections.SboApp.Forms.ActiveForm.TypeEx)) return;

            Form = new B1Forms(B1Connections.SboApp.Forms.ActiveForm.UniqueID);
            var itemUID = GetItemUID(Form.PaneLevel);
            var table = (SAPbouiCOM.Matrix)Form.Items.Item(itemUID).Specific;

            new B1Matrix(table).DeleteRowSelected();
        }

        private string GetItemUID(int paneLevel)
        {
            switch (paneLevel)
            {
                case 1:
                    return "Item_62";
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

    }
}
