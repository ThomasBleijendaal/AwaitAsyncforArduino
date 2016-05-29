using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsyncConverter.Generators;
using AsyncConverter.Interpreters;

namespace AsyncConverter
{
	public static class Extensions
	{
		public static string ToCodeString(this List<Variable> list, bool singleLine = false, bool trailingComma = false, bool startingComma = false, bool withValues = false, bool valuesOnly = false, bool namesOnly = false, bool setToThis = false)
		{
			if (list.Count == 0)
			{
				return "";
			}

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

			if (code != "")
			{
				
				foreach (string part in code.Split(','))
				{
					string[] typename = part.Trim().Split(" ".ToCharArray(), 2);

					Variable var = new Variable();

					var.name = typename[1].Trim();
					var.type = typename[0].Trim();

					list.Add(var);
				}

			}

			return list;
		}
		public static List<Variable> ToCallList(this string code)
		{
			List<Variable> list = new List<Variable>();

			if (code != "")
			{

				foreach (string part in code.Split(','))
				{
					Variable var = new Variable();

					var.value = part.Trim();

					list.Add(var);
				}

			}

			return list;
		}
		public static AsyncMethod FindMethodByName(this List<AsyncMethod> list, string methodName, string callerName)
		{
			foreach (AsyncMethod method in list)
			{
				if (method.methodName == methodName && method.callerName == callerName)
				{
					return method;
				}
			}

			return null;
		}
		public static AsyncMethod FindMethodByName(this List<AsyncMethod> list, string methodName)
		{
			foreach(AsyncMethod method in list)
			{
				if(method.methodName == methodName)
				{
					return method;
				}
			}

			return null;
		}
	}
}
