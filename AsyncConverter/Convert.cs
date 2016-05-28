using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncConverter
{
	public static class Convert
	{
		public static string ToCodeString(this List<Variable> list, bool singleLine = false, bool trailingComma = false, bool startingComma = false, bool withValues = false, bool valuesOnly = false, bool namesOnly = false, bool setToThis = false)
		{
			if (setToThis || withValues)
			{
				singleLine = false;
			}

			string result = "";

			if (startingComma)
			{
				result += ", ";
			}

			bool first = true;
			foreach (Variable var in list)
			{
				if (first)
				{
					first = false;
				}
				else if (singleLine)
				{
					result += ", ";
				}

				if (!setToThis)
				{
					if (!withValues)
					{
						if (valuesOnly)
						{
							result += var.value;
						}
						else if (namesOnly)
						{
							result += var.name;
						}
						else
						{
							result += var.type + " " + var.name + ((!singleLine) ? ";" : "");
						}
					}
					else
					{
						result += var.name + " = " + var.value + ";";
					}
				}
				else
				{
					result += "this->" + var.name + " = " + var.name + ";";
				}

				if (!singleLine)
				{
					result += "\r\n";
				}
			}

			if (trailingComma)
			{
				result += ", ";
			}

			return result;
		}
		public static List<Variable> ToVariableList(this string code)
		{
			List<Variable> list = new List<Variable>();

			foreach(string part in code.Split(','))
			{
				string[] typename = part.Split(" ".ToCharArray(),2);

				Variable var = new Variable();

				var.name = typename[1].Replace(" ","");
				var.type = typename[0].Replace(" ","");

				list.Add(var);
			}

			return list;
		}
	}
}
