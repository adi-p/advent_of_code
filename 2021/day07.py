import sys
import math


def find_min_fuel_cost(positions):
    max_el = max(positions)
    min_diff = math.inf
    for alignment_position in range(max_el + 1):
        diff = [ abs(alignment_position - position) for position in positions ]
        diff_sum = sum(diff)
        min_diff = min(diff_sum, min_diff)

    return min_diff

def find_min_fuel_cost_2(positions):
    max_el = max(positions)
    min_diff = math.inf
    for alignment_position in range(max_el + 1):
        diff = [ abs(alignment_position - position) for position in positions ]
        temp = [ (n*(n+1)) /2 for n in diff]
        diff_sum = sum(temp)
        min_diff = min(diff_sum, min_diff)

    return min_diff

def parse_file(file):
    lines = []
    with open(file) as f:
        lines = f.read().splitlines()
    
    return [ int(el) for el in lines[0].split(',') ]

def main():

    file = sys.argv[1]
    positions = parse_file(file)

    fuel_cost = find_min_fuel_cost(positions)
    print("Part 1: The lowest fuel cost is {} ".format(fuel_cost))

    fuel_cost = find_min_fuel_cost_2(positions)
    print("Part 2: The lowest fuel cost is {} ".format(fuel_cost))

if __name__ == "__main__":
    main()