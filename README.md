#Await Async for Arduino

This is a bit silly.

But it could be fun to tinker with.

##General idea:

```
void loop() {
	someFunction();
}

void someFunction() {
	string local1 = "a";
	string local2 = "b";

	string awaitThis = methodAsync();

	Serial.print(local1); 
	Serial.print(local2);
	Serial.println(awaitThis);
}

string methodAsync() {
	// does something async or awaitable
}
```

##Which gets converted to:

```
void loop() {
	someFunction();

	AsyncHandler.Loop();
}

void someFunction() {
	if(AsyncHandler.IsAwaiting("methodAsync")) {
		return;
	}

	string local1 = "a";
	string local2 = "b";

	localContext.someFunction.local1 = local1;
	localContext.someFunction.local2 = local2;

	AsyncHandler.Start("methodAsync"); // does the async statemachine handling
}

void someFunctionCallback1(awaitThis, local1, local2) { // function arguments filled with values when async method completes
	Serial.print(local1); 
	Serial.print(local2);
	Serial.println(awaitThis);
}
``` 