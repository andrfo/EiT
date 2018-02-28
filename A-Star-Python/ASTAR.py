from Algorithms import *
from Graph import *
from Tools import *
from ImportMap import *
import sys
def main(argv):
    filename = 'gulv-modell-avansert.png'
    print(argv)
    test = ImportMap(filename)
    try:
        isinstance(argv, list)
        start_location = argv
        print("current position: "+argv(1))
    except:
        print("not valid input position")
        print("setting initial position to [50, 20]")
        start_location = (50 , 20)

    #current lowest cost
    lowest_cost = np.inf
    closest_door = []

    for i in range (0,len(test.map.doors)):
        came_from, cost_so_far = a_star_search(test.map, start_location,  test.map.doors[i])
        current_cost = (list(cost_so_far.items())[-1][1])

        if current_cost < lowest_cost:
            lowest_cost = current_cost
            closest_door = test.map.doors[i]
            cost = cost_so_far
    test.map = back_propagation(test.map, start_location, closest_door, cost)

    #draw_grid(test.map, width=3, point_to=came_from, start=start_location, goal=(closest_door))
    #draw_grid(test.map, width=3, number=cost_so_far, start=(80, 80), goal=(125, 125))
    #test = exportMap('floormodelOUT.png', test.map)

    exportMap(filename, test.map.path)

    return test.map.path



if __name__ == '__main__':
    main(sys.argv[1:])
    print("shortest path successfully found")
