using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOPoolSystem : MonoBehaviour
{
    public bool canExpand = true;
    public int initialAmount = 10;
    public GameObject pooledPrefab;

    private List<GameObject> pooledGOs;
    private Transform pooledContainer;

    public int PooledAmount { get { return pooledGOs.Count; } }

    private void Start()
    {
        CreateContainer();
        InitiatePooledGOs();
    }

    private void CreateContainer()
    {
        pooledContainer = transform.FindChild("PooledGameObjects");
        if (pooledContainer == null)
        {
            pooledContainer = new GameObject("PooledGameObjects").transform;
            pooledContainer.parent = gameObject.transform;
        }
    }

    private void InitiatePooledGOs()
    {
        pooledGOs = new List<GameObject>();

        for (int i = 0; i < initialAmount; i++)
        {
            AddPooledGO(Instantiate(pooledPrefab) as GameObject);
        }
    }

    private void Update()
    {
        DisablePoolableGOs();
        enabled = false;
    }

    private void AddPooledGO(GameObject instance)
    {
        if (instance.GetComponent<IPoolable>() != null)
        {
            instance.transform.parent = pooledContainer;
            instance.transform.position = Vector2.zero;
            pooledGOs.Add(instance);
        }
        else
        {
            throw new UnityException("Instância " + instance.name + " não possui a interface IPoolable!");
        }
    }

    private void DisablePoolableGOs()
    {
        for (int i = 0; i < pooledGOs.Count; i++)
        {
            pooledGOs[i].SetActive(false);
        }
    }


    public GameObject Get(Vector2 position, Vector2 scale, Quaternion rotation)
    {
        for (int i = 0; i < pooledGOs.Count; i++)
        {
            if (!pooledGOs[i].activeInHierarchy)
            {
                pooledGOs[i].transform.position = position;
                pooledGOs[i].transform.localScale = scale;
                pooledGOs[i].transform.rotation = rotation;

                pooledGOs[i].GetComponent<IPoolable>().Ativate();
                pooledGOs[i].SetActive(true);
                return pooledGOs[i];
            }
        }

        if (canExpand)
        {
            AddPooledGO(Instantiate(pooledPrefab) as GameObject);
            int size = pooledGOs.Count - 1;
            pooledGOs[size].SetActive(true);

            pooledGOs[size].transform.position = position;
            pooledGOs[size].transform.localScale = scale;
            pooledGOs[size].transform.rotation = rotation;

            StartCoroutine(ActivatePoolableGO(size));
            return pooledGOs[size];
        }

        throw new UnityException("Não há GO disponíveis no momento");
    }

    public void SendBack(GameObject instance)
    {
        instance.transform.parent = pooledContainer;
        instance.transform.position = Vector2.zero;
        instance.GetComponent<IPoolable>().Deactivate();
        instance.SetActive(false);
    }

    private IEnumerator ActivatePoolableGO(int index)
    {
        yield return new WaitForFixedUpdate();
        pooledGOs[index].GetComponent<IPoolable>().Ativate();
    }
}
