using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Text;
using System.IO;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
		private FileStream logFile;

		private bool isRecordMode = false;

		private readonly string RECORD_FILE = @"C:\Users\h23i03\Desktop\test.ito";

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }

		private void Start()
		{
			if (isRecordMode) {
				logFile = new FileStream (RECORD_FILE, FileMode.Create, FileAccess.Write);
			} else {
				logFile = new FileStream (RECORD_FILE, FileMode.Open, FileAccess.Read);
			}
		}

		private void OnApplicationQuit()
		{
			logFile.Close ();
		}

		private Vector2 getVelocity(bool up, bool down, bool right, bool left) {
			var ret = new Vector2 (0f, 0f);
			if (right)
				ret.x += 1.0f;
			if (left)
				ret.x -= 1.0f;
			if (up)
				ret.y += 1.0f;
			if (down)
				ret.y -= 1.0f;
			return ret;
		}

        private void FixedUpdate()
        {
			if (isRecordMode) {
				bool up    = Input.GetKey (KeyCode.UpArrow);
				bool down  = Input.GetKey (KeyCode.DownArrow);
				bool right = Input.GetKey (KeyCode.RightArrow);
				bool left  = Input.GetKey (KeyCode.LeftArrow);
				
				logFile.WriteByte ((byte)((up ? 0x1 : 0x0) | 
				                          (down ? 0x2 : 0x0) | 
				                          (right ? 0x4 : 0x0) | 
				                          (left ? 0x8 : 0x0)));

				var v = getVelocity(up, down, right, left);
				m_Car.Move(v.x, v.y, v.y, 0f);

			} else {
				int data = logFile.ReadByte();

				bool up    = (data & 0x1) != 0;
				bool down  = (data & 0x2) != 0;
				bool right = (data & 0x4) != 0;
				bool left  = (data & 0x8) != 0;
				
				var v = getVelocity(up, down, right, left);
				m_Car.Move(v.x, v.y, v.y, 0f);
			}

        }
    }
}
