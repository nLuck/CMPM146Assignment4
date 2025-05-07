using System.Collections.Generic;

public class Blackboard
{
    private Dictionary<string, object> storage = new Dictionary<string, object>();

    public void Set<T>(string key, T value)
    {
        storage[key] = value;
    }

    public T Get<T>(string key)
    {
        if (storage.TryGetValue(key, out object value) && value is T typeValue) {
            return typeValue;
        }
        return default(T);
    }

    public bool Has(string key)
    {
        return storage.ContainsKey(key);
    }
}