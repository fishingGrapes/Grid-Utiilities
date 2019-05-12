using UnityEngine;
using VH.Pathfinding;
using VH.RangeDetection;

namespace VH
{
    [ExecuteAfter(typeof(TilemapInitializer))]
    public class GridDebugger : ComponentBehaviour
    {
        [SerializeField]
        private Tilemap tilemap = null;

        [SerializeField, Range(5, 120)]
        private ushort range = 15;

        private NavigationAgent agent;
        private Camera mainCamera;

        private Vector3 boundSize;
        private Vector3 mousePosition;

        private Tile myTile;
        private Intersection myIntersection, currentIntersection;

        protected override void Awake()
        {
            base.Awake();

            agent = new NavigationAgent();
            agent.width = agent.height = 1;
            agent.range = 30;
            agent.penaltyModifier = 1;

            mainCamera = Camera.main;
            boundSize = new Vector3();

        }

        private void Start()
        {
            myTile = tilemap.GetTileFromWorldPosition(transform.position);
            myIntersection = tilemap.GetNearestIntersection(myTile.position, Direction.NorthWest);
        }

        public override void Tick()
        {
            base.Tick();
            mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonUp(0))
            {

                //Debug.Log($"{myIntersection.GetDodecant(tilemap, mousePosition)}");


                GridManager.RequestCircleRange(tilemap, new CircleRangeRequest(mousePosition, range, null));

                return;

                GridManager.RequestConeRange(tilemap, new ConeRangeRequest(myTile.position, mousePosition, range, null));

                GridManager.RequestPath(tilemap,
           new PathRequest(agent, transform.position, mousePosition, null));

                GridManager.RequestConeRange(tilemap, new ConeRangeRequest(myTile.position, mousePosition, range, null));


                GridManager.RequestMovementRange(tilemap,
                    new MovementRangeRequest(transform.position, agent, null));


                GridManager.RequestLineRange(tilemap,
         new LineRangeRequest(transform.position, mousePosition, range, null));



                Logger.Log("Requested Path");

            }

        }

        private void OnGUI()
        {
            //Tile tile = tilemap.GetTileFromWorldPosition(mousePosition);
            //Vector3 circlePosition = tile.position;
            //circlePosition.z = -5f;
            //DebugExtension.DebugCircle(circlePosition, Vector3.forward, Color.green, tilemap.TileSize * 0.5f);

        }


        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            boundSize.Set(tilemap.Columns, tilemap.Rows, 1);
            Gizmos.DrawWireCube(Vector3.zero, boundSize);

            return;
            //foreach (Intersection intersection in tilemap.Intersections)
            //{
            //    if (RangeDetectionUtility.ConeIntersections != null && RangeDetectionUtility.ConeIntersections.Contains(intersection))
            //        DebugExtension.DrawPoint(intersection.position, Color.blue, 1);
            //    else
            //        DebugExtension.DrawPoint(intersection.position, Color.black, 1);
            //}



            foreach (var tile in tilemap.Tiles)
            {
                Gizmos.color = tile.walkable ? Color.white : Color.red;

                //if (RangeDetectionUtility.MovementRange != null)
                //{
                //    if (RangeDetectionUtility.MovementRange.Contains(tile))
                //        Gizmos.color = Color.green;
                //}

                if (RangeDetectionUtility.CircleRange != null)
                {
                    if (RangeDetectionUtility.CircleRange.Contains(tile))
                    {
                        Gizmos.color = Color.green;
                    }
                }

                if (RangeDetectionUtility.ConeRange != null)
                {
                    if (RangeDetectionUtility.ConeRange.Contains(tile))
                    {
                        Gizmos.color = Color.cyan;
                    }
                }

                if (PathfindingUtility.Path != null)
                {
                    if (PathfindingUtility.Path.Contains(tile))
                        Gizmos.color = Color.black;
                }

                //if (RangeDetectionUtility.LineRange != null)
                //{
                //    if (RangeDetectionUtility.LineRange.Contains(tile))
                //    {
                //        Gizmos.color = Color.cyan;
                //    }
                //}



                if (tile == myTile)
                {
                    Gizmos.color = Color.blue;
                    // DebugExtension.DrawPoint(tilemap.GetNearestIntersection(tile.position).position, Color.green, 1);
                }


                Gizmos.DrawCube(tile.position, Vector3.one * 0.95f);


                //if (tile == myTile)
                //    DebugExtension.DrawPoint(tile.position, Color.blue, 1);

            }
        }
    }
}
