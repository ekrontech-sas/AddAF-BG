using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System;

namespace bagant.Form._2004012021
{
    class Grid_Item_30 : B1Item
    {
        public Grid_Item_30()
        {
            FormType = "2004012021";
            ItemUID = "Item_30";
        }

        [B1Listener(BoEventTypes.et_DOUBLE_CLICK, false)]
        public virtual void OnAfterDoubleClick(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var grid = (Grid)Form.Items.Item(pVal.ItemUID).Specific;
            var opt =(string) grid.DataTable.GetValue("Sel", 1);

            if( pVal.ColUID == "Sel" )
            {
                if (pVal.Row == -1)
                {
                    try
                    {
                        Form.Freeze(true);
                        for (int i = 0; i < grid.DataTable.Rows.Count; i++)
                            grid.DataTable.SetValue("Sel", i, opt.Equals("Y") ? "N" : "Y");
                    }
                    catch (Exception ex)
                    {
                        Log.Info("Exception ex => " + ex.Message);
                    }
                    finally
                    {
                        Form.Freeze(false);
                    }
                }
            }
        }
    }
}
