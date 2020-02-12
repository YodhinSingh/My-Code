<!--
Change text on html page based on random input
-->

<html>
<title> Task 3 </title>
<style>
  p { width: 75%; margin: auto; color:blue; background-color: silver;
      padding:20px;  }
</style>
<body>
<h1> Task 3: PHP Program #1 </h1>
<?php  /* Write your code here */
  $myVolume = rand(1,100);
  print"<p style=\"text-align:center\"> Volume is $myVolume </p>";
  if ($myVolume < 30) {
    print"<p style=\"font-size: 0.5em; text-align:center\"> quiet </p>";
  }
  elseif ($myVolume < 70) {
    print"<p style=\"font-size: 1.25em; text-align:center\"> normal </p>";
  }
  else {
    print"<p style=\"font-size: 3em; text-align:center\"> loud </p>";
  }

?>
</body>
</html>
