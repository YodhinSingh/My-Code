# This is a encryption/decryption program that takes user input and returns the modified string
def choice():
    while True:         # keep asking the user for input until they give either encrypt or decrypt
        print("Do you want to encrypt or decrypt?")
        answer = input().lower()
        if answer in "encrypt e decrypt d".split():
            return answer
        else:
            print("Choose either 'encrypt' or 'e' or 'decrypt' or 'd'")

def get_word():         # store and return the words that the user wants to modify
    name = input("Type your message")
    return name

def get_shift():            # store and return the shift value. How many shifts should the encryption do
    translate = int(input("How many shifts?"))
    if your_choice == "e":
        return translate
    if your_choice == "encrypt":
        return translate
    if your_choice == "d":          # if its decrypt, then the shift value will be the other way
        return -translate
    if your_choice == "decrypt":
        return -translate


def another_ceaser(word, shift):        # this does the translation of letters
    """
    >>> another_ceaser("abread", 4)
    'e'
    """
    if len(word) > 0:
       first_letter = word[0]
       if first_letter == " ":
           return " "
       else:
           if first_letter.isupper():
               if ord(first_letter)+ shift >= 65 and ord(first_letter)+ shift <= 90:        # if A <= letter <= Z
                   trans_letter = chr(ord(first_letter)+ shift)
                   return trans_letter
               if ord(first_letter)+ shift < 65:                                 # if A > letter. Used for roundabout
                   trans_letter = chr(ord(first_letter) + shift + 26)
                   return trans_letter
               if ord(first_letter)+ shift > 90:                                # if Z < letter. Used for roundabout
                   trans_letter = chr(ord(first_letter) + shift - 26)
                   return trans_letter
           if first_letter.islower():                           # same as above, but for lowercase values
               if ord(first_letter)+ shift >= 97 and ord(first_letter)+ shift <= 122:
                   trans_letter = chr(ord(first_letter)+ shift)
                   return trans_letter
               if ord(first_letter)+ shift <= 97:
                   trans_letter = chr(ord(first_letter) + shift + 26)
                   return trans_letter
               if ord(first_letter)+ shift >= 122:
                   trans_letter = chr(ord(first_letter) + shift - 26)
                   return trans_letter

def august(word, shift, translation):       # recursive method that shifts all the letters in the word, 1 at a time
    if len(word) > 0:
        trans_letter = another_ceaser(word, shift)
        translation = translation + trans_letter        # store the new modified message
        remainder = word[1:]
        if len(remainder) > 0:              # if more letters, then do this method again from the 2nd letter onwards
            august(remainder, shift, translation)
        if len(remainder) <= 0:             # if no more letters, then print the modified text
            print(translation)



if __name__ == "__main__":
    your_choice = choice()              # choose encyrpt or decrypt
    word = get_word()                   # get phrase from user input
    shift = get_shift()                 # get shift value on how much to translate
    move = august(word, shift, "")      # do the shifting
