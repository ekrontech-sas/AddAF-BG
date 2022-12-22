using B1Framework.B1Frame;
using B1Framework.RecordSet;
using bagant.Services;
using SAPbouiCOM;
using System;

namespace bagant.Form._150
{
    class Button_1 : B1Item
    {
        public Button_1()
        {
            FormType = "150";
            ItemUID = "1";
        }

        [B1Listener(BoEventTypes.et_CLICK, true)]
        public virtual bool OnBeforeClick(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            if (Form.Mode == BoFormMode.fm_ADD_MODE || Form.Mode == BoFormMode.fm_UPDATE_MODE)
            {
                var invtrType = Form.DataSources.DBDataSources.Item("OITM").GetValue("GLMethod", 0).Trim();
                var mngItem = ((ComboBox)Form.Items.Item("162").Specific).Selected;

                var invtrParam = GetParamMethod();

                if (!invtrType.Equals(invtrParam))
                {
                    B1Connections.SboApp.SetStatusBarMessage("El metodo de asignacion de cuenta no es igual al parametrizado para el addon, por favor verificar.");
                    return false;
                }

                if (mngItem != null)
                {
                    if (!mngItem.Value.Equals("2"))
                    {
                        B1Connections.SboApp.SetStatusBarMessage("El metodo de gestion del articulo, no esta manegado por lote. Por favor verificar.");
                        return false;
                    }
                }
                else
                {
                    B1Connections.SboApp.SetStatusBarMessage("El metodo de gestion del articulo, no esta manegado por lote. Por favor verificar.");
                    return false;
                }

                Log.Info("Entro click3 ");
            }

            return true;
        }

        private string GetParamMethod()
        {
            var query = B1Util.GetEmbeddedResource("bagant.Hana.GetGLMethod.sql", GetType().Assembly);
            Log.Info("(GetParamMethod) " + query);
            var resp = Record.Instance.Query(query).Execute().First();

            return resp["Type"];
        }

    }
}
