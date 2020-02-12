<!--
You are given a file named “names.txt” that has a list of names, each name is
a single word for the first name and a single word for the last name (or
surname). The first and last name are separated by a single space character. 
Write a PHP program that reads in the file “names.txt” and prints out a table
as follows.   The first column is the full name (both first and surname), the
second column is only the surname that has been converted to all uppercase
letters
-->

<html>
<title> Task 4 </title>
<style>
  table, td, th { border: 1px black solid; border-collapse: collapse; }
  p { padding: 0px; margin:0px; font-family: monospace;}
</style>
<body>
<h1> Task 4: PHP Program #2 </h1>
<?php  /* Write your code here */
  $myfile = file("names.txt");
  print"<table>";
  print"<tr><th><em>Full Name</em></th><th><em>Surname Only</em></th></tr>";
  for ($l = 0; $l < count($myfile); $l++) {
    $eachline = explode(" ",$myfile[$l]);
    $eachcapline = strtoupper($eachline[1]);
    print"<tr><th> $myfile[$l]</th><th> $eachcapline</th></tr>";
  }
  print"</table>";
?>

<h1> Task 4 Alternate: PHP Program #3 </h1>
<h2> Big string backwards </h2>
<?php
  $bigString = "This is a big string.";
  for ($l = strlen($bigString)-1; $l >= 0; $l--) {
    $upper = strtoupper($bigString[$l]);
    print"<p> $l = \" $upper \" </p>";
  }


?>
</body>
</html>
