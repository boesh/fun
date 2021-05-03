using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts
{
    public class PortalAnim : MonoBehaviour
    {
        Quaternion quaternion;

        private static PortalAnim instance;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * 10);
        }
    }
}
