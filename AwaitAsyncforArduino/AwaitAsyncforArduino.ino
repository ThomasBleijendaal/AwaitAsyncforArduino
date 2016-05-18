#define await(a) a
#define async void
#define async_bool bool


void setup() {
	pinMode(13, OUTPUT);

	Serial.begin(9600);
}

void loop() {
	pinHandling();
	postingMessage();
}

void pinHandling() {
	await(methodAsync());
}

void postingMessage() {
	await(wait(millis(), 1000));
	Serial.println("Jeej");
}

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

async_bool wait(long start, long delay) {
	while (true) {
		if (millis() - start > delay) {
			return true;
		}
	}
}