using B1Framework.B1Frame;
using SAPbouiCOM;

namespace bagant.Form._1473000075
{
    class Form_Load : B1Form
    {
        public Form_Load()
        {
            FormType = "1473000075";
        }

        [B1Listener(BoEventTypes.et_FORM_LOAD, true)]
        public virtual bool OnBeforeLoad(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            CreateStructure(pVal.FormUID);

            return true;
        }

        private void CreateStructure(string formUID)
        {
            var item = Form.Items.Item("234000008");
            var itemsXml = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Form._1473000075.xml.UpdateAsset.xml", GetType().Assembly), formUID, item.Top + 14, item.Left) ;
            B1Connections.SboApp.LoadBatchActions(itemsXml);

            var grid = (Grid)Form.Items.Item("Item_4").Specific;
                grid.Columns.Item(0).TitleObject.Caption = "";
        }
    }
}
