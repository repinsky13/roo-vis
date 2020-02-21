using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JSONDataClass 
{
    public List<Pairing> pairings;
}

[Serializable]
public class Pairing
{
    public double distance;
    public Order order;
    public Restaurant restaurant;
    public Rider rider;
    public int uid;
}

[Serializable]
public class Order
{
    public List<string> description;
    public int id;
}

[Serializable] 
public class Location
{
    public double lat;
    public double @long;
}

[Serializable] 
public class Restaurant
{
    public int id;
    public Location location;
}

[Serializable]
public class Rider
{
    public int id;
    public Location location;
    public string name;
}