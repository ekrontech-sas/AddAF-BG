using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System.Linq;

namespace bagant.Menu
{
    class Menu_AddRowDa : B1XmlFormMenu
    {
        public Menu_AddRowDa()
        {
            MenuUID = "AddRowDa";
        }

        [B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        public virtual void OnBeforeMenuClick(MenuEvent pVal)
        {
            if (!(new[] { "2010012021" }).Contains(B1Connections.SboApp.Forms.ActiveForm.TypeEx)) return;

            Form = new B1Forms(B1Connections.SboApp.Forms.ActiveForm.UniqueID);

            try
            {
                Form.Freeze(true);
                var matrix = (SAPbouiCOM.Matrix)Form.Items.Item("Item_42").Specific;
                    matrix.AddRow(1);

                    matrix.GetLineData(matrix.RowCount);
                        Form.DataSources.UserDataSources.Item("UD_Line").Value = matrix.RowCount.ToString();
                        Form.DataSources.UserDataSources.Item("UD_Cont").Value = "";
                        Form.DataSources.UserDataSources.Item("UD_Art").Value = "";
                        Form.DataSources.UserDataSources.Item("UD_ArtN").Value = "";
                        Form.DataSources.UserDataSources.Item("UD_Qty").Value = "0";
                matrix.SetLineData(matrix.RowCount);
            }
            finally
            {
                Form.Freeze(false);
            }
        }
    }
}
