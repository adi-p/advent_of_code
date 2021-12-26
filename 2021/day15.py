import sys
import heapq


class Cell:
    def __init__(self, value, coordinates):
        self.value = value
        self.coordinates = coordinates
        self.risk_accrued = 0

    def __lt__(self, other):
        return self.value + self.risk_accrued < other.value + other.risk_accrued

def get_neighbours(board, cell):
    HEIGHT = len(board)
    WIDTH = len(board[0])
    row, col = cell.coordinates
    return [ Cell(board[i][j] ,(i,j)) for (i,j) in [(row - 1, col), (row + 1, col), (row, col - 1), (row, col + 1)] 
        if not (i < 0 or i >= HEIGHT) and not (j < 0 or j >= WIDTH) ]


# basically just dijkstra's
def find_min_path(board):
    END = Cell(board[-1][-1], (len(board[0]) - 1, len(board) -1))
    START = Cell(0, (0,0))
    seen = {}
    # Prefill stuff
    seen[START.coordinates] = None 
    frontier = [START]
    heapq.heapify(frontier)

    while(True):
        try:
            current_cell = heapq.heappop(frontier)
            if current_cell.coordinates == END.coordinates:
                break
            
            neighbours = get_neighbours(board, current_cell)
            for neighbour in neighbours:
                if neighbour.coordinates in seen:
                    continue
                
                # update how much risk the path has accrued to get to this cell
                neighbour.risk_accrued = current_cell.value + current_cell.risk_accrued
                seen[neighbour.coordinates] = current_cell
                heapq.heappush(frontier, neighbour)
                
        except IndexError:
            print("No more cells to explore")
            break
    
    find_path = END
    path = [find_path]
    while seen[find_path.coordinates] is not None:
        find_path = seen[find_path.coordinates]
        path.append(find_path)

    return list(reversed(path)) 

# Maybe this board could be build as we search instead of beforehand
def build_bigger(board):
    SMALL_HEIGHT = len(board)
    SMALL_WIDTH = len(board[0])
    big_board = [[ 0 for j in range(SMALL_WIDTH * 5)] for i in range(SMALL_HEIGHT * 5)]

    for i in range(SMALL_HEIGHT):
        for j in range(SMALL_WIDTH):
            big_board[i][j] = board[i][j]
            for k in range(1,5):
                big_board[i + (k * SMALL_HEIGHT)][j] = (big_board[i][j] + k) % 10 + 1 if (big_board[i][j] + k) > 9 else (big_board[i][j] + k)


    for i in range(SMALL_HEIGHT*5):
        for j in range(SMALL_WIDTH):
            for k in range(1,5):
                big_board[i][j + (k * SMALL_WIDTH)] = (big_board[i][j] + k) % 10 + 1 if (big_board[i][j] + k) > 9 else (big_board[i][j] + k)
    return big_board

def parse_file(file):
    lines = []
    with open(file) as f:
        lines = f.read().splitlines()

    return [[int(el) for el in line.strip()] for line in lines]


def main():

    file = sys.argv[1]
    board = parse_file(file)

    path = find_min_path(board)
    total = sum(map(lambda cell: cell.value, path))
    print("Part 1: Total Risk taken {}".format(total))

    big_board = build_bigger(board)
    # for line in big_board:
    #     print(''.join(map(lambda el: str(el), line)))
    path_2 = find_min_path(big_board)
    total_2 = sum(map(lambda cell: cell.value, path_2))
    print("Part 2: Total Risk taken {}".format(total_2))



if __name__ == "__main__":
    main()