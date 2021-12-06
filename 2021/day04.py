import sys
import operator


BOARD_HEIGHT = 5
BOARD_WIDTH = 5

def checkBingo(board, row, col):
    col_win = True
    row_win = True
    for i in range(BOARD_HEIGHT):
        val, seen = board[i][col]
        if(seen == 0):
            col_win = False
            break

    for i in range(BOARD_WIDTH):
        val, seen = board[row][i]
        if(seen == 0):
            row_win = False
            break
    
    return row_win or col_win

def sum_unseen(board):
    sum = 0
    for i in range(BOARD_HEIGHT):
        for j in range(BOARD_WIDTH):
            val, seen = board[i][j]
            if(seen == 0):
                sum += val
    return sum

def bingo(numbers, boards):
    final_num = None
    boards = [[list(map(lambda el: (el,0), row)) for row in board] for board in boards]

    for num in numbers:
        final_num = num
        for board in boards:
            # TODO:: how to split this out? I think the nested level is too high.
            # Problem -> I modify board. How to replicate that behaviour.
            #
            for i in range(BOARD_HEIGHT):
                for j in range(BOARD_WIDTH):
                    val, seen = board[i][j]
                    if(val == num):
                        board[i][j] = (val, 1)
                        if(checkBingo(board, i, j)):
                            return final_num, sum_unseen(board)

# def bingo_part2(numbers, board):
#     final_num = None
#     boards = [[list(map(lambda el: (el,0), row)) for row in board] for board in boards]

#     last_winner = None

#     def rec_mark_and_check(num, boards):
        
#         if(len(boards) == 0):
#             return last_winner
        
#         board = boards[1]
#         for i in range(BOARD_HEIGHT):
#             for j in range(BOARD_WIDTH):
#                 val, seen = board[i][j]
#                 if(val == num):
#                     board[i][j] = (val, 1)
#                     if(checkBingo(board, i, j)):
#                         last_winner = board
#                         return rec_mark_and_check(num,boards[1:])

#         return rec_mark_and_check(num, boards)

#     for num in numbers:
#         final_num = num


        

def parse_file(file):
    lines = []
    with open(file) as f:
        lines = f.read().splitlines()
    
    numbers = [ int(n) for n in lines[0].split(',') ]

    lines = lines[2:]

    boards = []
    board = []
    for line in lines:
        if (line == ''):
            boards.append(board)
            board = []
            continue
        board.append([ int(n) for n in line.split() ])

    boards.append(board)
    return numbers, boards 


def main():

    file = sys.argv[1]
    numbers, boards = parse_file(file)

    final_num, unsee_sum = bingo(numbers, boards)
    print("Part 1: final_num {}, unsee_sum {}. {}*{}={}".format(final_num, unsee_sum, final_num, unsee_sum, final_num * unsee_sum))


if __name__ == "__main__":
    main()