using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchNode
{
    public int x;
    public int y;
    public SearchNode parent;
    public char type;
    public List<SearchNode> children;
    public float weight;
    public float g;
    public float h;
    public float f;
}

public class AStar : MonoBehaviour {

    public Texture2D image;

    private SearchNode startNode;
    private List<SearchNode> goals;
    private SearchNode[,] map;
    private List<SearchNode> path;


    // Use this for initialization
    void Start()
    {
        goals = new List<SearchNode>();
        map = new SearchNode[image.width, image.height];
        loadMap();
        startNode = new SearchNode
        {
            x = 30,
            y = 30,
            type = 'O',
            weight = 1,
            parent = null
        };
        map[30, 30] = startNode;
        Debug.Log(goals[0].x);

        bestFirstSearch(startNode, goals[0]);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private SearchNode popLeft(List<SearchNode> A)
    {
        SearchNode r = A[A.Count - 1];
        A.RemoveAt(A.Count - 1);
        return r;
    }

    private float edgeCost(SearchNode a, SearchNode b)
    {
        if(a.x != b.x && a.y != b.y)
        {
            return 1.3f;
        }
        return 1.0f;
    }


    private void attachEval(SearchNode C, SearchNode P, SearchNode goal)
    {
        C.parent = P;
        C.g = P.g + 1;
        C.h = heuristic(C, goal);
        C.f = C.g + C.h;
    }

    //Goes through the chain of children and updates if better.
    private void propagatePathImprovements(SearchNode P)
    {
        foreach (SearchNode n in P.children)
        {
            if(P.g + edgeCost(P, n) < n.g)
            {
                n.parent = P;
                n.g = P.g + edgeCost(P, n);
                n.f = n.g + n.h;
                propagatePathImprovements(n); //Recursion, this is where it chains backwards.
            }
        }
    }


    /**
     * A*-search.
     * Returns: bool. True if success, False is not.
     */
    public bool bestFirstSearch(SearchNode start, SearchNode goal)
    {
        List<SearchNode> open = new List<SearchNode>();
        List<SearchNode> closed = new List<SearchNode>();
        SearchNode current;


        Debug.Log("heihei");


        start.g = 0;
        start.h = heuristic(start, goal);
        start.f = start.g + start.h;

        open.Add(start);

        // Loops while there are still nodes to check, or the path is found
        while (open.Count > 0)
        {
            current = popLeft(open);
            closed.Add(current);

            // Reached target, creating path
            if (current == goal)
            {
                path = new List<SearchNode>();
                path.Add(current);
                path.Add(start);
                SearchNode no = current.parent;
                while (no.parent != null)
                {
                    path.Add(no);
                    no = no.parent;
                }
                foreach (SearchNode node in path)
                {
                    node.type = 'P';
                }
                return true;
            }


            generateChildren(current);
            // Goes through all children and processes them
            foreach (SearchNode node in current.children)
            {
                //Checks if it is a "new" node
                if (!open.Contains(node) && !closed.Contains(node))
                {
                    attachEval(node, current, goal); //sets parent and calculates g, h, and f
                    open.Add(node);
                    open = quickSort(open); // Sorts based on f. If equal f it sorts on the heuristic(manhattan distance)
                }
                // If already discovered, but the current node is a better parent
                else if (current.g + edgeCost(current, node) < node.g)
                {
                    attachEval(node, current, goal);
                    //if already visited, but the current node is a better parent
                    if (closed.Contains(node))
                    {
                        propagatePathImprovements(node);
                    }
                }
            }

        }

        return false;
    }

    public void setStart(SearchNode start)
    {
        this.startNode = start;
        start.parent = null;
    }
    public SearchNode getStart()
    {
        return startNode;
    }
    public List<SearchNode> getGoal()
    {
        return goals;
    }

    /**
     * Sort the list in descending order
     * 
     */
    private List<SearchNode> quickSort(List<SearchNode> A)
    {
        //TODO: Sort based on h-value aswell
        List<SearchNode> less = new List<SearchNode>();
        List<SearchNode> equal = new List<SearchNode>();
        List<SearchNode> greater = new List<SearchNode>();
        int pivot;

        if(A.Count > 1)
        {
            pivot = (int)A[A.Count - 1].f;
            foreach (SearchNode node in A)
            {
                if(node.f < pivot) { less.Add(node); }
                else if (node.f == pivot) { equal.Add(node); }
                else if (node.f > pivot) { greater.Add(node); }
            }

            List<SearchNode> An = new List<SearchNode>();

            An.AddRange(quickSort(less));
            An.AddRange(quickSort(equal));
            An.AddRange(quickSort(greater));

        }
        return A;
    }


    //Calculates estimated cost from a to b. Manhattan
    private int heuristic(SearchNode a, SearchNode b)
    {
        return (int)(Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y + b.y));
    }


    private void generateChildren(SearchNode N)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int posX = N.x + i;
                int posY = N.y + j;
                if((posX > 0 && posX < image.width - 1) && 
                    (posY > 0 && posY < image.height - 1) &&
                    (posX == 0 && posY == 0))
                {
                    SearchNode neighbour = map[posX, posY];
                    if(neighbour.type != 'W') { N.children.Add(neighbour); }

                }
            }
        }
    }

    private float getWeight(char type)
    {
        return 1.0f;
    }

    /**
     * Creates a 2d array <map> from the image.
     * {255, 242, 0} = door -> 2
     * {0, 0, 0} = wall -> 1
     * {x, y, z} = floor -> 0
     * 
     * Could be an idea here to generate all the children. Could save processing time while running
     */
    private void loadMap()
    {
        for (int i = 0; i < image.width; i++)
        {
            for (int j = 0; j < image.height; j++)
            {
                Color pixel = image.GetPixel(i, j);
                SearchNode node = new SearchNode();
                map[i, j] = node;
                
                //Is it a wall?
                if (pixel.r < 0.5 && pixel.g < 0.5 && pixel.b < 0.5)
                {
                    node.type = 'W';
                    node.weight = getWeight('W');
                    node.x = i;
                    node.y = j;
                    continue;
                }
                //Is it a door?
                if (pixel.r > 0.5 && pixel.g > 0.5 && pixel.b < 0.5)
                {
                    node.type = 'D';
                    node.weight = getWeight('D');
                    node.x = i;
                    node.y = j;
                    goals.Add(node);
                    continue;
                }
                //It is a floor.
                node.type = 'F';
                node.weight = getWeight('F');
                node.x = i;
                node.y = j;
            }
        }
        Debug.Log("Map Loaded");
    }



    private int[,] backpropagation()
    {
        return null;
    }
}

