using Assets.Scripts.People;
using Assets.Scripts.Utility;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class Room : MonoBehaviour
    {
        public GameObject Entrance;

        private static readonly float TRANSITION_TIME = 0.5f;

        public void OnExit(bool instant)
        {
            Person.PoolAllPeople();

            StartCoroutine(SlideOut(instant));
        }

        private IEnumerator SlideOut(bool instant)
        {
            if (!instant)
            {
                Vector2 destination = new Vector2(0, -10);
                Vector2 startPos = transform.position;

                float elapsed = 0f;
                while (!transform.position.Equals(destination))
                {
                    yield return null;
                    elapsed += Time.deltaTime;
                    float t = Mathf.Min(1, elapsed / TRANSITION_TIME);
                    transform.position = Vector2.Lerp(startPos, destination, t);
                }
            }

            Destroy(gameObject);
        }

        public void OnEnter(bool instant, GameObject player)
        {
            if (instant)
            {
                transform.localPosition = Vector3.zero;
            }
            else
            {
                transform.localPosition = new Vector2(0, 10);
            }

            StartCoroutine(SlideIn(instant, player));
        }

        private IEnumerator SlideIn(bool instant, GameObject player)
        {
            if(!instant)
            {
                Vector2 destination = new Vector2(0, 0);
                Vector2 startPos = transform.position;

                float elapsed = 0f;
                while (!transform.position.Equals(destination))
                {
                    yield return null;
                    elapsed += Time.deltaTime;
                    float t = Mathf.Min(1, elapsed / TRANSITION_TIME);
                    transform.position = Vector2.Lerp(startPos, destination, t);
                }
            }

            player.transform.position = Entrance.transform.position;
            player.SetActive(true);
        }
    }

}
