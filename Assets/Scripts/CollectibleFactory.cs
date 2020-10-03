using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleFactory : MonoBehaviour, IFactory
{

    public GameObject smallCollectible;

    public GameObject largeCollectible;

    public IProduct produce()
    {
        float randomNum = Random.Range(0f, 1f);
        if (randomNum <= 0.9)
        {
            return (TorusMover)smallCollectible.GetComponent<CollectibleSmall>();
        }
        else
        {
            return (TorusMover)largeCollectible.GetComponent<CollectibleBig>();
        }

    }

    public IProduct produceManual(int type)
    {
        if (type == 1)
        {
            return (TorusMover)smallCollectible.GetComponent<CollectibleSmall>();
        }
        else
        {
            return (TorusMover)largeCollectible.GetComponent<CollectibleBig>();
        }
    }
}
