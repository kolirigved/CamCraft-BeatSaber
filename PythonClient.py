import cv2
import mediapipe as mp
import numpy as np
import socket
import sys
import struct

# UDP Configurations
VIDEO_PORT = 5005
DATA_PORT = 25001
HOST = "127.0.0.1"

video_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
data_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

# Hand Tracking Setup
mp_hands = mp.solutions.hands
hands = mp_hands.Hands(static_image_mode=False, max_num_hands=2, min_detection_confidence=0.5, min_tracking_confidence=0.5)

cap = cv2.VideoCapture(0)
print("READY")
sys.stdout.flush()

def send_data(data):
    try:
        data_socket.sendto(data.encode("utf-8"), (HOST, DATA_PORT))
    except Exception as e:
        print(f"Error sending data: {e}")

def send_frame(frame):
    try:
        _, buffer = cv2.imencode('.jpg', frame)
        data = buffer.tobytes()
        size = len(data)
        MAX_SIZE = 60000

        # Send size first
        video_socket.sendto(struct.pack(">L", size), (HOST, VIDEO_PORT))

        # Send data in chunks
        for i in range(0, size, MAX_SIZE):
            chunk = data[i:i+MAX_SIZE]
            video_socket.sendto(chunk, (HOST, VIDEO_PORT))

    except Exception as e:
        print(f"Error sending frame: {e}")

def get_angle(a, b):
    return np.degrees(np.arctan2(-b.y + a.y, b.x - a.x))

while cap.isOpened():
    success, frame = cap.read()
    if not success:
        continue

    frame = cv2.resize(frame, (320, 240))
    # frame = cv2.flip(frame, 1)
    frame_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    # frame_rgb = cv2.flip(frame_rgb, 1)

    results = hands.process(frame_rgb)
    if results.multi_hand_landmarks:
        data_list = []
        for i, hand_landmarks in enumerate(results.multi_hand_landmarks):
            handedness_label = results.multi_handedness[i].classification[0].label
            index_mcp = hand_landmarks.landmark[mp_hands.HandLandmark.INDEX_FINGER_MCP]
            pinky_mcp = hand_landmarks.landmark[mp_hands.HandLandmark.PINKY_MCP]

            handPos_x = (index_mcp.x + pinky_mcp.x-1) / 2
            handPos_y = (index_mcp.y + pinky_mcp.y-1) / 2
            angle = get_angle(index_mcp, pinky_mcp)

            data_list.extend([handedness_label, handPos_x, -handPos_y, angle])

        if len(data_list) < 8:
            if data_list[0] == "Left":
                data_list.extend(["Right", 0, 0, 0])
            else:
                data_list.extend(["Left", 0, 0, 0])

        data = " ".join(map(str, data_list))
        send_data(data)

    send_frame(cv2.resize(frame, (160, 120)))
    
    if cv2.waitKey(5) & 0xFF == ord('q'):
        break

cap.release()
cv2.destroyAllWindows()
video_socket.close()
data_socket.close()
