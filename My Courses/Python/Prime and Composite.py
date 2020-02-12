# create a function that takes in a number and returns a list of al prime numbers less than that number
# what is a list, how to add to a list, what is a prime number, is 1 a prime number?

def get_num(): # asks for number (to use in prime functions)
    answer = int(input("what is the number?"))
    return answer


def is_prime(number): # finds prime in 2 functions (1st)
    for i in range(2, number):
        if number % i == 0:
            return False
    return True

def list_of_primes(number): # finds prime in 2 functions (2nd)
    prime_list = []
    i = 2
    while i < number:
        if is_prime(i):
            prime_list.append(i)
        i = i + 1
    print("Prime numbers are:")
    print(prime_list)

def all_primes(number): # finds prime in 1 function
    prime_list = []
    for x in range(2, number):
        for i in range(2, x + 1):
            if x == 2:
                prime_list.append(x)
                break
            if all(x % i != 0 for i in range(2,x)):
                prime_list.append(x)
                break
    print("Prime numbers are:")
    print(prime_list)

def get_other_num(): # asks for number (to use for composite functions)
    num = int(input("what is the other number?"))
    return num


def is_composite(number): # finds composite in 2 functions (1st)
    for i in range(2, number):
        if number % i == 0:
            return True
    return False

def list_of_composite(number): # finds composite in 2 functions (2nd)
    composite_list = []
    i = 2
    while i < number:
        if is_composite(i):
            composite_list.append(i)
        i = i + 1
    print("Composite numbers are:")
    print(composite_list)

def all_composites(number): # finds composite in 1 function
    composite_list = []
    for x in range(2, number):
        for i in range(2, x):
            if x % i == 0:
                composite_list.append(x)
                break
    print("Composite numbers are:")
    print(composite_list)


if __name__ == "__main__":
    find_prime = get_num()
    get_prime = all_primes(find_prime)
    find_composite = get_other_num()
    get_composite = all_composites(find_composite)


