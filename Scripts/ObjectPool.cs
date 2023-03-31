using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    //单例模式
    
    public int Pool_Size = 10;

    public List<GameObject> Prefabs = new List<GameObject>();

    private Dictionary<string,Queue<GameObject>> Available_Objects_Dictionary = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            FillPool(Pool_Size, Prefabs);
        }
    }

    /// <summary>
    /// 这个函数用来填充对象池，对象池是一个队列，用来存储可用的对象
    /// </summary>
    private void FillPool(int Size, List<GameObject> Prefabs)
    {
        for (int i = 0; i < Size; i++)
        {
            foreach (var Prefab in Prefabs)
            {
                var obj = Instantiate(Prefab, this.transform, true);
                ReturnPool(obj);
            }
        }
    }
    /// <summary>
    /// 这个函数用来将对象放回对象池，放回对象池的对象会被设置为不可用
    /// </summary>
    /// <param name="obj">将被放回对象池的对象</param>
    public void ReturnPool(GameObject Object)
    {
        Object.SetActive(false);

        //这里需要根据对象的类型来放入不同的队列中
        if (Available_Objects_Dictionary.ContainsKey(Object.name))
        {
            Available_Objects_Dictionary[Object.name].Enqueue(Object);
        }
        else
        {
            Available_Objects_Dictionary.Add(Object.name,new Queue<GameObject>());
            Available_Objects_Dictionary[Object.name].Enqueue(Object);
        }
    }
    
    /// <summary>
    /// 这个函数用来从对象池中获取一个对象，如果对象池中没有可用的对象，就会创建一个新的对象
    /// </summary>
    /// <returns>获取的对象</returns>
    public GameObject GetPool(GameObject Object)
    {
        var NAME = Object.name + "(Clone)";
        if (Available_Objects_Dictionary[NAME].Count == 0)
        {
            FillPool(1, new List<GameObject>(){Object});
        }

        var obj = Available_Objects_Dictionary[NAME].Dequeue();
        obj.SetActive(true);
        return obj;
    }
}