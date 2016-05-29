using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;

using AsyncConverter.Generators;

namespace AsyncConverter.Interpreters
{
	public static class SourceCode
	{
		public static string[] lines;
		public static List<AsyncMethod> asyncMethods;
		public static List<AwaitCall> awaitCalls;

		public static void OpenFile(string folder, string file)
		{
			if(File.Exists(folder + file))
			{
				lines = File.ReadAllLines(folder + file);
			}
		}

		public static void SaveFile(string folder, string file)
		{
			File.WriteAllLines(folder + file, lines);
		}

		public static void ParseFile()
		{
			ParseAsyncMethods();
			ParseAwaitCalls();

			foreach (AwaitCall method in awaitCalls)
			{
				lines = method.ToCode().Split('\n').Concat(lines).ToArray();
			}

			foreach (AsyncMethod method in asyncMethods)
			{
				if (method.callerName != "")
				{
					lines = method.ToCode().Split('\n').Concat(lines).ToArray();
				}
			}
		}

		public static void ParseAsyncMethods()
		{
			IEnumerable<Method> methods = GetMethods("async_",true);
			asyncMethods = new List<AsyncMethod>();

			// comment out all normal async methods
			// create AsyncMethods from Methods

			foreach (Method method in methods)
			{
				for (int i = method.startingLine; i <= method.endingLine; i++)
				{
					lines[i] = "//" + lines[i];
				}

				asyncMethods.Add(AsyncMethod.CreateFromMethod(method));
			}

			
		}

		public static void ParseAwaitCalls()
		{
			IEnumerable<Method> methods = GetMethods("async_", false);
			awaitCalls = new List<AwaitCall>();

			Regex awaitCall = new Regex(@"await\(([A-Za-z0-9_]*)\(");
			Match callMatches;

			// comment out all normal methods with an await call
			foreach(Method method in methods)
			{
				foreach(string line in method.body)
				{
					if(awaitCall.IsMatch(line))
					{
						callMatches = awaitCall.Match(line);

						string asyncMethodName = callMatches.Groups[1].Value;
						AsyncMethod asyncMethod = asyncMethods.FindMethodByName(asyncMethodName).Clone();

						asyncMethod.callerName = method.name;
						asyncMethods.Add(asyncMethod);

						for (int i = method.startingLine; i <= method.endingLine; i++)
						{
							lines[i] = "//" + lines[i];
						}

						awaitCalls.Add(AwaitCall.CreateFromMethod(method));

					}
				}
			}
		}

		public static IEnumerable<Method> GetMethods(string startingWith, bool hasWhile)
		{
			Method method = new Method();
			Regex methodFinder = new Regex(startingWith + @"([A-Za-z0-9\-_]+)[\s]+([A-Za-z0-9\-_]+)\(([A-Za-z0-9\s\-_,]*)\)[\s]*\{");
			Match methodMatches;
			Regex whileFinder = new Regex(@"while[\s]+\([\s]*true[\s]*\)[\s]*\{");

			bool methodFound = false;
			bool whileFound = false;
			int level = 0;
			int i = 0;
			int methodStartAt = 0;

			foreach(string line in lines)
			{
				if(methodFinder.IsMatch(line))
				{
					methodMatches = methodFinder.Match(line);

					methodFound = true;
					method = new Method();
					method.name = methodMatches.Groups[2].Value;
					method.returnType = methodMatches.Groups[1].Value;
					method.arguments = methodMatches.Groups[3].Value;

					method.startingLine = i;

					methodStartAt = i + 1;

					whileFound = !hasWhile;
				}
				else if(methodFound && whileFinder.IsMatch(line))
				{
					whileFound = true;
					methodStartAt = i + 1;
				}

				if(line.Contains("{"))
				{
					level++;
				}

				if(line.Contains("}"))
				{
					level--;
				}

				if(methodFound && whileFound && level == 0)
				{
					int k = 0;

					string[] body = new string[i - methodStartAt - ((hasWhile) ? 1 : 0)];

					for (int j = methodStartAt; j < i - ((hasWhile) ? 1 : 0); j++)
					{
						body[k++] = lines[j];
					}

					method.body = body;
					method.endingLine = i;

					yield return method;

					methodFound = false;
					whileFound = false;
				}

				i++;
			}
		}
	}
}
