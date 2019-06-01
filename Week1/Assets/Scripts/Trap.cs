using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{

    public enum MoveType
    {
        Sequence,
        Random
    }

    [SerializeField] Transform[] petropoints;
    [SerializeField] MoveType movetype;
    public float speed = 5;
    int index = 0;

    void Start()
    {
        index = Random.Range(0, petropoints.Length);
        transform.position = petropoints[index].position;
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);

        if (Vector3.Distance(transform.position, petropoints[index].position) < 0.1f )
        {
            if(movetype == MoveType.Sequence)
            {
                index = (index + 1) % petropoints.Length;
            }
            else if(movetype == MoveType.Random)
            {
                index = Random.Range(0, petropoints.Length);
            }
            
        }

        transform.position = Vector3.MoveTowards(transform.position, petropoints[index].position, speed * Time.deltaTime);
    }
}
