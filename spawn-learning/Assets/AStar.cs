using System.Collections.Generic;
using System.Collections;
using System.Linq;
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
    public GameObject cube;
}

public class AStar : MonoBehaviour
{

    public Texture2D image;
    private List<SearchNode> goals;
    private SearchNode[,] map;
    private List<SearchNode> path;
    public GameObject door;
    


    // Use this for initialization
    void Start()
    {
        goals = new List<SearchNode>();
        map = new SearchNode[image.width, image.height];
        loadMap();


        
        bestFirstSearch(20, 50);

        printMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private int findIndexOfMin(List<float> A)
    {
        float smallest = Mathf.Infinity;
        int n = 0;
        for (int i = 0; i < A.Count; i++)
        {
            if(A[i] < smallest)
            {
                smallest = A[i];
                n = i;
            }
        }
        return n;
    }

    public List<List<int>> bestFirstSearch(int startX, int startY)
    {

        SearchNode startNode = map[startX, startY];
        startNode.type = 'O';
        List<List<SearchNode>> paths = new List<List<SearchNode>>();
        List<float> costs = new List<float>();

        for (int i = 0; i < goals.Count; i++)
        {
            SearchNode n = goals[i];
            if(Astar(startNode, n))
            {
                paths.Add(path);
                costs.Add(0);
                foreach (SearchNode p in path)
                {
                    if(p.parent != null)
                    {
                        costs[i] += edgeCost(p.parent, p);
                    }
                }
                Debug.Log(i + " " + costs[i]);

            }
        }
        int j = findIndexOfMin(costs);
        Debug.Log(j);
        List<SearchNode> bestPath = paths[j];
        List<List<int>> intPath = new List<List<int>>();
        for (int i = 0; i < bestPath.Count; i++)
        {
            intPath.Add(new List<int>());
            int x = bestPath[i].x;
            int y = bestPath[i].y;
            intPath[i].Add(x);
            intPath[i].Add(y);
            map[x, y].type = 'B';
        }
        return intPath;
        
    }

    private void printMap()
    {
        for (int j = 0; j < image.height; j++)
        {
            for (int i = 0; i < image.width; i++)
            {
                if (map[i, j].type.Equals('P'))
                {
                    map[i, j].cube.GetComponent<Renderer>().material.color = Color.magenta;
                    continue;
                }
                if (map[i, j].type.Equals('B'))
                {
                    map[i, j].cube.GetComponent<Renderer>().material.color = Color.green;
                    continue;
                }
            }
        }
    }

    private SearchNode popLeft(List<SearchNode> A)
    {
        SearchNode r = A[0];
        A.RemoveAt(0);
        return r;
    }

    private float edgeCost(SearchNode a, SearchNode b)
    {
        if (a.x != b.x && a.y != b.y)
        {
            return 1.3f;
        }
        return 1.0f;
    }


    private void attachEval(SearchNode C, SearchNode P, SearchNode goal)
    {
        C.parent = P;
        C.g = P.g + edgeCost(P, C);
        C.h = heuristic(C, goal);
        C.f = C.g + C.h;
    }

    //Goes through the chain of children and updates if better.
    private void propagatePathImprovements(SearchNode P)
    {
        foreach (SearchNode n in P.children)
        {
            if (P.g + edgeCost(P, n) < n.g)
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
    public bool Astar(SearchNode start, SearchNode goal)
    {
        List<SearchNode> open = new List<SearchNode>();
        List<SearchNode> closed = new List<SearchNode>();
        SearchNode current;


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
                    open.Sort((x, y) => x.f.CompareTo(y.f));// Sorts based on f. If equal f it sorts on the heuristic(manhattan distance)
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
    public List<SearchNode> getGoal()
    {
        return goals;
    }



    /**
     * Sort the list in descending order
     * 
     */
    public List<SearchNode> Quicksort(List<SearchNode> elements)
    {
        if (elements.Count < 2) return elements;
        int pivot = (int)elements.Count / 2;
        SearchNode val = elements[pivot];
        List<SearchNode> lesser = new List<SearchNode>();
        List<SearchNode> greater = new List<SearchNode>();
        for (int i = 0; i < elements.Count; i++)
        {
            if (i != pivot)
            {
                if (elements[i].f < elements[pivot].f)
                {
                    lesser.Add(elements[i]);
                }
                else
                {
                    greater.Add(elements[i]);
                }
            }
        }

        List<SearchNode> merged = Quicksort(lesser);
        merged.Add(val);
        merged.AddRange(Quicksort(greater));
        return merged;
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
                if (i == 0 && j == 0) { continue; }
                int posX = N.x + i;
                int posY = N.y + j;
                if ((posX > 0 && posX < image.width - 1) &&
                    (posY > 0 && posY < image.height - 1))
                {
                    SearchNode neighbour = map[posX, posY];
                    if (!neighbour.type.Equals('W'))
                    {
                        N.children.Add(neighbour);
                    }

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
        for (int j = 0; j < image.height; j++)
        {
            for (int i = 0; i < image.width; i++)
            {
                Color pixel = image.GetPixel(i, j);
                SearchNode node = new SearchNode();
                map[i, j] = node;
                node.children = new List<SearchNode>();
                node.cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                node.cube.transform.position = new Vector3(i, 0, j);
                node.x = i;
                node.y = j;

                //Is it a wall?
                if (pixel.r == 0 && pixel.g == 0 && pixel.b == 0)
                {
                    node.type = 'W';
                    node.weight = getWeight('W');
                    node.cube.transform.localScale += new Vector3(0, 2, 0);
                    node.cube.transform.position = new Vector3(node.cube.transform.position.x, 1.5f, node.cube.transform.position.z); 
                    node.cube.GetComponent<Renderer>().material.color = Color.red;
                    continue;
                }
                //Is it a door?
                if (pixel.r > 0.5 && pixel.g > 0.5 && pixel.b < 0.5)
                {
                    node.type = 'D';
                    node.weight = getWeight('D');
                    node.cube.GetComponent<Renderer>().material.color = Color.blue;
                    GameObject d = Instantiate(door);
                    d.transform.position = new Vector3(node.x, 0.54f, node.y);
                    d.transform.localScale = new Vector3(0.46f, 0.49f, 0.64f);

                    goals.Add(node);
                    continue;
                }
                if (pixel.r > 0.5 && pixel.g > 0.5 && pixel.b > 0.5)
                {
                    //It is a floor.
                    node.type = 'F';
                    node.weight = getWeight('F');
                }
            }
        }

        foreach (SearchNode n in goals)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0) { continue; }
                    int posX = n.x + i;
                    int posY = n.y + j;
                    SearchNode neighbour = map[posX, posY];
                    if (neighbour.type.Equals('W'))
                    {
                        neighbour.cube.transform.localScale = new Vector3(1, 1, 1);
                        neighbour.cube.transform.position = new Vector3(neighbour.cube.transform.position.x, 0, neighbour.cube.transform.position.z);
                    }
                }
            }
        }
        Debug.Log("Map Loaded");
    }
}

