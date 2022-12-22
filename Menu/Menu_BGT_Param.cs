using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System;

namespace bagant.Menu
{
    class Menu_BGT_Param : B1XmlFormMenu
    {
        public Menu_BGT_Param()
        {
            MenuUID = "BGT_Param";
        }

        [B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        public virtual void OnBeforeMenuClick(MenuEvent pVal)
        {
            Log.Info("(OnBeforeMenuClick) 17");

            var uid = Guid.NewGuid().ToString().Substring(0, 6);
            var xml = B1Util.Formato(GetEmbeddedResource("bagant.Form._2003112020.srf.FRM_PARAMS.srf"), uid);

            _init(xml, uid);
        }

        private void _init(string xml, string uid)
        {
            try
            {
                B1Connections.SboApp.LoadBatchActions(ref xml);
                Log.Info("Error Load  " + B1Connections.SboApp.GetLastBatchResults());
                Form = new B1Forms(uid);

                var querySeries = GetEmbeddedResource("bagant.Hana.GetSeriesItems.sql");
                var queryClassAf = GetEmbeddedResource("bagant.Hana.GetClassAF.sql");
                var queryWarehouse = GetEmbeddedResource("bagant.Hana.GetWareHouse.sql");
                var querySucursal = GetEmbeddedResource("bagant.Hana.GetBranches.sql");
                var querySerie = GetEmbeddedResource("bagant.Hana.GetSeries.sql");

                FillColumnComboBoxWithQuery(querySeries, "Item_7", "Col_0");
                FillColumnComboBoxWithQuery(querySeries, "Item_7", "Col_1");
                FillColumnComboBoxWithQuery(queryClassAf, "Item_7", "Col_2");
                FillColumnComboBoxWithQuery(queryWarehouse, "Item_2", "Col_0");
                FillColumnComboBoxWithQuery(querySucursal, "Item_50", "Col_2");
                FillColumnComboBoxWithQuery(querySerie, "Item_50", "Col_1");

                var matrix = (SAPbouiCOM.Matrix)Form.Items.Item("Item_55").Specific;
                    matrix.Columns.Item("Col_2").Visible = false;
            }
            catch(Exception ex)
            {
                Log.Info("Exception (_init)=>" + ex.Message);
            }

            Form.Visible = true;
        }
    }
}
