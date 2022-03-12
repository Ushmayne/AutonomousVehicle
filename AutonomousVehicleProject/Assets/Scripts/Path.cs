using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    private List<Transform> nodes = new List<Transform>();
    public Color lineColor;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = lineColor;

        Transform[] pathTransform = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for(int i = 0; i< pathTransform.Length; i++)
        {//gets all nodes
            if(pathTransform[i] != transform)
            {//new nodes, add to list
                nodes.Add(pathTransform[i]);
            }
        }

        for(int i = 0; i < nodes.Count; i++)
        {
            Vector3 currentNode = nodes[i].position;
            Vector3 previousNode = Vector3.zero;

            if (i>0)
            {//not first node
                previousNode = nodes[i - 1].position;
            }
            else if(i == 0 && nodes.Count > 1)
            {//firs node and there is more than one node
                previousNode = nodes[nodes.Count - 1].position;//connect first with the last
            }

            Gizmos.DrawLine(previousNode, currentNode);
            Gizmos.DrawWireSphere(currentNode, 2f);
        }
    }
}
