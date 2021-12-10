import sys
import math        
from functools import reduce

class DepthMap:
    def __init__(self, depth_map):
        self.depth_map = depth_map
        self.HEIGHT = len(depth_map)
        self.WIDTH = len(depth_map[0])
    
    def get_neighbours(self, row, col):
        return [ (i,j) for (i,j) in [(row - 1, col), (row + 1, col), (row, col - 1), (row, col + 1)] if not (i < 0 or i >= self.HEIGHT) and not (j < 0 or j >= self.WIDTH) ]

    def find_low_points(self):
        depth_map = self.depth_map
        low_points = []
        low_points_indexes = []
        for row_index, row in enumerate(depth_map):
            for col_index, el in enumerate(row):
                neighbours = self.get_neighbours(row_index, col_index)
                if all([ el < depth_map[i][j] for i,j in neighbours ]):
                    low_points.append(el)
                    low_points_indexes.append((row_index, col_index))

        return low_points, low_points_indexes

    def get_low_points_risk(self):
        low_points, _ = self.find_low_points()
        return sum(low_points) + len(low_points)

    def find_largest_bassins(self):
        depth_map = self.depth_map
        def bfs(queue, seen, bassin_size):

            if(len(queue) == 0):
                return bassin_size

            row, col = queue[0]
            queue = queue[1:]
            neighbours = self.get_neighbours(row, col)

            for neighbour in neighbours:
                if neighbour in seen:
                    continue
                i,j = neighbour
                if depth_map[i][j] == 9:
                    continue
                
                seen[neighbour] = 1
                queue.append(neighbour)
                bassin_size += 1

            return bfs(queue, seen, bassin_size)
        

        low_points, low_points_indexes = self.find_low_points()
        bassin_sizes = []
        for start in low_points_indexes:
            bassin_size = bfs([start], {}, 0)
            bassin_sizes.append(bassin_size)

        bassin_sizes.sort()
        return reduce(lambda x,y: x*y, bassin_sizes[-3:])

def parse_file(file):
    lines = []
    with open(file) as f:
        lines = f.read().splitlines()

    return DepthMap([ [ int(s) for s in list(line)]  for line in lines ])

def main():

    file = sys.argv[1]
    depth_map = parse_file(file)

    # print(signals)
    sum_risk = depth_map.get_low_points_risk()
    print("Part 1: The sum of the risk for low points is {}".format(sum_risk))

    sum_three_big_bassins = depth_map.find_largest_bassins()
    print("Part 2: {} ".format(sum_three_big_bassins))

if __name__ == "__main__":
    main()