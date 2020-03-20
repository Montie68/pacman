using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class palletCounter : MonoBehaviour
{
    public static palletCounter instance;
    public PalletEatenEvent palletEaten = new PalletEatenEvent();
    public int palletCount;
    List<AddPoints> pallets = new List<AddPoints>();
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else instance = this;
    }
    void Start()
    {
        
        foreach(AddPoints ap in GetComponentsInChildren<AddPoints>())
        {
            pallets.Add(ap);
        }
        palletCount = pallets.Count;
        palletEaten.AddListener(OnPalletEaten);
    }

    // Update is called once per frame
    public void OnPalletEaten(AddPoints pallet)
    {
        pallets.Remove(pallet);
        palletCount = pallets.Count;

    }
}

