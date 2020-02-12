
#include <stdio.h>
#include <string.h>

#define SIZE 40

void printReverse (char *);
void printReverse2 (char *);
int isPalindrome (char *);
isPalindrome3 (char *);

// Various ways to check if user input is Palindrone

int main ()
{   
   int result;  char c; int i;  int count=0;
   char arr[SIZE];
  
   fgets(arr,SIZE,stdin);
   while (strcmp(arr, "quit\n"))
   {
      arr[strlen(arr)-1] = '\0'; /* remove the trailing \n
      				    print backward*/
      /*printReverse(arr);*/
      printReverse2(arr);
      for (i = 0; i < strlen(arr); i++){
         printf("%c", *(arr + i));
      }
   
      if (isPalindrome (arr)) 
         printf ("\nIs a palindrome.\n\n");
      else 
         printf ("\nNot a palindrome.\n\n");
      fgets(arr,SIZE,stdin);
      
    }
    return 0;
}


int isPalindrome (char * str)
{
	int len = strlen(str) - 1;
	int i = 0;
	while ((str+i) - (str + len) > sizeof(*str)) {
		if (*(str+i) != *(str + len)){
			return 0;
		}
		i++;
		len--;
	}
	
	return 1;
}

/* assume the \n was already removed manually*/
void printReverse(char * str)
{
	if (* (str + 1) == '\0'){
		printf("%c",  * str);
		return;	
	}
	else {
		*str++;
		printReverse(str);
		printf("%c",  * (str -1));	
	}
}

void printReverse2(char* str) /* another version Yodhin made without recursion which actually makes string reversed.*/
{
	int len = strlen(str) - 1;
	int i = 0;
	while ((str+i) - (str + len) > sizeof(*str)) {
		char temp = *(str+i);
		*(str+i) = *(str+len);
		*(str+len) = temp;
		i++;
		len--;
	}
}
int isPalindrome3 (char * str){   /* another pointer verison, which I (Teacher) like  */
    int len = strlen(str);
    char * end = str + len -1;   /* create a local pointer, which point to the end of the input */
    int i;
    while( str < end)
    {
        if ( * str != * end  ) 
           return 0;
        str ++;    /* move str to the right */
        end --;    /* move end to the left  */
     }
     return 1;
}
