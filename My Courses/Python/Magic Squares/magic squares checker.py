"""
Magic Squares are square arrays of numbers that have the interesting property that the numbers in each column,
and in each row, all add up to the same total.

Given a 4 x 4 square of numbers, determine if it is magic square

"""

def magic(a):
    x = 0
    sum1 = 0
    sum2 = 0
    sum3 = 0
    sum4 = 0
    sum5 = 0
    sum6 = 0
    for line in a:
        number = line.split()
        sum1 = (int(number[0]) + int(number[1]) + int(number[2]) + int(number[3]))
        sum3 = sum3 + int(number[0])
        sum4 = sum4 + int(number[1])
        sum5 = sum5 + int(number[2])
        sum6 = sum6 + int(number[3])
        if x == 0:
            sum2 = sum1
        if sum2 != sum1:
            x = 55
        sum1 = 0
        x = x + 1
    if (sum3 != sum4) or (sum3 != sum5) or (sum3 != sum6):
        x = 55
    if x < 10:
        print("magic")
    if x > 10:
        print("not magic")


"""
Need to take input of text file 'magic_square.txt' to make function work

>>> a = open("magic_square.txt")
>>> magic(a)
magic
"""

