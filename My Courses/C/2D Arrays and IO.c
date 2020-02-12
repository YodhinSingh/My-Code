

#include <stdio.h>
#include <string.h>
#include <ctype.h>
#include <stdlib.h>

#define ROWS 20
#define COLUMNS 30

// Use a 2D array and store user input in a table. The next row will be same user input but with modified data
// like name is all caps, age is + 10, and rate has 50% increase. Then print out table when user done inputting
// after they write xxx

int main(int argc, char *argv[])
{
     char input_table[ROWS][COLUMNS];
     int age = 0;   
     float rate = 0;
     char name[10];
     int current_row = 0;
     int i = 0;
	
     printf("Enter name age and rate: ");
     fgets(input_table[current_row], 30, stdin);   /* add a /n */
     sscanf(input_table[current_row++], "%s %d %f", name, &age, &rate);
	 
     while(strcmp(name, "xxx"))        
     {    
	 /* need to 'tokenize' the current input */	    
	
	for (i = 0; i < 10; i++){
		name[i] = toupper(name[i]);
	}
	sprintf(input_table[current_row++], "%s %d %.2f\n", name, age + 10, rate * 1.5);


        /* read again */
	printf("Enter name age and rate: ");
     	fgets(input_table[current_row], 30, stdin);    
	sscanf(input_table[current_row++], "%s %d %f", name, &age, &rate); 
     }

     printf("\nRecords generated in %s on %s %s\n", __FILE__, __DATE__, __TIME__);
     /* now display the input_table row by row */

    for (i = 0; i < current_row-1; i++){
	printf("row[%d]: %s", i, input_table[i]);
    }


     return 0;
}
