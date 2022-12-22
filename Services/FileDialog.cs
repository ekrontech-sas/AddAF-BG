using System.Threading;

namespace bagant.Services
{
    public class FileDialog
    {
        public static string OpenFolderDialog()
        {
            var oGetFileName = new GetFileNameClass
            {
                //InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
            };
            var threadGetExcelFile = new Thread(oGetFileName.GetFileName);
            threadGetExcelFile.SetApartmentState(ApartmentState.STA);
            threadGetExcelFile.Priority = ThreadPriority.Highest;
            threadGetExcelFile.Start();

            while (!threadGetExcelFile.IsAlive) ;
            Thread.Sleep(1);
            threadGetExcelFile.Join();

            return oGetFileName.Path;
        }
    }

}
