import sys
import math


def count_simple_digits(signals):
    # The digits 1, 4, 7, 8 are uniquely represented using 2, 4, 3 and 7 segments respectively
    simple_digits_segments_counts = [ 2, 3, 4, 7 ] 

    count = 0
    for signal in signals:
        _, outputs = signal
        for output in outputs:
            if len(output) in simple_digits_segments_counts:
                count += 1

    return count

# this isn't very nicely done but it does work
def decode_output(signals):
    
    def find_simple_digits(patterns):
        one = four = seven = eight = None
        for pattern in patterns:
            if len(pattern) == 2:
                one = pattern
            elif len(pattern) == 4:
                four = pattern
            elif len(pattern) == 3:
                seven = pattern
            elif len(pattern) == 7:
                eight = pattern
        return one, four, seven, eight

    def decode(simple_digits, pattern):
        one, four, seven, eight = simple_digits
        c_set = [x for x in four if x not in one] # this will be used to find 5 (the c shaped part)
        if len(pattern) == 2:
            return 1
        elif len(pattern) == 4:
            return 4
        elif len(pattern) == 3:
            return 7
        elif len(pattern) == 7:
            return 8
        elif len(pattern) == 6: # (0, 6, 9)
            if len([ c for c in pattern if c in one]) == 2 : #(0,9)
                if len([ c for c in pattern if c in four]) == 4 : # 9
                    return 9
                else:
                    return 0
            else:
                return 6
        elif len(pattern) == 5: # (2, 3, 5)
            if len([ c for c in pattern if c in one]) == 2 : #(3)
                return 3
            else:
                if len([ c for c in pattern if c in c_set]) == 2:
                    return 5
                else:
                    return 2
        else:
            print( "Pattern {} is not being detected properly".formatpattern)
            

    sum = 0

    for signal in signals:
        patterns, outputs = signal
        simple_digits = find_simple_digits(patterns)
        temp = ''
        for output in outputs:
            temp = temp + str(decode(simple_digits, output))
        
        sum += int(temp)
    return sum
        


def parse_file(file):
    lines = []
    with open(file) as f:
        lines = f.read().splitlines()
    
    result = []
    for line in lines:
        patterns, outputs = line.split('|')
        result.append((list(map(lambda s: list(s), patterns.split())), list(map(lambda s: list(s), outputs.split()))))

    return result

def main():

    file = sys.argv[1]
    signals = parse_file(file)

    # print(signals)
    simple_digits_count = count_simple_digits(signals)
    print("Part 1: There are {} simple digits".format(simple_digits_count))

    val = decode_output(signals)
    print("Part 2: {} ".format(val))

if __name__ == "__main__":
    main()