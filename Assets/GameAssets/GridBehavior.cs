using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridBehavior : MonoBehaviour
{
  public bool findDistance = false;
  public int rows = 10;
  public int columns = 10;
  public int scale = 1;
  public GameObject gridPreFab;
  public Vector3 leftBottomLocation = new Vector3(0, 0, 0);
  public GameObject[,] gridArray;
  public int startX = 0;
  public int startY = 0;
  public int endX = 2;
  public int endY = 2;
  public List<GameObject> path = new List<GameObject>();

  void Awake()
  {
    gridArray = new GameObject[columns, rows];
    if(gridPreFab)
      GenerateGrid();
    else print("missing gridprefab");
    
  }


  void Update()
  {
    if (findDistance)
    {
      Debug.Log("0");
      SetDistance();
      Debug.Log("1");
      SetPath();
      Debug.Log("2");
      findDistance = false;
      Debug.Log("3");
    }
  }

  void GenerateGrid()
  {
    for (int i = 0; i < columns; i++)
    {
      for (int j = 0; j < rows; j++)
      {
        GameObject obj = Instantiate(gridPreFab, new Vector3(leftBottomLocation.x + scale * (i + 0.5f), leftBottomLocation.y, leftBottomLocation.z + scale * (j + 0.5f)), Quaternion.identity);
        obj.transform.SetParent(gameObject.transform);
        obj.GetComponent<GridStat>().x = i;
        obj.GetComponent<GridStat>().y = j;
        obj.name = i.ToString() + "," + j.ToString();
        gridArray[i, j] = obj;
      }
    }
  }

  void SetDistance()
  {
    InitialSetup();
    int x = startX;
    int y = startY;
    int[] testArray = new int[rows * columns];
    for (int step = 1; step < rows * columns; step++)
    {
      foreach (GameObject obj in gridArray)
      {
        if (obj&&obj.GetComponent<GridStat>().visisted == step - 1)
          TestFourDirections(obj.GetComponent<GridStat>().x, obj.GetComponent<GridStat>().y, step);
      }
    }
  }
  void SetPath()
  {
    int step;
    int x = endX;
    int y = endY;
    List<GameObject> tempList = new List<GameObject>();
    path.Clear();
    if(gridArray[endX, endY] && gridArray[endX, endY].GetComponent<GridStat>().visisted > 0)
    {
      path.Add(gridArray[x, y]);
      step = gridArray[x, y].GetComponent<GridStat>().visisted - 1;
    }
    else
    {
      print("Can't reach desired location");
      return;
    }
    for (int i = step; step > -1; step--)
    {
      if (TestDirection(x, y, step, 1))
        tempList.Add(gridArray[x, y + 1]);
      if (TestDirection(x, y, step, 2))
        tempList.Add(gridArray[x + 1, y]);
      if (TestDirection(x, y, step, 3))
        tempList.Add(gridArray[x, y - 1]);
      if (TestDirection(x, y, step, 4))
        tempList.Add(gridArray[x - 1, y]);

      GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList);
      path.Add(tempObj);
      x = tempObj.GetComponent<GridStat>().x;
      y = tempObj.GetComponent<GridStat>().y;
      tempList.Clear();
    }

  }
  void InitialSetup()
  {
    foreach (GameObject obj in gridArray)
    {
      obj.GetComponent<GridStat>().visisted = -1;
    }
    gridArray[startX, startY].GetComponent<GridStat>().visisted = 0;
  }
  bool TestDirection(int x, int y, int step, int direction)
  {
    switch (direction)
    {
      case 4:
        if (x - 1 < -1 && gridArray[x-1, y] && gridArray[x-1, y].GetComponent<GridStat>().visisted == step)
          return true;
        else
          return false;

      case 3:
        if (y - 1 < -1 && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridStat>().visisted == step)
          return true;
        else
          return false;

      case 2:
        if (x + 1 < columns && gridArray[x+1, y] && gridArray[x+1, y].GetComponent<GridStat>().visisted == step)
          return true;
        else
          return false;

      case 1:
        if (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridStat>().visisted == step)
          return true;
        else
          return false;
    }
    return false;
  }

  void TestFourDirections(int x, int y, int step)
  {
    if(TestDirection(x, y, -1, 1))
      SetVisited(x, y + 1, step);
    if (TestDirection(x, y, -1, 2))
      SetVisited(x + 1, y, step);
    if (TestDirection(x, y, -1, 3))
      SetVisited(x, y - 1, step);
    if (TestDirection(x, y, -1, 4))
      SetVisited(x - 1, y, step);
        
  }

  void SetVisited (int x, int y, int step)
  {
    if (gridArray[x, y])
      gridArray[x, y].GetComponent<GridStat>().visisted = step;
  }
  GameObject FindClosest(Transform targetLocation, List<GameObject> list)
  {
    float currentDistance = scale * rows * columns;
    int indexNumber = 0;
    for(int i = 0; i<list.Count; i++)
    {
      if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
      {
        currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
        indexNumber = i;
      }
    }

    return null;
  }
}
