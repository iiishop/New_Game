using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    //单例模式

    public GameObject ShadowPrefab;
    public int ShadowPoolSize = 10;
    private Queue<GameObject> Available_Objects = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            FillPool();
        }
    }

    public void FillPool()
    {
        for (int i = 0; i < ShadowPoolSize; i++)
        {
            var New_Shadow = Instantiate(ShadowPrefab);
            New_Shadow.transform.SetParent(this.transform);

            ReturnPool(New_Shadow);
        }
    }

    public void ReturnPool(GameObject obj)
    {
        obj.SetActive(false);
        Available_Objects.Enqueue(obj);
    }

    public GameObject GetPool()
    {
        if (Available_Objects.Count == 0)
        {
            var New_Shadow = Instantiate(ShadowPrefab);
            New_Shadow.transform.SetParent(this.transform);

            ReturnPool(New_Shadow);
        }

        var obj = Available_Objects.Dequeue();
        obj.SetActive(true);
        return obj;
    }
}