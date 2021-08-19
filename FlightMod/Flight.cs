using MelonLoader;
using System;
using UnityEngine;

namespace FlightMod
{
    public class Flight : MelonMod
    {
        public override void OnUpdate()
        {
            if (!PlayerExtensions.IsInWorld() && flying)
            {
                PlayerExtensions.LocalPlayer.gameObject.GetComponent<CharacterController>().enabled = true;
                flying = false;
            }
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                if (PlayerExtensions.IsInWorld() && PlayerExtensions.LocalPlayer != null)
                {
                    if (flying)
                    {
                        PlayerExtensions.LocalPlayer.gameObject.GetComponent<CharacterController>().enabled = true;
                        flying = false;
                    }
                    else
                    {
                        PlayerExtensions.LocalPlayer.gameObject.GetComponent<CharacterController>().enabled = false;
                        player = PlayerExtensions.LocalPlayer.gameObject;
                        flying = true;
                    }
                }
            }

            if (flying)
            {
                float number = Input.GetKey(KeyCode.LeftShift) ? (flySpeed * 2f) : flySpeed;
                if (Input.mouseScrollDelta.y != 0)
                {
                    flySpeed += (int)Input.mouseScrollDelta.y;

                    if (flySpeed <= 0)
                        flySpeed = 1;
                }

                if (PlayerExtensions.LocalPlayer.IsInVR())
                {
                    if (Math.Abs(Input.GetAxis("Vertical")) != 0f)
                        player.transform.position += GetPlayerCamera.transform.forward * (number * Time.deltaTime * Input.GetAxis("Vertical"));

                    if (Math.Abs(Input.GetAxis("Horizontal")) != 0f)
                        player.transform.position += GetPlayerCamera.transform.right * (number * Time.deltaTime * Input.GetAxis("Horizontal"));
                    if (Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") < 0f)
                        player.transform.position += GetPlayerCamera.transform.up * (number * Time.deltaTime * Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryThumbstickVertical"));
                    if (Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") > 0f)
                        player.transform.position += GetPlayerCamera.transform.up * (number * Time.deltaTime * Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryThumbstickVertical"));
                }

                for (int i = 0; i < keys.Length; i++) switch (Input.GetKey(keys[i]))
                    {
                        case true when (i == 0):
                            player.transform.position += GetPlayerCamera.transform.forward * number * Time.deltaTime;
                            break;

                        case true when (i == 1):
                            player.transform.position -= GetPlayerCamera.transform.right * number * Time.deltaTime;
                            break;

                        case true when (i == 2):
                            player.transform.position -= GetPlayerCamera.transform.forward * number * Time.deltaTime;
                            break;

                        case true when (i == 3):
                            player.transform.position += GetPlayerCamera.transform.right * number * Time.deltaTime;
                            break;

                        case true when (i == 3):
                            player.transform.position += Vector3.up * number * Time.deltaTime;
                            break;

                        case true when (i == 5):
                            player.transform.position -= Vector3.up * number * Time.deltaTime;
                            break;
                    }
            }
        }

        private KeyCode[] keys = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.E, KeyCode.Q };

        public static GameObject GetPlayerCamera
        {
            get
            {
                if (CachedPlayerCamera == null)
                {
                    CachedPlayerCamera = GameObject.Find("Camera (eye)");
                }
                return CachedPlayerCamera;
            }
        }

        private static GameObject CachedPlayerCamera;
        public static float flySpeed = 10;
        public static bool flying = false;
        private GameObject player;
    }
}