using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Excel;
namespace exeSearch
{
	class Program
	{
		static void Main(string[] args)
		{
			Microsoft.Office.Interop.Excel.Application oXL;
			Microsoft.Office.Interop.Excel._Workbook oWB;
			Microsoft.Office.Interop.Excel._Worksheet oSheet;
			Microsoft.Office.Interop.Excel.Range oRng;
			oXL = new Microsoft.Office.Interop.Excel.Application();
			oXL.Visible = false;
			//Get a new workbook.
			oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(""));
			oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;

            
               // string[] array 
                 List<string> list   = new List<string>();
                 //list.AddRange(GetFiles(@"X:\Jobs", ".bat"));
                     
                list= GetFiles(@"X:\Jobs", "*.bat").ToList();
                    // list.AddRange(Directory.GetFiles(@"X:\Jobs", "*.bat", SearchOption.AllDirectories).Where(s => !s.Contains(@"X:\Jobs\SunGard_r2\Batch\Scripts\ftp\transfer")));
                     foreach (string n in list)
                     {
                         Console.WriteLine(n.ToString());
                     }

			Regex rgx = new Regex(@"^(D):.*exe");
			// Display all EXE files.
			Console.WriteLine("--- bat Files: ---");
			int i = 1;
			foreach (string name in list)
			{
                				foreach (string line in File.ReadLines(name))
				{
					if (rgx.IsMatch(line))
					{
						Console.WriteLine(name.ToString()+"...."+line.ToString());
						//Fill A2:B6 with an array of values (First and Last Names).
						oSheet.Cells[i,1] = name.ToString();
						oSheet.Cells[i, 2] = line.ToString();
						i++;
					}
				}
				
			}
           
			oXL.UserControl = false;
			oWB.SaveAs("H:\\final1.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
				false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
				Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

			oWB.Close();
			Console.ReadLine();
		}

        public static IEnumerable<string> GetFiles(string root, string searchPattern)
        {
            Stack<string> pending = new Stack<string>();
            pending.Push(root);
            while (pending.Count != 0)
            {
                var path = pending.Pop();
                string[] next = null;
                try
                {
                    next = Directory.GetFiles(path, searchPattern);
                }
                catch { }
                if (next != null && next.Length != 0)
                    foreach (var file in next) yield return file;
                try
                {
                    if(path != @"X:\Jobs\SunGard_r2\Batch\Scripts\ftp\transfer" )
                    {
                    next = Directory.GetDirectories(path);
                    foreach (var subdir in next) pending.Push(subdir);
                    }
                }
                catch { }
            }
        }

	}
}
