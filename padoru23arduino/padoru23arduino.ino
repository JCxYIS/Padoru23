#define BTN_PIN 13

#include <WiFi.h>
#include <HTTPClient.h>

const char* ssid = "";
const char* password = "";

int lastState = 1;

void setup() {
  Serial.begin(9600);
  pinMode(BTN_PIN, INPUT); 

  WiFi.begin(ssid, password);
  Serial.println("Connecting");
  while(WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.print("Connected to WiFi network with IP Address: ");
  Serial.println(WiFi.localIP());
}

void loop() {
  // put your main code here, to run repeatedly:
  int btnState = digitalRead(BTN_PIN);

  //Send an HTTP request 
  if (btnState == 0 && lastState == 1) {
    //Check WiFi connection status
    if(WiFi.status()== WL_CONNECTED){
      HTTPClient http;
      String serverPath = "http://140.115.54.31:9065/padoru";

      http.begin(serverPath.c_str());
      int httpResponseCode = http.GET();
      
      if (httpResponseCode>0) {
        Serial.print("HTTP Response code: ");
        Serial.println(httpResponseCode);
        // String payload = http.getString();
        // Serial.println(payload);
      }
      else {
        Serial.print("Error code: ");
        Serial.println(httpResponseCode);
      }
      // Free resources
      http.end();
    }
    else {
      Serial.println("WiFi Disconnected");
    }    
  }
  lastState = btnState;
}
