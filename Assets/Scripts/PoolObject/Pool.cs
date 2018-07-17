using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool<T>
{
    public List<PoolObject<T>> poolList;
    public delegate T CallbackFactory();
    private int _count;
    private bool _isDinamic = false;
    private PoolObject<T>.PoolCallback _init;
    private PoolObject<T>.PoolCallback _finalize;
    private CallbackFactory _factoryMethod;

    public Pool(int initialStock, CallbackFactory factoryMethod, PoolObject<T>.PoolCallback initialize, PoolObject<T>.PoolCallback finalize, bool isDinamic)
    {
        poolList = new List<PoolObject<T>>();

        //Guardamos los punteros para cuando los necesitemos.
        _factoryMethod = factoryMethod;
        _isDinamic = isDinamic;
        _count = initialStock;
        _init = initialize;
        _finalize = finalize;

        //Generamos el stock inicial.
        for (int i = 0; i < _count; i++)
        {
            poolList.Add(new PoolObject<T>(_factoryMethod(), _init, _finalize));
        }
    }

    public PoolObject<T> GetPoolObject()
    {
        for (int i = 0; i < _count; i++)
        {
            if (!poolList[i].isActive)
            {
                poolList[i].isActive = true;
                return poolList[i];
            }
        }
        if (_isDinamic)
        {
            PoolObject<T> po = new PoolObject<T>(_factoryMethod(), _init, _finalize);
            po.isActive = true;
            poolList.Add(po);
            _count++;
            return po;
        }
        return null;
    }

    public T GetObjectFromPool()
    {
        for (int i = 0; i < _count; i++)
        {
            if (!poolList[i].isActive)
            {
                poolList[i].isActive = true;
                return poolList[i].GetObj;
            }
        }

        if (_isDinamic)
        {
            PoolObject<T> po = new PoolObject<T>(_factoryMethod(), _init, _finalize);
            po.isActive = true;
            poolList.Add(po);
            _count++;
            return po.GetObj;
        }
        return default(T);
    }

    public void DisablePoolObject(T obj)
    {
        foreach (PoolObject<T> poolObj in poolList)
        {
            if (poolObj.GetObj.Equals(obj))
            {
                poolObj.isActive = false;
                return;
            }
        }
    }
}
