using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(0, -8f, 0);
    [SerializeField] float peroid = 2f;

    float movementFactor;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (peroid <= Mathf.Epsilon) return;
        float cycles = Time.time / peroid;

        const float tau = Mathf.PI * 2f;
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
    }
}
