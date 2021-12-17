import sys
import copy

START = 'start'
END = 'end'

def find_paths(cave_system, max_see = 1):
    paths = []
    def search(cave, cur_path, seen, cur_max_see):
        if(cave == END):
            paths.append(cur_path + [END])
            return

        see_count = seen.get(cave, 0)
        if see_count < cur_max_see:
            if cave == START:
                seen[START] = 2
            elif cave.islower():
                seen[cave] = seen.get(cave, 0) + 1
                if seen[cave] >= 2: # for part 2
                    cur_max_see = 1
            neighbours = cave_system[cave]
            for neighbour in neighbours:
                search(neighbour, cur_path + [cave], dict(seen), cur_max_see)

    search(START, [], {}, max_see)
    return paths

def parse_file(file):
    lines = []
    with open(file) as f:
        lines = f.read().splitlines()


    cave_system = {}

    for line in lines:
        caves = line.split('-')

        cave_system[caves[0]] = cave_system.get(caves[0], []) + [caves[1]]
        cave_system[caves[1]] = cave_system.get(caves[1], []) + [caves[0]]

    return cave_system

def main():

    file = sys.argv[1]
    cave_system = parse_file(file)

    paths = find_paths(cave_system)
    print("Part 1: path count {}".format(len(paths)))
    

    paths_2 = find_paths(cave_system, 2)
    print("Part 2: path count {}".format(len(paths_2)))
    # for i,path in enumerate(paths_2):
    #     print(i,path)

if __name__ == "__main__":
    main()