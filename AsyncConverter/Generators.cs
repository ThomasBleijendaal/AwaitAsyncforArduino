using System.Collections.Generic;

namespace AsyncConverter.Generators
{
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
				localContext.ToCodeString(singleLine: true, trailingComma: true),
				localContext.ToCodeString(singleLine: true),
				arguments.ToCodeString(singleLine: true, startingComma: true, valuesOnly: true)
				);
		}
	}

	public class AsyncMethod
	{
		// 0 = method name
		// 1 = local context, single line
		// 2 = local context, multi line
		// 3 = local context, set to this->
		// 4 = method parameters, single line, starting with comma
		// 5 = method parameters, multi line
		// 6 = method parameters, set to this->
		// 7 = local context, single line, names only
		private string code1 = @"
			class {0}
			{{
			private:
				bool _isActive;
				void(*callback)({1});

				// local context
				{2}

				// method parameters
				{5}

			public:
				void startMethod({1}, void(*callback)({1}){4}) {{
					{3}

					this->callback = callback;

					{6}

					this->_isActive = true;
				}}

				bool isActive() {{
					return _isActive;
				}}

				void tryMethod() {{
					// original method with returns edited

					";
		private string code2 = @"

					// / original method with returns edited
				}}
			}} {0};
		";
		private string returnReplace = @"_isActive = false; callback({7});";

		public string methodName = "";
		public string methodBody = "";
		public List<Variable> arguments = new List<Variable>();
		public List<Variable> localContext = new List<Variable>();
		// Variable returnVariable;

		public string ToCode()
		{
			string replacedMethodBody = methodBody.Replace("return;", returnReplace);

			string code = code1 + replacedMethodBody + code2;

			return string.Format(code,
				methodName,
				localContext.ToCodeString(singleLine: true),
				localContext.ToCodeString(),
				localContext.ToCodeString(setToThis: true),
				arguments.ToCodeString(singleLine: true, startingComma: true),
				arguments.ToCodeString(),
				arguments.ToCodeString(setToThis: true),
				localContext.ToCodeString(singleLine: true, namesOnly: true));
		}

		public static AsyncMethod CreateFromMethod(Method method)
		{
			AsyncMethod m = new AsyncMethod();

			m.methodName = method.name;
			m.methodBody = method.body;
			m.arguments = method.arguments.ToVariableList();

			return m;
		}

	}
}
