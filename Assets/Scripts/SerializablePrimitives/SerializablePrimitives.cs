using Newtonsoft.Json;
using UnityEngine;

public class SerializableVector3
{

    [JsonProperty]
    private float _x, _y, _z;

    [JsonIgnore]
    public Vector3 Value => new Vector3(_x, _y, _z);

    public SerializableVector3() {  }

    public SerializableVector3(float x, float y, float z)
    {
        _x = x;
        _y = y;
        _z = z;
    }

    public SerializableVector3(Vector3 toCopy) : this(toCopy.x, toCopy.y, toCopy.z) { }

}

public class SerializableQuaternion
{

    [JsonProperty]
    private float _x, _y, _z, _w;

    [JsonIgnore]
    public Quaternion Value => new Quaternion(_x, _y, _z, _w);

    public SerializableQuaternion() {  }

    public SerializableQuaternion(float x, float y, float z, float w)
    {
        _x = x;
        _y = y;
        _z = z;
        _w = w;
    }

    public SerializableQuaternion(Quaternion toCopy) : this(toCopy.x, toCopy.y, toCopy.z, toCopy.w) {  }

}

public class SerializableTransform
{

    [JsonProperty]
    private SerializableVector3 _position, _scale;

    [JsonProperty]
    private SerializableQuaternion _rotation;

    [JsonIgnore]
    public Vector3 Position => _position.Value;

    [JsonIgnore]
    public Vector3 Scale => _scale.Value;

    [JsonIgnore]
    public Quaternion Rotation => _rotation.Value;

    public SerializableTransform() { }

    public SerializableTransform(SerializableVector3 position, SerializableVector3 scale, SerializableQuaternion rotation)
    {
        _position = position;
        _scale = scale;
        _rotation = rotation;
    }

    public SerializableTransform(Transform toCopy) 
        : this(new SerializableVector3(toCopy.position), new SerializableVector3(toCopy.localScale), new SerializableQuaternion(toCopy.rotation)) { }

}
