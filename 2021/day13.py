import sys


def make_folds(points, folds, fold_count):
    if fold_count == 0:
        return points, folds

    fold_axis, fold_value = folds[0]

    new_points = {}
    for point in points.keys():
        x, y = point
        if fold_axis == 'y':
            new_y = y
            if y > fold_value:
                new_y = fold_value - (y - fold_value)
            new_points[(x,new_y)] = 1
        else:
            new_x = x
            if x > fold_value:
                new_x = fold_value - (x - fold_value)
            new_points[(new_x,y)] = 1

    fold_count -= 1
    return make_folds(new_points, folds[1:], fold_count)

def parse_file(file):
    lines = []
    with open(file) as f:
        lines = f.read().splitlines()

    at_folds = False
    points = {}
    folds = []

    for line in lines:
        if line == "":
            at_folds = True
            continue
        if at_folds:
            temp = line.split('=')
            axis = temp[0][-1]
            value = int(temp[1])
            folds.append((axis, value))
        else:
            x, y = line.split(',')
            points[(int(x), int(y))] = 1

    return points, folds

def main():

    file = sys.argv[1]
    points, folds = parse_file(file)

    new_points, new_folds = make_folds(points, folds, 1)
    print("Part 1: point count {}".format(len(new_points.keys())))
    
    # print the points to read what they spell
    new_points, new_folds = make_folds(points, folds, len(folds))
    width = max(map(lambda point: point[0], new_points.keys())) + 1
    height = max(map(lambda point: point[1], new_points.keys())) + 1
    
    final_board = [[ '.' for j in range(width)] for i in range(height) ]
    for x, y in new_points.keys():
        final_board[y][x] = '#'
    
    for i in range(height):
        print(''.join(final_board[i]))

if __name__ == "__main__":
    main()