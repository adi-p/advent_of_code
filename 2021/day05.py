import sys

class Point:
    def __init__(self, x, y):
        self.x = x
        self.y = y

    def __str__(self):
        return '<{},{}>'.format(self.x, self.y)


# def get_covering(segments):
#     covering = {}

#     for p1, p2 in segments:
#         for i in range(min(p1.x, p2.x), max(p1.x, p2.x) + 1):
#             for j in range(min(p1.y, p2.y), max(p1.y, p2.y) + 1):
#                 covering[(i,j)] = covering.get((i,j), 0) + 1 
#     return covering

def get_covering(segments):
    # NOTE this assumes diagonal lines are alwasy 45 degrees
    covering = {}

    for p1, p2 in segments:
        x_diff = p2.x - p1.x
        y_diff = p2.y - p1.y
        length = max(abs(x_diff), abs(y_diff))

        x_incr = x_diff/length
        y_incr = y_diff/length

        for i in range(length + 1):
                point = (p1.x + i*x_incr, p1.y + i*y_incr)
                covering[point] = covering.get(point, 0) + 1

    return covering

def get_vertical_and_horizontal(segments):
    def is_vertical_or_horizontal(points):
        p1, p2 = points
        return p1.y == p2.y or p1.x == p2.x
    return list(filter(is_vertical_or_horizontal, segments))


def parse_file(file):
    lines = []
    with open(file) as f:
        lines = f.read().splitlines()
    
    segments = []


    for line in lines:
        temp_points = [ point.split(',') for point in line.split(" -> ")]
        point_1 = Point(int(temp_points[0][0]), int(temp_points[0][1]))
        point_2 = Point(int(temp_points[1][0]), int(temp_points[1][1]))
        segments.append((point_1, point_2))

    return segments

def main():

    file = sys.argv[1]
    segments = parse_file(file)
    print(len(segments))

    covering = get_covering(get_vertical_and_horizontal(segments))
    covered_twice_or_more = [k for k,v in covering.items() if v >= 2]
    print("Part 1: points covered more than twice: {}".format(len(covered_twice_or_more)))


    covering = get_covering(segments)
    covered_twice_or_more = [k for k,v in covering.items() if v >= 2]
    print("Part 2: points covered more than twice: {}".format(len(covered_twice_or_more)))


if __name__ == "__main__":
    main()