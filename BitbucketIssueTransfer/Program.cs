using BitbucketIssueTransfer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitbucketIssueTransfer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 3)
            {
                Console.WriteLine("Use Command: ProgramName Version{v1|v2} SourceFileName DestFileName");
                Console.ReadKey();
                return;
            }

            try
            {
                string version = args[0];
                string sourceFile = args[1];
                string distFile = args[2];

                if (version.Equals("v2", StringComparison.InvariantCultureIgnoreCase))
                {
                    new IssueTransfer().ExportV2(sourceFile, distFile);
                }
                else
                {
                    new IssueTransfer().Export(sourceFile, distFile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            Console.ReadKey();
        }
    }
}
