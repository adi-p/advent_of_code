import sys



def get_syntax_error_score(chunks):

    score_map = { ')': 3, ']': 57, '}': 1197 , '>': 25137 }
    parenth_map = { '(' : ')', '[' : ']', '{' : '}', '<' : '>' }
    def find_syntax_error(chunk):
        stack = []
        for parenth in chunk:
            if parenth in parenth_map.keys(): # is an opening parenth
                stack.append(parenth)
            else:
                cur_opening = stack.pop()
                if parenth != parenth_map[cur_opening]:
                    return True, parenth

        return False, None

    result = 0
    for chunk in chunks:
        has_error, first_wrong = find_syntax_error(chunk)
        if has_error:
            result += score_map[first_wrong]
    
    return result

def get_autocomplete_score(chunks):
    score_map = { ')': 1, ']': 2, '}': 3 , '>': 4 }
    parenth_map = { '(' : ')', '[' : ']', '{' : '}', '<' : '>' }
    def get_autocomplete_score_helper(chunk):
        score = 0
        stack = []
        for parenth in chunk:
            if parenth in parenth_map.keys(): # is an opening parenth
                stack.append(parenth)
            else:
                cur_opening = stack.pop()
                if parenth != parenth_map[cur_opening]:
                    return score

        # if we got here the line is correct but unfinished
        close_stack = [ parenth_map[p] for p in stack ]
        
        for i in range(len(close_stack) -1, -1, -1):
            score *= 5
            score += score_map[close_stack[i]]

        return score

    scores = []
    for chunk in chunks:
        score = get_autocomplete_score_helper(chunk)
        if score > 0:
            scores.append(score)
    
    scores.sort()
    return scores[int(len(scores) / 2)] # return the middle score


def parse_file(file):
    lines = []
    with open(file) as f:
        lines = f.read().splitlines()

    return lines

def main():

    file = sys.argv[1]
    chunks = parse_file(file)

    syntax_error_score = get_syntax_error_score(chunks)
    print("Part 1: The syntax error score is {}".format(syntax_error_score))

    score = get_autocomplete_score(chunks)
    print("Part 2: {} ".format(score))

if __name__ == "__main__":
    main()