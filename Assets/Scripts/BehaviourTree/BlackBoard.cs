using UnityEngine;

[System.Serializable]
public struct BlackBoardData<T>
{
    public string key;
    public T value;
}

[System.Serializable]
public class BlackBoard : ScriptableObject
{
    public BlackBoardData<int>[] intData;
    public BlackBoardData<float>[] floatData;
    public BlackBoardData<bool>[] boolData;
    public BlackBoardData<string>[] stringData;
    public BlackBoardData<Vector3>[] vector3Data;
}
