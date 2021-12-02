import sys


def count_increase(depths):
    increase_count = 0

    for i in range(1,len(depths)):
        if depths[i] > depths[i-1]:
            increase_count += 1
    
    return increase_count

def count_sliding_door_increase(depths):
    increase_count = 0

    for i in range(3,len(depths)):
        window_1 = sum([depths[i - j] for j in range(0,3)])
        window_2 = sum([depths[i - j] for j in range(1,4)])
        if window_1 > window_2:
            increase_count += 1
    
    return increase_count

def parse_file(file):
    lines = []
    with open(file) as f:
        lines = f.readlines()
    
    return [ int(line.strip()) for line in lines]

def main():

    file = sys.argv[1]
    depths = parse_file(file)
    
    increase_count = count_increase(depths)
    print("Part 1: there were {} increases".format(increase_count))

    sliding_increase_count = count_sliding_door_increase(depths)
    print("Part 2: there were {} increases".format(sliding_increase_count))


if __name__ == "__main__":
    main()