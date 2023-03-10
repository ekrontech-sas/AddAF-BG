using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System;

namespace bagant.Menu
{
    class Menu_BGT_Devol : B1XmlFormMenu
    {
        public Menu_BGT_Devol()
        {
            MenuUID = "BGT_Devol";
        }

        [B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        public virtual void OnBeforeMenuClick(MenuEvent pVal)
        {
            try
            {
                var uid = Guid.NewGuid().ToString().Substring(0, 6);
                var xml = B1Util.Formato(GetEmbeddedResource("bagant.Form._2010012021.srf.FRM_DEVOL.srf"), uid);

                _init(xml, uid);
            }
            catch (Exception ex)
            {
                Log.Error("Exception (OnBeforeMenuClick) => " + ex.Message);
            }
        }

        private void _init(string xml, string uid)
        {
            B1Connections.SboApp.LoadBatchActions(ref xml);
            Form = new B1Forms(uid);

            var gridWhs = (Grid)Form.Items.Item("Item_30").Specific;
                gridWhs.Columns.Item("Sel").Type = BoGridColumnType.gct_CheckBox;
                gridWhs.Columns.Item("Sel").TitleObject.Caption = "";

            var gridColWH = (EditTextColumn)gridWhs.Columns.Item("Codigo Almacen");
                gridColWH.LinkedObjectType = "64";

            Form.SetCflConditions("CFL_OED", new string[] { "DocStatus", "CANCELED" }, new BoConditionOperation[] { BoConditionOperation.co_NOT_EQUAL, BoConditionOperation.co_NOT_EQUAL }, new string[] { "C", "Y" });
            Form.SetCflConditions("CFL_OEH", new string[] { "DocStatus", "CANCELED" }, new BoConditionOperation[] { BoConditionOperation.co_NOT_EQUAL, BoConditionOperation.co_NOT_EQUAL }, new string[] { "C", "Y" });

            for (int i = 0; i < gridWhs.Columns.Count; i++)
            {
                if (i == 1)
                    continue;

                gridWhs.Columns.Item(i).Editable = false;
            }

            Form.Visible = true;
        }

    }
}
