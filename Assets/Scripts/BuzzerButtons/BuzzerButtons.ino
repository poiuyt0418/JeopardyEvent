#include <Adafruit_CircuitPlayground.h>
#define button1 A2
#define button2 A3
#define button3 A6
#define button4 A7
bool button1trigger = false;
bool button2trigger = false;
bool button3trigger = false;
bool button4trigger = false;
int buttonValue = 0;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  CircuitPlayground.begin();
  pinMode(button1, INPUT_PULLUP);
  pinMode(button2, INPUT_PULLUP);
  pinMode(button3, INPUT_PULLUP);
  pinMode(button4, INPUT_PULLUP);
  attachInterrupt(digitalPinToInterrupt(button1),but1,FALLING);
  attachInterrupt(digitalPinToInterrupt(button2),but2,FALLING);
  attachInterrupt(digitalPinToInterrupt(button3),but3,FALLING);
  attachInterrupt(digitalPinToInterrupt(button4),but4,FALLING);
}

void but1() {
  button1trigger = true;
}

void but2() {
  button2trigger = true;
}

void but3() {
  button3trigger = true;
}

void but4() {
  button4trigger = true;
}

void loop() {
  if(button1trigger || button2trigger || button3trigger || button4trigger)
  {
    if(button1trigger)buttonValue+=1000;
    if(button2trigger)buttonValue+=100;
    if(button3trigger)buttonValue+=10;
    if(button4trigger)buttonValue+=1;
    button1trigger = false;
    button2trigger = false;
    button3trigger = false;
    button4trigger = false;
    Serial.println(buttonValue);
    buttonValue = 0;
  }
  //Serial.println();
  //delay(10)
}
