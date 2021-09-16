using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject tile;

    private void Start()
    {
        Instantiate(tile, transform.position, Quaternion.identity);
    }
}
