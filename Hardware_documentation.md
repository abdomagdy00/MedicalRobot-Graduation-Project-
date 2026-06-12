
## 🤖 Integration Doc - ESP32 & Medical Robot Backend
This file contains all the endpoints and events required to connect the ESP32 robot hardware to the server.

## 🔗 1. Base URLs
Please define these URLs as constants at the beginning of your code. Keep in mind that the server URL (Ngrok) may change upon server restart (unless the domain is static).

### Baseline Server URL:
https://body-onslaught-dancing.ngrok-free.dev/api

### SignalR Hub URL:

https://body-onslaught-dancing.ngrok-free.dev/robot-hub?deviceType=robot

## 📡 2. SignalR Hub - Receiving Commands from the Server
The ESP32 must maintain a constant connection (WebSocket/SignalR) to the Hub URL mentioned above.

The bot acts as a "listener" for these events to execute commands as soon as they arrive.

### A. Movement Event
Event Name: ReceiveMovementCommand

Received Data (Payload): String (single character)

Example values: "F" (forward), "B" (backward), "L" (left), "R" (right), "S" (stop).

### B. Manual Drawer Control Event
Event Name: DrawerCommand

Data Received (Payload): String.

Examples of values: "O1" (open first drawer), "C2" (close second drawer).

## 3. HTTP Requests - Sending Data from the Robot to the Server
Here, the ESP32 acts as a client, sending HTTP POST requests to the server in specific situations.

### First Request: 
Recording the Camera Stream Link (sent once upon startup)
Once the ESP32-CAM connects to Wi-Fi and obtains a local IP address, this IP address must be sent to the server.

Link: POST /Configuration/register-camera-ip

### Request Body:
 (⚠️ Important Note: The IP address must be sent as a direct text file enclosed in quotation marks, not as a JSON object, and without writing http://)

JSON
"192.168.1.50"
Expected Server Response: 200 OK

### 👤 Second Request: Drawer Opening by Face Recognition (HuskyLens)
When the camera recognizes the patient's face and retrieves the FaceId, the ESP32 sends this number to the server to determine which drawer to open.

Link: POST /Drawers/open-by-face/{faceId}

(Example: If the FaceId is 1001, the link becomes: /Drawers/open-by-face/1001)

Request Body: Empty - The data is sent in the same link.

Expected Server Response: You will receive a JSON file. Read the command value and use it to open the drawer's servo motor.

JSON
{
"command": "O1",

"message": "Drawer open command successfully sent to the physical robot."

}
### 🩺 Third Request: Uploading Medical Readings (Vitals)
When the sensors read the patient's vital signs, they are sent to be recorded in the database and displayed directly to the physician.

Link: POST /Records/upload

Request Body: The JSON Object must be sent with the following names and data types exactly:

JSON
{
"faceId": 1001,

"temperature": 37.5,

"heartRate": 80,

"spO2": 97
}
Expected Server Response: 200 OK

### ⚠️ Important Programming Tips for ESP32 Developers
Header Content-Type: For all HTTP POST requests (except IP requests), ensure the following header is added: Content-Type: application/json

Wi-Fi Connection: During local camera testing, the robot and the mobile device (or laptop) must be connected to the same Wi-Fi network for the mobile device to display the live feed.

### SignalR Library:
 It is recommended to use a reliable SignalR library with ESP32 such as the SignalRClient library available in the Arduino Library Manager, to ensure stable communication.
