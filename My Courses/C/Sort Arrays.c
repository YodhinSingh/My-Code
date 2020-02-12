
#include <stdio.h>
#include <string.h>

#define SIZE 40

void sortArray(char *);

// Sort the array of chars in acending order

int main ()
{   
   int i = 0;
   char arr[SIZE];
  
   fgets(arr,SIZE,stdin);
   while (strcmp(arr, "quit\n"))
   {
      arr[strlen(arr)-1] = '\0'; /* remove the trailing \n
      				    print backward*/
      sortArray(arr);
      for (i = 0; i < strlen(arr); i++){
	  printf("%c", *(arr+i));
      }  
    
      printf("\n\n");
      fgets(arr,SIZE,stdin);
      
    }
    return 0;
}


void sortArray (char * str)
{
	int len = strlen(str);
	int i;
	for (i = 0; i < len ; i++){
		char* smallest = str + i;
		int j;
		for (j = i + 1; j < len ; j++){
			if (*(smallest) > *(str+j) ){
				smallest = str+j;
			}		
		}
		int temp = *(str+i);
		*(str+i) = *smallest;
		*smallest = temp;
	}
}
