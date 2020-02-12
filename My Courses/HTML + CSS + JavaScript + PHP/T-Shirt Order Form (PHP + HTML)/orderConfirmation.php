<!-- Writes the added order from the html page into an orders.txt doc
and also prints it on the html page-->

<!DOCTYPE html>
<html>
<head>
<style>
  #previousorders {
    width: 70%;
    border: dashed 2px black;
    background-color: silver;
    padding: 10px;
  }
  #previousorders p {
      padding: 0px;
      margin-top: 0px;
      margin-bottom: 0px;
      font-family: monospace;
    }
  #container {
    width: 80%;
    padding: 10px;
    background-color: LightSkyBlue;
    margins: auto;
    border: solid 5px black;
    font-size: 1.25em;
  }
  .orderitem {
    color: blue;
    font-family: monospace;
    font-size: 1.5em;
  }
  #message  {
    background-color: silver;
    border: 2px black solid;
    font-family: monospace;
    font-size: 1.24em;
    height: 100px;
    width: 50%;
  }
</style>
</head>
<body>
  <?php
  /* turn on error message for debugging */
  ini_set('display_errors', 1); # only need to call these functions
  error_reporting(E_ALL);       # one time

  /* REMOVE comments to see all data passed to this PHP program, this is useful for debugging.*/
    print_r($_GET);


  /* Recall all data is passed from the form to this PHP file as an associative array */
  /* that is defined in variable $_GET.  This is a simple example to help you get
     started. */
    $name = $_GET["name"];
    $sizeandprice = $_GET["sizeandprice"];
    $colour = $_GET["colour"];
    $cardtype = $_GET["cardtype"];
    $cardnum = $_GET["cardnumber"];
    $date = $_GET["date"];


    /* Since "gift" is a checkbox, it ,might not be set by the user.  in this case the variable will not be defined .  You can use PHP "isset()" function to test if it was set or not in the $_GET. */
    if ( isset($_GET["gift"]) ) {
      $gift = $_GET["gift"];
      $message = $_GET["message"];
    }
    else {
      $gift = false;
      $giftmessage = "";
    }

    //rom the Forms: color, size and price, credit card type, card number, date

  ?>

   <div id="container">
   <?php

    print "<h2> Confirmation of your &quoteCat Fails Video&quote T-shirt order! </h2> \n";
    print"<p> Name: <span class=\"orderitem\">$name </span><br><br>";
    print"Item: <span class=\"orderitem\">$sizeandprice ($colour) </span><br><br>";
    print"Credit Card: <span class=\"orderitem\">$cardtype </span><br><br>";
    print"Card Number: <span class=\"orderitem\">$cardnum </span><br><br>";
    print"Expiration Date: <span class=\"orderitem\">$date </span><br><br>";
    if ($gift != false) {
      print"Gift Message <br> </p><p id=\"message\"> $message </p><p>";

    }
    print"</p>";
    $addedline= "\n$name, $sizeandprice, $colour, $cardnum";
    file_put_contents("orders.txt",$addedline,FILE_APPEND);
    /* you also need to write to the file "orders.txt" the string containing the
       following info:  name, size and price, color, cardnumber */

    /* if you print out the gift message, use class "message" defined above - see Lab document */
    ?>

  <div id="previousorders">
    <h2> Previous Orders </h2>
    <?php
      $ordersFile = file("orders.txt");
      /* This is an alternative loop syntax when you need a loop to access
         items in an array */
      /* This creates loop for the array $ordersFile, where each time the loop
         runs the current value in $ordersFile is copied to the variable $line.
        This prints out each line of the file */
      foreach ($ordersFile as $line)
      {
        print "<p> $line </p>";
      }
    ?>
  </div>
</div> <!-- end container -->

</body>
</html>
