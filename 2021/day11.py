import sys
import copy


class Board:
    def __init__(self, octopuses):
        self.octopuses = octopuses
        self.height = len(octopuses)
        self.width = len(octopuses[0])

    def __repr__(self):
        s = ''
        for i in range(self.height):
            for j in range(self.width):
                s += str(self.octopuses[i][j])
            s += '\n'
        return s
                

    def get_neighbours(self, row, col):
        def is_in_bound(index_tuple): 
            x, y = index_tuple
            return (x >= 0 and x < self.width) and (y >= 0 and y < self.height)
        neighbour_indexes = [(row-i,col-j) for i in range(-1,2) for j in range(-1,2) if not (j==0 and i==0)]
        return list(filter(is_in_bound, neighbour_indexes))

    def check_and_flash(self, flash_seen, start_row, start_col):
        def dfs_helper(stack):
            if(len(stack) == 0):
                return flash_seen

            row, col = stack.pop()
            if(self.octopuses[row][col] > 9 and not (row, col) in flash_seen):
                flash_seen[(row,col)] = 1
                neighbours = self.get_neighbours(row, col)
                for i, j in neighbours:
                    self.octopuses[i][j] += 1
                    stack.append((i,j))
            
            return dfs_helper(stack)
        
        return dfs_helper([(start_row, start_col)])


    def next(self):
        self.octopuses = [ list(map(lambda x: x + 1, row)) for row in self.octopuses ]
        flash_seen = {}
        for row_index, row in enumerate(self.octopuses):
            for col_index, octopuse in enumerate(row):
                # TODO: maybe skip if it already flashed
                flash_seen = self.check_and_flash(flash_seen, row_index, col_index)


        for i, j in flash_seen.keys():
            self.octopuses[i][j] = 0

        return len(flash_seen)


    def count_flashes(self, step_counts):
        flash_count = 0
        for i in range(step_counts):
            flash_count += self.next()

        return flash_count
        
    def find_all_flash(self):
        step_count = 1
        while(True):
            flash_count = self.next()

            if(flash_count == self.height * self.width):
                return step_count
            
            step_count += 1
        

        

def parse_file(file):
    lines = []
    with open(file) as f:
        lines = f.read().splitlines()

    return Board([[int(s) for s in list(line)] for line in lines ])

def main():

    file = sys.argv[1]
    board = parse_file(file)
    board_2 = copy.deepcopy(board)

    flash_count = board.count_flashes(100)
    print("Part 1: The flash count {}".format(flash_count))

    step = board_2.find_all_flash()
    print("Part 2: {} ".format(step))

if __name__ == "__main__":
    main()