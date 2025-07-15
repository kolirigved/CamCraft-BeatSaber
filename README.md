# CamCraft- Camera Controlled Beatsaber

This project bridges **Python and Unity** using **sockets and a local UDP server** to create a real-time interactive game controlled by hand gestures. It showcases a powerful low latency interface where **MediaPipe** detects hand landmarks in Python, computes angles in the camera plane, and transmits this data to Unity—where it's used to control in-game swords.
<br />
Built as a demonstration of cross-platform integration and real-time hand tracking, this project also lays the foundation for incorporating machine learning tasks from python into Unity.

---

## Demo

![Demo gif](https://imgur.com/R1CXxgD.gif)
<br />
*Demostration of the Camera Controlled game.*

---

## Features

- **Real-Time Hand Tracking:**  
  Utilizes **MediaPipe Hands** to detect 21 hand landmarks per frame.

- **Python ↔ Unity Communication:**  
  Establishes a **UDP-based local server** using Python's `socket` library for low-latency transmission.

- **Unity Animation Control:**  
  Unity receives the angle data and uses it to control a sword-wielding character's arm and weapon via animation rigging.

- **Live Camera Feed Transmission:**  
  Encodes and sends **camera frames from Python to Unity** so the player can see themselves rendered in the Unity game view. This helps to build a complete game along with video feed.


---

## Tech Stack

| Component         | Utility                         |
|------------------|-------------------------------------|
| Hand Tracking     | Mediapipe                           |
| Socket Server     | Python `socket` (UDP)               |
| Visualization/Game| Unity (C#)                          |
| Integration       | Python ↔ Unity UDP bridge           |

---

