import sys
from collections import Counter

# naive solution
def get_fish_count(iteration_count, fish):

    for day in range(iteration_count):
        new_fish = []
        for i in range(len(fish)):
            if(fish[i] == 0):
                new_fish.append(8)
                fish[i] = 6
            else:
                fish[i] -= 1
        fish = fish + new_fish
    
    return len(fish)

# less naive solution
def get_fish_count_2(iteration_count, fish):
    fish_counter = Counter(fish)
 
    for day in range(iteration_count):
        temp_counter = Counter()
        for k, v in fish_counter.items():
            if(k == 0):
                temp_counter[6] = temp_counter.get(6, 0) + v
                temp_counter[8] = v
            else:
                temp_counter[k - 1] = temp_counter.get(k-1, 0) + v
        fish_counter = temp_counter

    total = 0
    for k, v in fish_counter.items():
            total += v
    return total




def parse_file(file):
    lines = []
    with open(file) as f:
        lines = f.read().splitlines()
    
    return [ int(el)for el in lines[0].split(',') ]

def main():

    file = sys.argv[1]
    fish_day_count = parse_file(file)

    day_count = 256
    count = get_fish_count_2(day_count, fish_day_count)
    print("Part 1: There are {} fish after {} days".format(count, day_count))

if __name__ == "__main__":
    main()