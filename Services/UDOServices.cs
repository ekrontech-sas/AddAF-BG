using B1Framework.B1Frame;
using SAPbobsCOM;
using System;

namespace bagant.Services
{
    class UDOServices
    {
        private readonly GeneralService _generalServices;
        private readonly GeneralDataParams _generalDataParams;
        private GeneralData _generalData;

        private String HeaderTable { get; set; }
        private String ChildTable { get; set; }


        public UDOServices(string headerTable, String childTable = null)
        {
            try
            {
                HeaderTable = headerTable;
                ChildTable = childTable;

                _generalServices = B1Connections.CpService.GetGeneralService(HeaderTable);
                _generalDataParams = (GeneralDataParams)_generalServices.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
            }
            catch(Exception ex)
            {
                Log.Info("(Exception) UDOServices => " + ex.Message);
            }
        }

        public int LoadDocumentHeader(string fieldFind, object docKey)
        {
            try
            {
                _generalDataParams.SetProperty(fieldFind, docKey);
                _generalData = _generalServices.GetByParams(_generalDataParams);
            }
            catch (Exception ex) 
            {
                Log.Info("(Exception) LoadDocumentHeader => "+ex.Message);
            }
            return -1;
        }

        public int UpdateHeaderDocument(object value, string field = "DocEntry")
        {
            try
            {
                //_generalDataParams.SetProperty(field, value);
                _generalData = _generalServices.GetByParams(_generalDataParams);
                _generalData.SetProperty(field, value);
                _generalServices.Update(_generalData);
                return 0;
            }
            catch (Exception ex) 
            {
                Log.Info("(Exception) UpdateHeaderDocument => " + ex.Message);
            }
            return -1;
        }

        public void Update()
        {
            try
            {
                _generalServices.Update(_generalData);
                Log.Debug("Mensaje: " + B1Connections.DiCompany.GetLastErrorDescription());
            }
            catch (Exception ex)
            {
                Log.Info("(Exception) Update => " + ex.Message);
            }
        }

        public int AddLinesDocument(string[] field, object[] value)
        {
            try
            {
                _generalData = _generalServices.GetByParams(_generalDataParams);

                var lineas = _generalData.Child(ChildTable);
                lineas.Add();

                int linea = lineas.Count - 1;
                for (int i = 0; i < field.Length; i++)
                    lineas.Item(linea).SetProperty(field[i], value[i]);

                _generalServices.Update(_generalData);
                return 0;
            }
            catch (Exception ex) { }
            return -1;
        }

        public int UpdateLinesDocument(int nroLinea, string field, object value)
        {
            if (nroLinea != -1)
            {
                try
                {
                    _generalData = _generalServices.GetByParams(_generalDataParams);
                    var lineas = _generalData.Child(ChildTable);
                    lineas.Item(nroLinea).SetProperty(field, value);
                    _generalServices.Update(_generalData);
                    return 0;
                }
                catch (Exception ex)
                {
                    Log.Info("Exception (UpdateLinesDocument) => " + ex.Message);
                }
            }
            return -1;
        }

        public int LoadLinesDocument(int nroLinea, string field, object value)
        {
            try
            {
                _generalData = _generalServices.GetByParams(_generalDataParams);

                var lineas = _generalData.Child(ChildTable);
                lineas.Item(nroLinea).SetProperty(field, value);

                return 0;
            }
            catch (Exception ex) { }
            return -1;
        }

        public GeneralDataCollection GetLines()
        {
            return _generalData.Child(ChildTable);
        }

        public int SearchValueLinesA(string fieldToSearch, string valueToSearch)
        {
            var lineas = GetLines();

            for (int i = 0; i < lineas.Count; i++)
                if (lineas.Item(i).GetProperty(fieldToSearch).ToString().Equals(valueToSearch))
                    return i;
            return -1;
        }

        public void Close()
        {
            B1Connections.FlushMemory(_generalServices);
            B1Connections.FlushMemory(_generalDataParams);
            B1Connections.FlushMemory(_generalData);
        }
    }
}
