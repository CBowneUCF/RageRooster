﻿using EditorAttributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldChange : ScriptableObject, ICustomSerialized
{
    [SerializeField, DisableInEditMode, DisableInPlayMode] private bool _enabled;

    public bool Enabled => _enabled;
    [JsonIgnore]
    public System.Action Action;

    [JsonIgnore]
    public bool defaultValue;

    public void Activate()
    {
        Action?.Invoke();
        _enabled = true;
    }
    public void Deactivate() => _enabled = false;

    public JToken Serialize()
    {
        return new JObject
            (new JProperty("Enabled", _enabled)
            );
    }

    public void Deserialize(JToken Data)
    {
        _enabled = Data["Enabled"].Value<bool>();
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
