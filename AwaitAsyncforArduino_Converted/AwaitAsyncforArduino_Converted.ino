
			class waitForpostingMessage
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

				bool isActive() {
					return _isActive;
				}

				void tryMethod() {
					// original method with returns edited

							if (millis() - start > delay) {
			_isActive = false; callback(message);
		}

					// / original method with returns edited
				}
			} waitForpostingMessage;
		

			class methodAsyncForpinHandling
			{
			private:
				bool _isActive;
				void(*callback)();

				// local context
				

				// method parameters
				

			public:
				void startMethod(void(*callback)()) {
					

					this->callback = callback;

					

					this->_isActive = true;
				}

				bool isActive() {
					return _isActive;
				}

				void tryMethod() {
					// original method with returns edited

							if (Serial.available()) {
			String string;
			string = Serial.readString();
			
			if (string == "a") {
				digitalWrite(13, true);
			}
			else if (string == "b") {
				digitalWrite(13, false);
			}

			_isActive = false; callback();
		}

					// / original method with returns edited
				}
			} methodAsyncForpinHandling;
		

			void postingMessage() {
				if (waitForpostingMessage.isActive()) { return; }

				
	String message = "Jeej";

				waitForpostingMessage.startMethod(message, postingMessageCallback1, millis(), 1000);
			}
			void postingMessageCallback1(String message)
			{
				
	Serial.println(message);
			}
		

			void pinHandling() {
				if (methodAsyncForpinHandling.isActive()) { return; }

				

				methodAsyncForpinHandling.startMethod(pinHandlingCallback1);
			}
			void pinHandlingCallback1()
			{
				
			}
		
#define await(a) a
#define async_void void

void setup() {
	pinMode(13, OUTPUT);

	Serial.begin(9600);
}

void loop() {
	pinHandling();
	postingMessage();
}

//async_void pinHandling() {
//	await(methodAsync());
//}

//async_void postingMessage() {
//	String message = "Jeej";
//	await(wait(millis(), 1000));
//	Serial.println(message);
//}

//async_void methodAsync() {
//	while (true) {
//		if (Serial.available()) {
//			String string;
//			string = Serial.readString();
//			
//			if (string == "a") {
//				digitalWrite(13, true);
//			}
//			else if (string == "b") {
//				digitalWrite(13, false);
//			}
//
//			return;
//		}
//	}
//}

//async_void wait(long start, long delay) {
//	while (true) {
//		if (millis() - start > delay) {
//			return;
//		}
//	}
//}