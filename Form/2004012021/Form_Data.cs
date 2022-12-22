using B1Framework.B1Frame;
using B1Framework.RecordSet;
//using bagant.Services;
using SAPbouiCOM;
using System;
using static bagant.Program;

namespace bagant.Form._2004012021
{
    class Form_Data : B1Form
    {

        public Form_Data()
        {
            FormType = "2004012021";
        }

        //[B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        //public virtual void OnBeforeMenuClick(BusinessObjectInfo pVal)
        //{
        //    //Globals.pFormD = "2004012021";
        //    //Form = new B1Forms("2004012021");
        //    //Frd.FormRD = Form;
        //    return;
        //}

        [B1Listener(BoEventTypes.et_FORM_DATA_LOAD, false)]
        public virtual void OnAfterFormDataLoad(BusinessObjectInfo pVal)
        {
            Form = new B1Forms(pVal.FormTypeEx);
            Globals.pFormD = "2004012021";

            Frd.FormRD = Form;
            return;
        }

        //[B1Listener(BoEventTypes.et_FORM_ACTIVATE, false)]
        //public virtual void OnBeforeFormActivate(BusinessObjectInfo pVal)
        //{
        //    //Form = new B1Forms("2004012021");
        //    // Globals.pFormD = "2004012021";

        //    //  Frd.FormRD = Form;
        //    return;
        //}


    }
}
