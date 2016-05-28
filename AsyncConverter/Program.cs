using System;
using System.IO;
using System.Collections.Generic;

using AsyncConverter.Generators;
using AsyncConverter.Interpreters;

namespace AsyncConverter
{
	class Program
	{
		static void Main(string[] args)
		{
			SourceCode.OpenFile(@"D:\AwaitAsyncforArduino\AwaitAsyncforArduino\AwaitAsyncforArduino.ino");

			SourceCode.ParseFile();

			foreach(string line in SourceCode.lines)
			{
				Console.WriteLine(line);
			}

			Console.ReadLine();

		}
		static void TestGenerators() { 
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

			/*
					class wait
					{
						private:
				bool _isActive;
						void(*callback)(String message);

						// local context
						String message;

						// method parameters
						long start;
						long delay;

						public:
				void startMethod(String message, void(*callback)(String message), long start, long delay) {
					this->message = message;

					this->callback = callback;

					this->start = start;
					this->delay = delay;

					this->_isActive = true;
				}

					bool isActive()
					{
						return _isActive;
					}

					void tryMethod()
					{
						// original method with returns edited
						if (millis() - start > delay)
						{
							// edit
							_isActive = false;
							callback(message);
							// / edit
						}
						// / original method with returns edited
					}
				}
				wait;
			*/

			AsyncMethod test2 = new AsyncMethod();

			test2.methodName = "wait";
			test2.methodBody = @"if (Serial.available()) {{
			String string;
			string = Serial.readString();

			if (string == ""a"")
			{{
				digitalWrite(13, true);
			}}
			else if (string == ""b"")
			{{
				digitalWrite(13, false);
			}}

			return;
		}}";
			test2.localContext.Add(new Variable { name = "message", type = "String" });
			test2.arguments.Add(new Variable { name = "start", type = "long" });
			test2.arguments.Add(new Variable { name = "delay", type = "long" });

			Console.WriteLine(test2.ToCode());

			Console.ReadLine();
		}
	}
}