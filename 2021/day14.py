import sys
import math

# Solution change from part 1 to part 2 was hinted to me by kade-robertson

def get_counts(template, insertion_rules, steps):
    def get_counts_helper(pair_counter, char_counter):
        for i in range(steps):
            new_pair_counter = {}
            for pair, count in pair_counter.items():
                a, b = pair
                new_char = insertion_rules[a+b]
                # count the new char
                char_counter[new_char] = char_counter.get(new_char, 0) + count

                # count the pairs
                new_pair_counter[(a + new_char)] = new_pair_counter.get((a + new_char), 0) + count 
                new_pair_counter[(new_char + b)] = new_pair_counter.get((new_char + b), 0) + count
            pair_counter = new_pair_counter
        return char_counter

    # prep work for the real function
    
    new_pair_counter = {}
    char_counter = {}
    pairs = zip(template, template[1:])

    for a, b in pairs:
        new_pair_counter[(a + b)] = new_pair_counter.get((a + b), 0) + 1

    for c in template:
        char_counter[c] = char_counter.get(c, 0) + 1
        
    return get_counts_helper(new_pair_counter, char_counter)


def parse_file(file):
    lines = []
    with open(file) as f:
        lines = f.read().splitlines()

    template = lines[0]

    lines = lines[2:]
    insertion_rules = {}
    for line in lines:
        pair, insert = [ el.strip() for el in line.split('->') ]
        insertion_rules[pair] = insert

    return template, insertion_rules


def main():

    file = sys.argv[1]
    template, insertion_rules = parse_file(file)

    counter = get_counts(list(template), insertion_rules, 10)
    min_count = min(map(lambda el: el[1], counter.items()))
    max_count = max(map(lambda el: el[1], counter.items()))
    print("Part 1: result {}".format(max_count - min_count))


    counter = get_counts(list(template), insertion_rules, 40)
    min_count = min(map(lambda el: el[1], counter.items()))
    max_count = max(map(lambda el: el[1], counter.items()))
    print("Part 2: result {}".format(max_count - min_count))


if __name__ == "__main__":
    main()