import numpy as np
from PIL import Image
from Graph import SquareGrid




class ImportMap:
    def __init__(self, image):

            img = Image.open(image).convert('RGB')
            arr = np.array(img)

            # record the original shape
            shape = arr.shape

            self.map = SquareGrid(shape[1], shape[0])
            self.checked_doors = []
            for i in range (0, shape[0]):
                for j in range (0, shape[1]):
                    if arr[i][j][0] == 0 and arr[i][j][1] == 0 and arr[i][j][2] == 0 :
                        self.map.walls.append((j, i))
                        #print("wall found")

                    if self.is_door(arr, i, j) and (i, j) not in self.checked_doors:
                        self.check_neighbors(arr, (i, j))
                        self.map.doors.append((j, i))
                        #print("door found")

    def check_neighbors(self, arr, position):
        self.checked_doors.append(position)
        for k in range(-1, 2):
            for l in range(-1, 2):
                pos1 = position[0] + k
                pos2 = position[1] + l
                pos = (pos1, pos2)
                if self.is_door(arr, pos1, pos2):
                    if (pos) not in self.checked_doors:

                        self.check_neighbors(arr, pos)
    def is_door(self, arr, i, j):
        return arr[i][j][0] == 255 and arr[i][j][1] == 242 and arr[i][j][2] == 0


def exportMap(image, path):
    img = Image.open(image).convert('RGB')
    arr = np.array(img)
    for t in path:
        arr[t[1]][t[0]][0] = 255
        arr[t[1]][t[0]][1] = 0
        arr[t[1]][t[0]][2] = 0
    im = Image.fromarray(arr)
    im.save("outfile.png")
