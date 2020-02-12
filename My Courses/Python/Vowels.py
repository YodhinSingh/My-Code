# create function, that returns number of vowels in string (includes a,e,i,o,u or A,E,I,O,U)
# create function, that removes spaces (cannot use .replace method or any string method)


def get_string():
    word = input("what is the word?")
    return word


def num_vowels(s):
    new = ''
    for character in s:
        if character in "a" "A" "e" "E" "i" "I" "o" "O" "u" "U":
            new = new + character
    print(len(new))


def remove_spaces(a):
    new = ''
    for character in a:
        if character != " ":
            new = new + character
    print(new)

def remove_vowels(c):
    n = ''
    for x in c:
        if not(x in "a" "A" "e" "E" "i" "I" "o" "O" "u" "U"):
            n = n + x
    print(n)


if __name__ == "__main__":
    get_word = get_string()
    answer = num_vowels(get_word)
    other_answer = remove_spaces(get_word)
    gone = remove_vowels(get_word)
