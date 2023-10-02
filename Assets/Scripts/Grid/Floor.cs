using UnityEngine;

public class Floor : MonoBehaviour 
{
    [SerializeField] private GameObject[] floorModels;
    // [SerializeField] private string seed = ;
    
    public void Randomize()
    {
        foreach (var floorModel in floorModels)
        {
            floorModel.SetActive(false);
        }
        
        // Random.Range()
    }
}