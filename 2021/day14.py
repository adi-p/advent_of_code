import sys

from collections import Counter


def get_counts(template, insertion_rules, count):

    if count == 0:
        return Counter(template)

    pairs = zip(template, template[1:])

    new_template = []
    for a, b in pairs:
        new_template.append(a)
        new_template.append(insertion_rules[a+b])


    new_template.append(template[-1])
    # print(new_template)
    return get_counts(new_template, insertion_rules, count - 1)


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


    # counter = get_counts(list(template), insertion_rules, 40)
    # min_count = min(map(lambda el: el[1], counter.items()))
    # max_count = max(map(lambda el: el[1], counter.items()))
    # print("Part 2: result {}".format(max_count - min_count))


if __name__ == "__main__":
    main()