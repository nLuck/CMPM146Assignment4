using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class AIWaypointManager
{
    private List<AIWaypoint> waypoints;

    private static AIWaypointManager theInstance;
    public static AIWaypointManager Instance
    {
        get
        {
            if (theInstance == null)
                theInstance = new AIWaypointManager();
            return theInstance;
        }
    }

    private AIWaypointManager()
    {
        waypoints = new List<AIWaypoint>();
    }

    public void AddWaypoint(AIWaypoint wp)
    {
        waypoints.Add(wp);
    }

    public AIWaypoint GetClosest(Vector3 point)
    {
        return waypoints.Aggregate((a, b) => (a.position - point).sqrMagnitude < (b.position - point).sqrMagnitude ? a : b);
    }

    public AIWaypoint GetClosestByType(Vector3 point, AIWaypoint.Type type)
    {
        var of_type = waypoints.FindAll((a) => a.type == type);
        if (of_type.Count == 0) return null;
        return of_type.Aggregate((a, b) => (a.position - point).sqrMagnitude < (b.position - point).sqrMagnitude ? a : b);
    }

    public AIWaypoint Get(int i)
    {
        if (waypoints.Count <= i) return null;
        return waypoints[i];
    }

    public AIWaypoint GetFurthest(Vector3 point)
    {
        if (waypoints.Count == 0) return null;
        return waypoints.Aggregate((a, b) => (a.position - point).sqrMagnitude > (b.position - point).sqrMagnitude ? a : b);
    }

    public Transform GetOffsetWaypoint(Transform waypoint, float offset)
    {
        if (waypoint == null) return null;
        Vector3 offsetPos = waypoint.position + new Vector3(offset, 0, 0);
        GameObject offsetObj = new GameObject("OffsetWaypoint");
        offsetObj.transform.position = offsetPos;
        return offsetObj.transform;
    }

    public List<AIWaypoint> GetAllSafeWaypoints()
    {
        return waypoints.FindAll(wp => wp.type == AIWaypoint.Type.SAFE);
    }
}
