using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform platesVisualPrefab;
    [SerializeField] private Transform counterTopPoint;

    private List<GameObject> platesVisualList;

    private void Awake()
    {
        platesVisualList = new List<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlatesSpawned += PlatesCounter_OnPlatesSpawned;
        platesCounter.OnPlatesRemoved += PlatesCounter_OnPlatesRemoved;
    }

    private void PlatesCounter_OnPlatesRemoved(object sender, System.EventArgs e)
    {
        GameObject topPlate = platesVisualList[^1];
        platesVisualList.Remove(topPlate);
        Destroy(topPlate);
    }

    private void PlatesCounter_OnPlatesSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(platesVisualPrefab, counterTopPoint);


        float yOffset = .1f;
        yOffset *= platesVisualList.Count;
        plateVisualTransform.localPosition = new Vector3(0, yOffset, 0);
        platesVisualList.Add(plateVisualTransform.gameObject);
    }
}
