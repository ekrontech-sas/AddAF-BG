using B1Framework.B1Frame;
using SAPbouiCOM;

namespace bagant.Form._2002122020
{
    class Grid_Item_5 : B1Item
    {
        public Grid_Item_5()
        {
            FormType = "2002122020";
            ItemUID = "Item_5";
        }

        [B1Listener(BoEventTypes.et_MATRIX_LINK_PRESSED, true)]
        public virtual bool OnBeforeChooseFromList(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            
            return true;
        }
    }
}
