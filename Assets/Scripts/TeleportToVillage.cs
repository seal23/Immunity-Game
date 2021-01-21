using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    class TeleportToVillage : MonoBehaviour, ITeleport
    {
        // Start is called before the first frame update
        public string PositionName = "";
        public string DestinationSceneName = LevelManager.VillageSceneName;
        public string TeleportGameObjName = "Gate01";

        public bool IsEnable = true;

        void Start()
        {
            //if (!listTeleports.ContainsKey(PositionName))
            //{
            //    listTeleports.Add(PositionName, this);
            //    Debug.Log("start");

            //    Debug.Log(PositionName);
            //}
        }

        private void Awake()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (IsEnable)
            {
                if (triggerStay)
                {
                    if (Input.GetKeyDown(KeyCode.X))
                        isEnter = true;
                }
            }
        }

        private bool isEnter = false;
        private bool triggerStay = false;

        void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Entered");
            if (collision.gameObject.CompareTag("Player"))
            {
                triggerStay = true;
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            Debug.Log("Exited");
            if (collision.gameObject.CompareTag("Player"))
            {
                triggerStay = false;
                isEnter = false;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {

            if (isEnter && collision.GetComponent<PlayerController>() != null)
            {
                playerGameObj = collision.gameObject;
                var parameters = new LoadSceneParameters(LoadSceneMode.Single);
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene(DestinationSceneName);
            }

        }

        private GameObject playerGameObj;
        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            var listObj = arg0.GetRootGameObjects();
            Debug.Log(listObj.Length);

            foreach (var gameObj in listObj)
            {

                //if (gameObj.name.Equals(TeleportGameObjName))
                //{
                //    player.transform.position = gameObj.transform.position;
                //}
                var tele = gameObj.GetComponent<Teleport>();
                if (tele != null && tele.PositionName.Equals(TeleportGameObjName))
                {
                    Debug.Log(tele.PositionName);
                    if (playerGameObj != null)
                    {
                        var rigidbody2d = playerGameObj.GetComponent<Rigidbody2D>();
                        rigidbody2d.MovePosition(gameObj.transform.position);
                        Debug.Log("tele: " + gameObj.transform.position);
                        Debug.Log("player: " + rigidbody2d.transform.position);
                    }

                    break;
                }
            }
        }

        public string GetPostionName()
        {
            return PositionName;
        }
    }
}
