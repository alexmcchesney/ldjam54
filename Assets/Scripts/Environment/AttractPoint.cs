using Assets.Scripts.People;
using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Assets.Scripts.Environment
{
    public class AttractPoint : MonoBehaviour
    {
        public float Radius = 1f;

        public static HashSet<AttractPoint> LiveAttractPoints = new();

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }

        public void OnEnable()
        {
            LiveAttractPoints.Add(this);
        }

        public void OnDisable()
        {
            LiveAttractPoints.Remove(this);
        }

        public bool WithinRadius(Vector2 position)
        {
            return Vector2.Distance((Vector2)transform.position, position) < Radius;
        }
    }

}
