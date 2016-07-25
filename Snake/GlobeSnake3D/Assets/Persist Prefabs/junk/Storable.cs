using UnityEngine;
using System.Collections;
using System;

[Serializable]
public abstract class Storable  {
    protected string id;

    public string getId() { return id; }

    public bool compare(string id) { return this.id == id; }
}
