#define await(a) a
#define async void

// original
/*
async methodAsync() {
	while (true) {
		if (Serial.available()) {
			String string;
			string = Serial.readString();

			if (string == "a") {
				digitalWrite(13, true);
			}
			else if (string == "b") {
				digitalWrite(13, false);
			}

			return;
		}
	}
}
*/
// / original

// generated code
class methodAsync
{
private:
	bool _isActive;
	void(*callback)(void);

public:
	void startMethod(void(*callback)(void)) {
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
			// edit
			_isActive = false;
			callback();
			// / edit
		}

		// / original method with returns edited
	}
} methodAsync;
// / generated code

// original code
/*
async wait(long start, long delay) {
	while (true) {
		if (millis() - start > delay) {
			return;
		}
	}
}
*/
// / original code

// generated code
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

	bool isActive() {
		return _isActive;
	}

	void tryMethod() {
		// original method with returns edited
		if (millis() - start > delay) {
			// edit
			_isActive = false;
			callback(message);
			// / edit
		}
		// / original method with returns edited
	}
} wait;
// / generated code

class AsyncHandler
{
public:
	void loop() {
		if (methodAsync.isActive()) {
			methodAsync.tryMethod();
		}

		if (wait.isActive()) {
			wait.tryMethod();
		}
	}
} AsyncHandler;

void setup() {
	pinMode(13, OUTPUT);

	Serial.begin(9600);

	// generated code

	// / generated code
}

void loop() {
	pinHandling();
	postingMessage();

	// generated code
	AsyncHandler.loop();
	// / generated code
}

// original
/*
void pinHandling() {
	await(methodAsync());
}
*/
// / original

void pinHandling() {
	if (methodAsync.isActive()) { return; }
	methodAsync.startMethod(pinHandlingCallback1);
}
void pinHandlingCallback1() {

}

// original
/*
void postingMessage() {
	String message = "Jeej";

	await(wait(millis(), 1000));
	Serial.println(message);
}
*/
// / original

void postingMessage() {
	if (wait.isActive()) { return; }

	String message = "Jeej";

	wait.startMethod(message, postingMessageCallback1, millis(), 1000);
}
void postingMessageCallback1(String message) {
	Serial.println(message);
}



