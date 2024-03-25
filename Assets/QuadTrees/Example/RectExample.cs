using QuadTrees;
using QuadTrees.QTreeRectF;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

[ExecuteInEditMode]
public class RectExample : MonoBehaviour
{
    class QTreeObject : IRectFQuadStorable
    {
        public RectangleF Rect { get => _rect; }
        private RectangleF _rect;

        private Bounds _bounds;

        public int id;

        public QTreeObject(RectangleF rect, int id)
        {
            _rect = rect;
            this.id = id;
        }
    }

    [Header("single unit width")]
    public float width = 10;

    [Header("single unit height")]
    public float height = 10;

    [Header("quad tree center position of x axis")]
    public float centerX;

    [Header("quad tree center position of z axis")]
    public float centerZ;

    [Header("unit count in direction of x axis")]
    public int xCount = 10;

    [Header("unit count in direction of z axis")]
    public int zCount = 10;

    public GameObject moveableGo;
    public float moveableGoXSize = 15;
    public float moveableGoZSize = 15;

    private QuadTreeRectF<QTreeObject> qtree;
    private List<QTreeObject> visiableQTreeObjects;

    // Start is called before the first frame update
    void OnEnable()
    {
        visiableQTreeObjects = new List<QTreeObject>();
        qtree = new QuadTreeRectF<QTreeObject>(new RectangleF(centerX, centerZ, xCount * width, zCount * height));

        var unitId = 0;
        for (int i = 0; i < xCount; i++)
        {
            for (int j = 0; j < zCount; j++)
            {
                ++unitId;
                qtree.Add(new QTreeObject(new RectangleF(i * width, j * height, width, height), unitId));
            }
        }

        moveableGo.transform.localScale = new Vector3(moveableGoXSize, 1, moveableGoZSize);
    }

    // Update is called once per frame
    void Update()
    {
        qtree.GetObjects(new RectangleF(moveableGo.transform.position.x, moveableGo.transform.position.z, moveableGoXSize, moveableGoZSize), visiableQTreeObjects);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(moveableGo.transform.position, new Vector3(moveableGoXSize, 0, moveableGoZSize));

        Gizmos.color = Color.blue;
        foreach (var rect in qtree.GetAllObjects())
        {
            foreach (var visible in visiableQTreeObjects)
            {
                if (visible.id == rect.id)
                    continue;
            }
            Gizmos.DrawWireCube(new Vector3(rect.Rect.X, 0, rect.Rect.Y), new Vector3(rect.Rect.Width, 1, rect.Rect.Height));
        }

        Gizmos.color = Color.green;
        foreach (var rect in visiableQTreeObjects)
        {
            Gizmos.DrawWireCube(new Vector3(rect.Rect.X, 0, rect.Rect.Y), new Vector3(rect.Rect.Width, 1, rect.Rect.Height));
        }
    }
}
