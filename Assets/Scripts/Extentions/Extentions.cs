using UnityEngine;
public static class Extentions
{
    public static Vector3 Change(this Vector3 org, object x = null, object y = null, object z = null)
    {
        return new Vector3(x == null ? org.x : (float)x, y == null ? org.y : (float)y, z == null ? org.z : (float)z);
    }

    public static Vector2 Change(this Vector2 org, object x = null, object y = null)
    {
        return new Vector2(x == null ? org.x : (float)x, y == null ? org.y : (float)y);
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        var wantedComponent = gameObject.GetComponent<T>();
        if (wantedComponent == false)
            wantedComponent = gameObject.AddComponent<T>();
        return wantedComponent;
    }

    public static T GetOrAddComponent<T>(this Component component) where T : Component
    {
        var wantedComponent = component.gameObject.GetComponent<T>();
        if (wantedComponent == false)
            wantedComponent = component.gameObject.AddComponent<T>();
        return wantedComponent;
    }
}
