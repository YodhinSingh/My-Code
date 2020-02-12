<!-- keeps track of appointments on page after pressing submit-->

<!DOCTYPE html>
<html>
<head>
<style>
  #appointments {
    width: 70%;
    margin: auto;
    border: dashed 2px black;
    background-color: silver;
    padding: 10px;
  }
  #confirmation {
    width: 70%;
    margin: auto;
    border: solid 2px black;
    padding: 10px;
    margin-bottom: 10px;
  }
  #container {
    width: 80%;
    padding: 10px;
    background-color: LightSkyBlue;
    margins: auto;
    border: solid 5px black;
    font-size: 1.25em;
  }
  #container .info {
    background-color: silver;
  }
</style>
</head>
<body>

  <?php
  /* turn on error message for debugging */
  ini_set('display_errors', 1); # only need to call these functions
  error_reporting(E_ALL);       # one time

  /* uncomment to see $_GET values. */
  /*print_r($_GET);*/

  ?>

   <div id="container">
   <div id="confirmation">
   <?php
   $Surname = $_GET["surname"];
   $Firstname = $_GET["firstname"];
   $Date = $_GET["date"];
   $first = $_GET["first"];
   $Payment = $_GET["payment"];
   $Symptom = $_GET["symptoms"];
   $additional = $_GET["additional"];

   if ($first == "on"){
     $first = "Yes";
   }
   else{
     $first = "No";
   }

   print"<h1>Booking Confirmation</h1>";
   print"<p> Name: <span class=\"info\">$Firstname $Surname </span><br><br>";
   print"<p> Date: <span class=\"info\">$Date</span> <br><br>";
   print"<p> First Time? <span class=\"info\">$first</span> <br><br>";
   print"<p> Payment/Insurance: <span class=\"info\">$Payment </span><br><br>";
   print"<p> Symptoms: <span class=\"info\">$Symptom </span><br><br>";
   print"<p> Others? <br><br> <span class=\"info\">$additional </span><br><br> </p>";

   $addedline= "$Firstname $Surname, $Date, $Symptom\n";
   file_put_contents("appointments.txt",$addedline,FILE_APPEND);

    /* write your code here */

    ?>
   </div>

    <div id="appointments">
    <h2> All previous bookings </h2>
    <?php
      # this part is provided for you.



      $appointments = file("appointments.txt");
      foreach ($appointments as $info)
      {
        print "<p> $info </p>";
      }
    ?>
  </div>
  </div>
</body>
</html>
