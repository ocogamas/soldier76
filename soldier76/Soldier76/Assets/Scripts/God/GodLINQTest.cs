using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GodLINQTest : MonoBehaviour
{
    private class GodTestObject
    {
        public int Point;
        public List<GodTestChildObject> childList;

        public void AddPoint(int p)
        {
            Point += p;
        }

        public GodTestObject(int p)
        {
            Point = p;
        }
    }

    private class GodTestChildObject
    {
        public int ChildPoint;

        public GodTestChildObject(int p)
        {
            ChildPoint = p;
        }

        public void PrintChildPoint()
        {
            Debug.Log_yellow($"CHILD POINT IS {ChildPoint}", this);
        }
    }
    
    public void OnClickLINQTestButton()
    {
        Debug.Log_cyan($"OnClickLINQTestButton", this);
        // simpleSelect();
        // childListSelect();
        selectMany();
    }

    private void selectMany()
    {
        List<GodTestObject> dataList = createTestDataList();
        foreach (GodTestObject data in dataList)
        {
            data.childList = new List<GodTestChildObject>();
            data.childList.Add(new GodTestChildObject(100));
            data.childList.Add(new GodTestChildObject(200));
        }

        IEnumerable<GodTestChildObject> listEnumerable = dataList.SelectMany(data => data.childList);
        List<GodTestChildObject> list = listEnumerable.ToList();
        
        foreach (GodTestChildObject obj in list)
        {
            Debug.Log_cyan($"obj = {obj.ChildPoint}", this);
        }
    }

    private void simpleSelect()
    {
        List<GodTestObject> dataList = createTestDataList();

        IEnumerable<int> query = dataList.Select(data => data.Point);
        List<int> intList = query.ToList();

        foreach (int num in intList)
        {
            Debug.Log_cyan($"num = {num}", this);
        }
    }

    private void childListSelect()
    {
        List<GodTestObject> dataList = createTestDataList();
        foreach (GodTestObject data in dataList)
        {
            data.childList = new List<GodTestChildObject>();
            data.childList.Add(new GodTestChildObject(100));
            data.childList.Add(new GodTestChildObject(200));
        }
        
        IEnumerable<List<GodTestChildObject>> query = dataList.Select(data => data.childList);
        List<List<GodTestChildObject>> childObjectList = query.ToList();
        
        childObjectList.ForEach(x => x.ForEach(y => y.PrintChildPoint()));
    }

    private List<GodTestObject> createTestDataList()
    {
        List<GodTestObject> dataList = new List<GodTestObject>();

        for (int i = 0; i < 20; i++)
        {
            dataList.Add(new GodTestObject(i));
        }

        return dataList;
    }
    
    
}
