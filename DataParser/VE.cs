using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser
{
    class VE
    {
        private static void CreateBatchFile()
        {
            string VE_Dynamic_Load = String.Empty;
            VE_Dynamic_Load += @"SET Company=VE"
                + Environment.NewLine;
            VE_Dynamic_Load += @"SET TimeOut=timeout /t 10"
                + Environment.NewLine;

            VE_Dynamic_Load += @"Set ConfigValue=E10Test"
                + Environment.NewLine;
            VE_Dynamic_Load += @"Set DMT=C:\\Epicor\\AzureClient\\Client\\DMT.exe"
                + Environment.NewLine;
            VE_Dynamic_Load += @"Set Folder=C:\Dropbox\EpicorImplementation\VE\VE-DataDump-Load\"
                + Environment.NewLine
                + Environment.NewLine;

            VE_Dynamic_Load += @"Set Prog=GL02-QuantityAdjustments.csv"
                + Environment.NewLine
                + "%DMT% -Import=\"Quantity Adjustment\" -ConfigValue=%ConfigValue% -User=DMT_%Company% -pass=%PW% -Add -Update -Source=\"%Folder%%Prog% \""
                + Environment.NewLine;
            VE_Dynamic_Load += @"timeout /t 120"
                + Environment.NewLine;
            VE_Dynamic_Load += @"Set Prog=GL05-OrderHeaders.csv"
                + Environment.NewLine
                + "%DMT% -Import=\"Sales Order Header\" -ConfigValue=%ConfigValue% -User=DMT_%Company% -pass=%PW% -Add -Update -Source=\"%Folder%%Prog% \""
                + Environment.NewLine;
            VE_Dynamic_Load += @"timeout /t 120"
                + Environment.NewLine;
            VE_Dynamic_Load += @"Set Prog=GL06-OrderDetails.csv"
                + Environment.NewLine
                + "%DMT% -Import=\"Sales Order Detail\" -ConfigValue=%ConfigValue% -User=DMT_%Company% -pass=%PW% -Add -Update -Source=\"%Folder%%Prog% \""
                + Environment.NewLine;
            VE_Dynamic_Load += @"timeout /t 120"
                + Environment.NewLine;
            VE_Dynamic_Load += @"Set Prog=GL07-OrderReleases.csv"
                + Environment.NewLine
                + "%DMT% -Import=\"Sales Order Release\" -ConfigValue=%ConfigValue% -User=DMT_%Company% -pass=%PW% -Add -Update -Source=\"%Folder%%Prog% \""
                + Environment.NewLine;
            VE_Dynamic_Load += @"timeout /t 120"
                + Environment.NewLine;

            // Install Jobs
            VE_Dynamic_Load += @"Set Prog=GL10-JobHeaders.csv"
                + Environment.NewLine
                + "%DMT% -Import=\"Job Header\" -ConfigValue=%ConfigValue% -User=DMT_%Company% -pass=%PW% -Add -Update -Source=\"%Folder%%Prog% \""
                + Environment.NewLine;
            VE_Dynamic_Load += @"timeout /t 120"
                + Environment.NewLine;
            VE_Dynamic_Load += @"Set Prog=GL11-JobOperations.csv"
                + Environment.NewLine
                + "%DMT% -Import=\"Job Operation\" -ConfigValue=%ConfigValue% -User=DMT_%Company% -pass=%PW% -Add -Update -Source=\"%Folder%%Prog% \""
                + Environment.NewLine;
            VE_Dynamic_Load += @"timeout /t 120"
                + Environment.NewLine;
            VE_Dynamic_Load += @"Set Prog=GL12-JobMaterials.csv"
                + Environment.NewLine
                + "%DMT% -Import=\"Job Material\" -ConfigValue=%ConfigValue% -User=DMT_%Company% -pass=%PW% -Add -Update -Source=\"%Folder%%Prog% \""
                + Environment.NewLine;
            VE_Dynamic_Load += @"timeout /t 120"
                + Environment.NewLine;
            VE_Dynamic_Load += @"Set Prog=GL13-JobProd.csv"
                + Environment.NewLine
                + "%DMT% -Import=\"Job Prod\" -ConfigValue=%ConfigValue% -User=DMT_%Company% -pass=%PW% -Add -Update -Source=\"%Folder%%Prog% \""
                + Environment.NewLine;
            VE_Dynamic_Load += @"timeout /t 120"
                + Environment.NewLine;
            VE_Dynamic_Load += @"Set Prog=GL49-JobMtlAdjustments.csv"
                + Environment.NewLine
                + "%DMT% -Import=\"Job Mtl Adjustment\" -ConfigValue=%ConfigValue% -User=DMT_%Company% -pass=%PW% -Add -Update -Source=\"%Folder%%Prog% \""
                + Environment.NewLine;
            VE_Dynamic_Load += @"timeout /t 120"
                + Environment.NewLine;

            // PO

            VE_Dynamic_Load += @"Set Prog=GL20-POHeaders.csv"
                + Environment.NewLine
                + "%DMT% -Import=\"Job Mtl Adjustment\" -ConfigValue=%ConfigValue% -User=DMT_%Company% -pass=%PW% -Add -Update -Source=\"%Folder%%Prog% \""
                + Environment.NewLine;
            VE_Dynamic_Load += @"timeout /t 120"
                + Environment.NewLine;
            VE_Dynamic_Load += @"Set Prog=GL21-PODetails.csv"
                + Environment.NewLine
                + "%DMT% -Import=\"Job Mtl Adjustment\" -ConfigValue=%ConfigValue% -User=DMT_%Company% -pass=%PW% -Add -Update -Source=\"%Folder%%Prog% \""
                + Environment.NewLine;
            VE_Dynamic_Load += @"timeout /t 120"
                + Environment.NewLine;
            VE_Dynamic_Load += @"Set Prog=GL22-POReleases.csv"
                + Environment.NewLine
                + "%DMT% -Import=\"Job Mtl Adjustment\" -ConfigValue=%ConfigValue% -User=DMT_%Company% -pass=%PW% -Add -Update -Source=\"%Folder%%Prog% \""
                + Environment.NewLine;
            VE_Dynamic_Load += @"timeout /t 120"
                + Environment.NewLine;
            VE_Dynamic_Load += @"Set Prog=GL21-POHeaderApprovals.csv"
                + Environment.NewLine
                + "%DMT% -Import=\"Job Mtl Adjustment\" -ConfigValue=%ConfigValue% -User=DMT_%Company% -pass=%PW% -Add -Update -Source=\"%Folder%%Prog% \""
                + Environment.NewLine;
            VE_Dynamic_Load += @"timeout /t 120"
                + Environment.NewLine;

            // Service Call/Jobs
            VE_Dynamic_Load += @"Set Prog=GL40-Service_Call_Center_Combined.csv"
                + Environment.NewLine
                + "%DMT% -Import=\"Service Call Center Combined\" -ConfigValue=%ConfigValue% -User=DMT_%Company% -pass=%PW% -Add -Update -Source=\"%Folder%%Prog% \""
                + Environment.NewLine;
            VE_Dynamic_Load += @"timeout /t 120"
                + Environment.NewLine;
            VE_Dynamic_Load += @"Set Prog=GL41-FSJobOperations.csv"
                + Environment.NewLine
                + "%DMT% -Import=\"Service Job Operation\" -ConfigValue=%ConfigValue% -User=DMT_%Company% -pass=%PW% -Add -Update -Source=\"%Folder%%Prog% \""
                + Environment.NewLine;
            VE_Dynamic_Load += @"timeout /t 120"
                + Environment.NewLine;
            VE_Dynamic_Load += @"Set Prog=GL42-FSJobMaterials.csv"
                + Environment.NewLine
                + "%DMT% -Import=\"Service Job Material\" -ConfigValue=%ConfigValue% -User=DMT_%Company% -pass=%PW% -Add -Update -Source=\"%Folder%%Prog% \""
                + Environment.NewLine;
            VE_Dynamic_Load += @"timeout /t 120"
                + Environment.NewLine;
            VE_Dynamic_Load += @"Set Prog=GL46-FSJobHeadersEngineered.csv"
                + Environment.NewLine
                + "%DMT% -Import=\"Service Job Material\" -ConfigValue=%ConfigValue% -User=DMT_%Company% -pass=%PW% -Add -Update -Source=\"%Folder%%Prog% \""
                + Environment.NewLine;
            VE_Dynamic_Load += @"timeout /t 120"
                + Environment.NewLine;

            File.WriteAllText("VE_Dynamic_Load.bat", VE_Dynamic_Load);
        }

    }
}
