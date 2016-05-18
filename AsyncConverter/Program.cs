using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncConverter
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
			void postingMessage() {
				if (wait.isActive()) { return; }

				String message = "Jeej";

				wait.startMethod(message, postingMessageCallback1, millis(), 1000);
			}
			void postingMessageCallback1(String message) {
				Serial.println(message);
			}
			*/
			AwaitCall test = new AwaitCall();
			test.methodType = "void";
			test.methodName = "postingMesage";
			test.methodBodyBefore = @"String message = ""Jeej"";";
			test.methodBodyAfter = @"Serial.println(message);";
			test.localContext.Add(new Variable { name = "message", type = "String", value = "\"Jeej\"" });
			test.arguments.Add(new Variable { value = "millis()" });
			test.arguments.Add(new Variable { value = "1000" });

			Console.WriteLine(test.ToCode());

			Console.ReadLine();
		}
	}

	public static class Convert
	{
		public static string ToString(this List<Variable> list, bool singleLine = false, bool trailingComma = false, bool startingComma = false, bool withValues = false, bool valuesOnly = false, bool setToThis = false)
		{
			if(setToThis || withValues)
			{
				singleLine = false;
			}

			string result = "";

			if(startingComma)
			{
				result += ", ";
			}

			bool first = true;
			foreach(Variable var in list)
			{
				if (first)
				{
					first = false;
				}
				else
				{
					result += ", ";
				}

				if(!setToThis)
				{
					if(!withValues)
					{
						if(!valuesOnly)
						{
							result += var.name;
						}
						else
						{
							result += var.value;
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

				if(!singleLine)
				{
					result += "\r\n";
				}
			}

			if(trailingComma)
			{
				result += ", ";
			}

			return result;
		}
	}

	public class AwaitCall
	{
		// 0 = method type
		// 1 = method name
		// 2 = method body before call
		// 3 = method body after call
		// 4 = local context in method call, with trailing comma
		// 5 = local context, single line
		// 6 = method parameters, starting with comma, values only
		private string code = @"
			{0} {1}() {{
				if (wait.isActive()) {{ return; }}

				{2}

				wait.startMethod({4}{1}Callback1{6});
			}}
			{0} {1}Callback1({5})
			{{
				{3}
			}}
		";

		public string methodType = "";
		public string methodName = "";
		public string methodBodyBefore = "";
		public string methodBodyAfter = "";
		public List<Variable> localContext = new List<Variable>();
		public List<Variable> arguments = new List<Variable>();

		public string ToCode()
		{
			return string.Format(code, 
				methodType,
				methodName,
				methodBodyBefore,
				methodBodyAfter,
				localContext.ToString(singleLine: true, trailingComma: true),
				localContext.ToString(singleLine: true),
				arguments.ToString(singleLine: true, startingComma: true, valuesOnly: true)
				);
		}
	}

	public class AsyncMethod
	{
		// 1 = method name
		// 2 = local context, single line
		// 3 = local context, multi line
		// 4 = local context, set to this->
		// 5 = method parameters, single line, starting with comma
		// 6 = method parameters, multi line
		// 7 = method parameters, set to this->
		// 8 = original methods, with returns replaced
		private string code = @"
			class $1
			{
			private:
				bool _isActive;
				void(*callback)($2);

				// local context
				$3

				// method parameters
				$6

			public:
				void startMethod($2, void(*callback)($2)$5) {
					$4

					this->callback = callback;

					$7

					this->_isActive = true;
				}

				bool isActive() {
					return _isActive;
				}

				void tryMethod() {
					// original method with returns edited
					$8
					// / original method with returns edited
				}
			} $1;
		";

		public string methodName = "";
		public List<Variable> arguments = new List<Variable>();
		public List<Variable> localContext = new List<Variable>();
		// Variable returnVariable;

		public AsyncMethod()
		{
		}


	}

	public struct Variable {
		public string name;
		public string type;
		public string value;
	}
}
