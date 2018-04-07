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

			string[] array = Directory.GetFiles(@"D:\f1", "*.bat",SearchOption.AllDirectories); // <-- Case-insensitive

			Regex rgx = new Regex(@"^(D):/.*exe");
			// Display all EXE files.
			Console.WriteLine("--- bat Files: ---");
			int i = 1;
			foreach (string name in array)
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
			oWB.SaveAs("d:\\test505.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
				false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
				Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

			oWB.Close();
			Console.ReadLine();
		}
	}
}
