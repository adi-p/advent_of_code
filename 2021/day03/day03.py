import sys
import operator


def find_most_common_bits(bin_nums):
    bits = []
    for i in range(len(bin_nums[0])):
        sum = 0
        for j in range(len(bin_nums)):
            sum += bin_nums[j][i]
        
        # print(sums)
        if(sum > (len(bin_nums) / 2) ):
            bits.append(1)
        elif(len(bin_nums) % 2 == 0 and sum == (len(bin_nums) / 2)):
            bits.append(-1)
        else:
            bits.append(0)

    return bits
            

def find_gamma_epsilon(bin_nums):
    
    most_common_bits = find_most_common_bits(bin_nums)
    gamma = ''
    epsilon = ''
    for bit in most_common_bits:
        if(bit == 1):
            gamma += "1"
            epsilon += "0"
        else:
            gamma += "0"
            epsilon += "1"

    return int(gamma,2), int(epsilon,2)

def find_co2_o2(bin_nums):
    def rec_cull_list(compare_op, index, temp_nums):

        # print(temp_nums)

        if(len(temp_nums) == 1):
            return ''.join([str(s) for s in temp_nums[0]])

        ones = 0
        zeros = 0
        for j in range(len(temp_nums)):
            if(temp_nums[j][index] == 0):
                zeros +=1
            else:
                ones +=1
        
        if(compare_op(zeros, ones)):
            bit = 0
        else:
            bit = 1

        temp_nums = list(filter(lambda nums: nums[index] == bit , temp_nums))

        return rec_cull_list(compare_op, index + 1, temp_nums)

    oxygen_lvl = rec_cull_list(operator.gt, 0, bin_nums)
    co2_lvl = rec_cull_list(operator.le, 0, bin_nums)

    return int(oxygen_lvl, 2), int(co2_lvl, 2)



def parse_file(file):
    lines = []
    with open(file) as f:
        lines = f.readlines()

    arr = [ list(line.strip()) for line in lines ]

    return [ [int(c) for c in row] for row in arr ] 


def main():

    file = sys.argv[1]
    bin_nums = parse_file(file)
    
    gamma, epsilon = find_gamma_epsilon(bin_nums)
    print("Part 1: Gamma {}, epsilon {}. {}*{}={}".format(gamma, epsilon, gamma, epsilon, gamma * epsilon))

    o2, co2 = find_co2_o2(bin_nums)
    print("Part 1: O2 {}, CO2 {}. {}*{}={}".format(o2, co2, o2, co2, o2 * co2))


if __name__ == "__main__":
    main()