### 📂Medical Robot - API Documentation
### 🌐 1. Base Configuration
Since the server is local (localhost), ensure that your mobile device and the server are connected to the same Wi-Fi network.

### **Base URL: http://[YOUR_LOCAL_IP]:[PORT]/api**

### SignalR Hub URL: http://[YOUR_LOCAL_IP]:[PORT]/robot-hub

Note: Replace [YOUR_LOCAL_IP] with the server's IPv4 address (e.g., 192.168.1.10) and replace [PORT] with the port number your project is using (7095).

### 🔐 2. Authentication (Identity & JWT)
The system uses the JWT security protocol to secure all operations.

### [POST] /api/Auth/register
* Purpose: To register a new team member.

* Request Body (JSON):
```json
{
  "fullName": "string",
  "collegeId": "string",
  "email": "string"
}
```
* Response (200 OK):
``` 
"Team member registered!" 
```

### [POST] /api/Auth/login
* Purpose: To log in and obtain the token.

* Request Body (JSON):
```json
{
  "fullName": "string",
  "collegeId": "string"
}
```
* Response (200 OK):
  for example:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ..."
}
```
### Note:
 This token must be included in the header of all the following requests:
Authorization: Bearer <Your_Token>

### 👁️3. Robot Configuration (Camera Streaming)
### [GET] /Configuration/live-stream-url
* Purpose: To obtain the live stream link for "Robot Eyes". This is triggered when the Live button is pressed.
* Request: (No Body) 
* Response (200 OK):
  for example:
```json
{
  "streamUrl": "http://192.168.1.50:81/stream"
}
```
### **[POST] /api/Configuration/register-camera-ip**
* Request Body (JSON/Text):  `"192.168.1.50"` 
* Response (200 OK):
 ```
 { "message": "Robot camera IP registered successfully." }
 ```
### 🗄️ 4. Drawers & Patients (Data Management)

### [GET] /Drawers/status
* Purpose: To obtain the status of all medication drawers (open/closed) and from the patient associated with each drawer.
* Headers: Authorization: Bearer "token"
* Response (200 OK):** 
```json
[
 { "id": 1, "status": "Closed", "patientName": "Mohamed Khaled" },
 { "id": 2, "status": "Open", "patientName": "Sara Ahmed" } 
 ]
```


### [POST] /api/Drawers/{id}/toggle
* Purpose: To open or close a specific drawer using its ID.
* URL Parameter:** `id` (Drawer Number) * 
* Response (200 OK):
```json
 `{ "message": "Drawer 1 toggled successfully." }
 ```
 

### [POST] /api/Drawers/open-by-face/{faceId}
* Purpose: To open the drawer associated with a specific patient once their face is recognized.
* URL Parameter:** `faceId` (ID)
* Response: (200 OK) .
### 5. Patients (Patient Data)

### [GET] /api/Patients
* Purpose: To retrieve a list of all patients registered in the system
* Response: (200 OK) 
```json
 [ 
 { "id": 101, "name": "Mohamed Khaled", "age": 45,
 "gander": "Male" "assignedDrawerId": "1" },
 { "id": 102, "name": "Sara Ahmed", "age": 30,
 "gander": "Female" "assignedDrawerId": "2" }
 ]
  ```

### [GET] /api/Patients/face/{faceId}
* Purpose: To retrieve data for a specific patient using their Face ID.
* URL Parameter:** `faceId`
* Response (200 OK):
```json 
{ "id": 101, "name": "Mohamed Khaled", "faceId": "face_001", "assignedDrawerId": 1 } 
```
### **[POST] /api/Patients/vitals** 
* Request Body (JSON):
```json
{
  "faceId": 5,
  "temperature": 37.5,
  "heartRate": 80,
  "spO2": 95
}
```
* Response (200 OK):
 ```{ "status": "Vitals recorded successfully." }```
 ### 6. Records (Medical Records)
 ### [GET] /api/Records/patient/{patientId} 
 * Purpose: To obtain the medical history and previous readings of a particular patient.
 * URL Parameter:** `patientId` 
 * Response (200 OK):
 ```json 
 [ { "date": "2026-05-08T10:00:00",
	 "heartRate": 72, 
	 "temperature": 36.8 ,
	 "spo2": 95
	 },
	  { 
	  "date": "2026-05-08T22:00:00", 
	  "heartRate": 80,
	   "temperature": 37.5,
	   "spo2":92
	    }] 
```


### 🎮 7. RobotControl (Robot Control Panel)
* Purpose: This controller is responsible for sending real-time commands directly to the robot via SignalR without the need to process data in the database.
### [POST] /api/RobotControl/move
* Purpose: To send movement commands to the robot's motors (Forward, Backward, etc.).
* Request (Query String)
* direction (string): The direction of movement.
* Example request: 
  *  POST /api/RobotControl/move?direction=Forward
 * Response (200 OK):
 ```json
 {
  "message": "Direction Forward sent to robot."
}
```
 ### **[POST] /api/RobotControl/drawer**
 * Purpose: To send a direct manual command to control the drawers (Manual Override).
 * Request (Query String):
   * command (string): The code assigned to the drawer (such as O1 to open or C1 to close).
* Example request: 
  * POST /api/RobotControl/drawer?command=O1
* Response (200 OK):
```json
{ "message": "Command O1 sent to robot." }
```
### **[POST] /api/RobotControl/notify**
* Purpose: To send a text message that appears on the robot's screen or an audio alert to the patient.
* Request Body (JSON - Text):
```json 
 "Please take your medicine now."
  ```
*  Response (200 OK):
 ```json
  { "status": "Notification sent." }
 ```
 ### 💡 Technical Notes for the Mobile Team:

1. **Response Speed:** These commands are executed instantly (real-time); as soon as the request arrives at the server, it is routed to the robot in fractions of a second via the **SignalR Hub**.

2. **Use Cases:**

* Use `/move` to create movement control buttons (arrows) in the application.
* Use `/notify` to send manual alerts to the patient by the doctor or nurse.

3. **Authorization:** Don't forget to send the **Bearer Token** in the header, as this controller is fully protected by `[Authorize]` to ensure that only registered team members can control the robot.

---
