import sys
from enum import Enum, auto


class InputError(Exception):
    """Exception raised for errors in the input.

    Attributes:
        expression -- input expression in which the error occurred
        message -- explanation of the error
    """

    def __init__(self, message):
        self.message = message


class Direction(Enum):
    FORWARD = auto()
    UP = auto()
    DOWN = auto()

def get_postion(commands):
    horizontal_pos = 0
    depth = 0

    for direction, value in commands:
        if(direction == Direction.UP):
            depth -= value
        elif(direction == Direction.DOWN):
            depth += value
        else:
            horizontal_pos += value

    return horizontal_pos, depth


def get_postion_part_2(commands):
    horizontal_pos = 0
    depth = 0
    aim = 0

    for direction, value in commands:
        if(direction == Direction.UP):
            aim -= value
        elif(direction == Direction.DOWN):
            aim += value
        else:
            horizontal_pos += value
            depth += value*aim

    return horizontal_pos, depth


def parse_file(file):
    lines = []
    with open(file) as f:
        lines = f.readlines()

    commands = []
    for line in lines:
        command = line.strip().split()
        direction = None
        
        if(command[0] == "forward"):
            direction = Direction.FORWARD
        elif(command[0] == "up"):
            direction = Direction.UP
        elif(command[0] == "down"):
            direction = Direction.DOWN
        else:
            raise InputError("Failed to parse inout correctly.")

        value = int(command[1])
        commands.append((direction, value))
    
    return commands 


def main():

    file = sys.argv[1]
    commands = parse_file(file)
    
    horizontal_pos, depth = get_postion(commands)
    print("Part 1: Your horizontal postions is {}, your depth is {}. {}*{}={}".format(horizontal_pos, depth, horizontal_pos, depth, horizontal_pos * depth))

    horizontal_pos, depth = get_postion_part_2(commands)
    print("Part 1: Your horizontal postions is {}, your depth is {}. {}*{}={}".format(horizontal_pos, depth, horizontal_pos, depth, horizontal_pos * depth))


if __name__ == "__main__":
    main()