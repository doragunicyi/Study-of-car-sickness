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

		private bool isRecordMode = false;//false;

		private readonly string RECORD_FILE = @"C:\Users\h23i03\Desktop\test.ito";

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }

		private void Update()
		{

		}
		private void Start()
		{
			if (isRecordMode) {
				logFile = new FileStream (RECORD_FILE, FileMode.Create, FileAccess.Write);
			} else {
				logFile = new FileStream (RECORD_FILE, FileMode.Open, FileAccess.Read);
				Debug.Log(logFile.Length);
			}
		}
		private void OnApplicationQuit()
		{
			logFile.Close ();
		}
        private void FixedUpdate()
        {
            // pass the input to the car!
            //float h = Input.GetAxis("Horizontal");
            //float v = Input.GetAxis("Vertical");
			//Debug.LogFormat ("{0} {1}", h, v);
            //m_Car.Move(h, v, v, 0f);

			if (isRecordMode) {
				byte up = (byte)(Input.GetKey (KeyCode.UpArrow) ? 1 : 0);
				byte down = (byte)(Input.GetKey (KeyCode.DownArrow) ? 1 << 1 : 0);
				byte right = (byte)(Input.GetKey (KeyCode.RightArrow) ? 1 << 2 : 0);
				byte left = (byte)(Input.GetKey (KeyCode.LeftArrow) ? 1 << 3 : 0);
				
				logFile.WriteByte ((byte)(up | down | right | left));
				
				float h = 0f, v = 0f;
				if(Input.GetKey(KeyCode.RightArrow)) h += 1.0f;
				if(Input.GetKey(KeyCode.LeftArrow))  h -= 1.0f;
				if(Input.GetKey(KeyCode.UpArrow))    v += 1.0f;
				if(Input.GetKey(KeyCode.DownArrow))  v -= 1.0f;
				m_Car.Move(h, v, v, 0f);
			} else {
				int data = logFile.ReadByte();
				bool up = (data & 0x1) == 0x1,
				down = (data & 0x2) == 0x2,
				right = (data & 0x4) == 0x4,
				left = (data & 0x8) == 0x8;
				
				float h = 0f, v = 0f;
				if(right) h += 1.0f;
				if(left)  h -= 1.0f;
				if(up)    v += 1.0f;
				if(down)  v -= 1.0f;
				m_Car.Move(h, v, v, 0f);
			}

        }
    }
}
